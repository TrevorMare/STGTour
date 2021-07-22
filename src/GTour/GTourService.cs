using GTour.Abstractions;
using GTour.Abstractions.EventHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour
{
    /// <summary>
    /// Orchestrator service for GTours
    /// </summary>
    public class GTourService : Abstractions.IGTourService
    {

        #region Theme
        private static ITheme _theme = new Themes.Default();

        public static ITheme Theme
        {
            get => _theme;
            set
            {
                if (value == null)
                {
                    throw new Exception("Please set the theme to None instead of null");
                }
                _theme = value;
            }
        }
        #endregion

        #region Events
        public event TourRegisteredHandler OnTourRegistered;

        public event TourDeRegisteredHandler OnTourDeRegistered;

        public event TourStartedHandler OnTourStarted;

        public event TourStartingHandler OnTourStarting;

        public event TourCancelingHandler OnTourCanceling;

        public event TourCanceledHandler OnTourCanceled;

        public event TourCompletedHandler OnTourCompleted;

        public event TourCompletingHandler OnTourCompleting;

        #endregion

        #region Properties
        public bool ThrowOnTourNotFound { get; set; } = false;

        public IGTour CurrentTour { get; private set; } = null;
        #endregion

        #region Members
        private readonly ILogger<GTourService> _logger;

        private List<IGTour> _registeredTours = new List<IGTour>();

        private GTourComponent _currentTourComponent => CurrentTour as GTourComponent;
        #endregion

        #region ctor
        public GTourService(ILogger<GTourService> logger)
        {
            this._logger = logger;
        }


        #endregion

        #region Public Methods
        /// <summary>
        /// Registers a tour with the service
        /// </summary>
        /// <param name="gTour"></param>
        /// <returns></returns>
        public void RegisterTour(IGTour gTour)
        {
            try
            {
                if (gTour == null)
                    throw new ArgumentNullException(nameof(gTour));

                if (string.IsNullOrEmpty(gTour.TourId) || gTour.TourId.Trim() == "")
                    throw new Exception("Unable to register the tour as the Tour Id is a required field.");

                var isTourRegistered = this._registeredTours.Any(t => t.TourId.ToLower() == gTour.TourId.ToLower());

                if (isTourRegistered == false)
                {
                    this._logger?.LogInformation($"{nameof(RegisterTour)}: Registering tour with Id {gTour.TourId}");
                    this._registeredTours.Add(gTour);
                    this.OnTourRegistered?.Invoke(this, gTour);
                }
                else
                {
                    this._logger?.LogInformation($"{nameof(RegisterTour)}: Tour with Id {gTour.TourId} already registered");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        public void DeRegisterTour(IGTour gTour)
        {
            try
            {
                if (gTour == null)
                    throw new ArgumentNullException(nameof(gTour));

                if (string.IsNullOrEmpty(gTour.TourId) || gTour.TourId.Trim() == "")
                    throw new Exception("Unable to deregister the tour as the Tour Id is a required field.");

                var isTourRegistered = this._registeredTours.Any(t => t.TourId.ToLower() == gTour.TourId.ToLower());

                if (isTourRegistered == true)
                {
                    this._logger?.LogInformation($"{nameof(DeRegisterTour)}: Deregistering tour with Id {gTour.TourId}");
                    this._registeredTours.Remove(gTour);
                    this.OnTourDeRegistered?.Invoke(this, gTour);
                }
                else
                {
                    this._logger?.LogInformation($"{nameof(RegisterTour)}: Tour with Id {gTour.TourId} already deregistered");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Starts a registered tour
        /// </summary>
        /// <param name="tourId">The tour Id to start. The tour must be registered</param>
        /// <param name="startStepName">The optional starting step name</param>
        /// <returns></returns>
        public async Task StartTour(string tourId, string startStepName = default)
        {
            try
            {
                if (string.IsNullOrEmpty(tourId) || tourId.Trim() == "")
                    throw new ArgumentNullException(nameof(tourId));

                // Find the Tour in the registered Tours
                var gTour = this._registeredTours.FirstOrDefault(t => t.TourId.ToLower() == tourId.ToLower());

                if (gTour == null && this.ThrowOnTourNotFound == true)
                    throw new Exception($"Unable to locate a registered tour with the name {tourId}");

                await this.StartTour(gTour, startStepName);
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }

        }

        /// <summary>
        /// Starts a tour
        /// </summary>
        /// <param name="gTour">The tour to start</param>
        /// <param name="startStepName">The optional starting step of the tour</param>
        /// <returns></returns>
        public async Task StartTour(IGTour gTour, string startStepName = default)
        {
            try
            {
                if (gTour == null)
                {
                    if (this.ThrowOnTourNotFound == true)
                        throw new ArgumentNullException(nameof(gTour));
                    else
                    {
                        this._logger?.LogWarning($"{nameof(StartTour)}: Tour is null and cannot start");
                        return;
                    }
                }

                this._logger?.LogInformation($"{nameof(StartTour)}: Starting tour with Id {gTour.TourId}");


                if (this.CurrentTour != null)
                {
                    await this.StopTour();
                }

                OnTourStarting?.Invoke(this, gTour);

                // Set the current tour object
                this.CurrentTour = gTour;

                await this._currentTourComponent.StartTour();

                this.OnTourStarted?.Invoke(this, gTour);
                this._logger?.LogInformation($"{nameof(StartTour)}: Tour with Id {gTour.TourId} started");

                if (!string.IsNullOrEmpty(startStepName))
                {
                    await this.GoToStep(startStepName);
                }

            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Stops the current tour, if the tour is on the last step it will be completed. If the tour is not on the last step
        /// it will be canceled
        /// </summary>
        /// <returns></returns>
        public async Task StopTour()
        {

            try
            {
                if (this.CurrentTour != null)
                {
                    if (this.CurrentTour.IsOnLastStep)
                    {
                        await CompleteTour();
                    }
                    else
                    {
                        await CancelTour();
                    }
                }
                else
                {
                    this._logger?.LogWarning($"{nameof(StopTour)}: There is no current tour to stop");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception("There is no current tour to stop");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Cancels the current Tour
        /// </summary>
        /// <returns></returns>
        public async Task CancelTour()
        {
            try
            {
                if (this.CurrentTour != null)
                {
                    this._logger?.LogInformation($"{nameof(CancelTour)}: Tour with Id {CurrentTour.TourId} canceled");

                    this.OnTourCanceling?.Invoke(this, this.CurrentTour);

                    await this._currentTourComponent.CancelTour();

                    this.OnTourCanceled?.Invoke(this, this.CurrentTour);
                    this.CurrentTour = null;
                }
                else
                {
                    this._logger?.LogWarning($"{nameof(CancelTour)}: There is no current tour to cancel");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception("There is no current tour to complete");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Completes the current Tour
        /// </summary>
        /// <returns></returns>
        public async Task CompleteTour()
        {
            try
            {
                if (this.CurrentTour != null)
                {
                    this._logger?.LogInformation($"{nameof(CompleteTour)}: Tour with Id {CurrentTour.TourId} completed");

                    this.OnTourCompleting?.Invoke(this, this.CurrentTour);

                    await this._currentTourComponent.CompleteTour();

                    this.OnTourCompleted?.Invoke(this, this.CurrentTour);
                    this.CurrentTour = null;
                }
                else
                {
                    this._logger?.LogWarning($"{nameof(CompleteTour)}: There is no current tour to complete");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception("There is no current tour to complete");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Navigates the tour to the specified step
        /// </summary>
        /// <param name="stepName"></param>
        /// <returns></returns>
        public async Task GoToStep(string stepName)
        {
            try
            {
                if (CurrentTour != null)
                {
                    this._logger?.LogInformation($"{nameof(GoToStep)}: Navigating to step {stepName} on tour with Id {CurrentTour.TourId}");

                    await this._currentTourComponent.GoToStep(stepName);

                }
                else
                {
                    this._logger?.LogWarning($"{nameof(GoToStep)}: No current tour found to advance step to");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception($"There is no current tour to advance to step {stepName}");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Navigates the current tour to the next step
        /// </summary>
        /// <returns></returns>
        public async Task NextStep()
        {
            try
            {
                if (CurrentTour != null)
                {
                    this._logger?.LogInformation($"{nameof(NextStep)}: Navigating to next step on tour with Id {CurrentTour.TourId}");

                    await this._currentTourComponent.NextStep();
                }
                else
                {
                    this._logger?.LogWarning($"{nameof(NextStep)}: No current tour found to navigate to the next step");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception($"There is no current tour to navigate to the next step");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Navigates the current tour to the previous step
        /// </summary>
        /// <returns></returns>
        public async Task PreviousStep()
        {
            try
            {
                if (CurrentTour != null)
                {
                    this._logger?.LogInformation($"{nameof(PreviousStep)}: Navigating to previous step on tour with Id {CurrentTour.TourId}");

                    await this._currentTourComponent.PreviousStep();

                }
                else
                {
                    this._logger?.LogWarning($"{nameof(PreviousStep)}: No current tour found to navigate to the previous step");
                    if (this.ThrowOnTourNotFound == true)
                        throw new Exception($"There is no current tour to navigate to the previous step");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical(ex.Message, ex);
                throw;
            }
        }
        #endregion

    }
}
