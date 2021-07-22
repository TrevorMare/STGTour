using GTour.Abstractions.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Abstractions
{
  public interface IGTourService
  {

    #region Events
    event TourRegisteredHandler OnTourRegistered;
    event TourDeRegisteredHandler OnTourDeRegistered;
    event TourStartingHandler OnTourStarting;
    event TourStartedHandler OnTourStarted;
    event TourCancelingHandler OnTourCanceling;
    event TourCanceledHandler OnTourCanceled;
    event TourCompletingHandler OnTourCompleting;
    event TourCompletedHandler OnTourCompleted;
    #endregion

    #region Properties
    bool ThrowOnTourNotFound { get; set; } 

    IGTour CurrentTour { get; }
    #endregion

    #region Methods
    void RegisterTour(Abstractions.IGTour gTour);
    
    Task StartTour(string tourId, string startStepName = default);
    
    Task StartTour(Abstractions.IGTour gTour, string startStepName = default);

    Task StopTour();

    Task CancelTour();

    Task PreviousStep();

    Task NextStep();

    Task GoToStep(string stepName);

    Task CompleteTour();
    
    void DeRegisterTour(IGTour gTour);
    #endregion

  }
}
