using GTour.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour
{
  public abstract class GTourComponent : ComponentBase, IGTour, IDisposable
  {

    #region Properties
    [Parameter]
    public virtual string TourId { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnTourStepRegistered { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnTourStepDeRegistered { get; set; }

    [Parameter]
    public EventCallback<IGTour> OnTourStarted { get; set; }

    [Parameter]
    public EventCallback<IGTour> OnTourCompleted { get; set; }

    [Parameter]
    public EventCallback<IGTour> OnTourCanceled { get; set; }

    protected virtual IEnumerable<IGTourStep> Steps { get; private set; } = new List<IGTourStep>();

    private bool HasUnOrderedSteps => Steps?.Any(s => s.TourStepSequence.HasValue == false) ?? false;

    private IEnumerable<IGTourStep> OrderedSteps => Steps?.OrderBy(s => s.TourStepSequence);

    public int? CurrentStepIndex { get; private set; }

    public string CurrentStepName { get; private set; }

    public bool IsOnLastStep { get; private set; } = false;

    public bool IsOnFirstStep { get; private set; } = false;

    public bool IsActive 
    {
      get => _isActive;
      private set
      {
        if (_isActive != value)
        {
          _isActive = value;
          this.StateHasChanged();
        }
      } 
    }
    #endregion

    #region Members
    [Inject]
    public IGTourService TourService { get; set; }

    [Inject]
    protected ILogger<GTourComponent> Logger { get; set; }

    protected IGTourStep CurrentStep { get; private set; } = null;

    private bool _isDisposed = false;

    private bool _isActive = false;
    #endregion

    #region Methods
    internal async Task CancelTour()
    {
      await CleanupTour();

      this.IsActive = false;

      this.CurrentStep = null;
      this.CurrentStepName = null;
      this.CurrentStepIndex = null;

      await this.OnTourCanceled.InvokeAsync(this);
    }

    internal async Task CompleteTour()
    {
      await CleanupTour();

      this.IsActive = false;

      this.CurrentStep = null;
      this.CurrentStepName = null;
      this.CurrentStepIndex = null;

      await this.OnTourCompleted.InvokeAsync(this);
    }

    internal async Task GoToStep(string stepName)
    {
      int? stepIndex = GetStepWithNameIndex(stepName);
      if (stepIndex.HasValue)
      {
        await SetTourStep(stepIndex.Value);
      }
    }

    internal async Task NextStep()
    {
      int? nextStepIndex = GetNextStepIndex();
      if (nextStepIndex.HasValue)
      {
        await SetTourStep(nextStepIndex.Value);
      }
    }

    internal async Task PreviousStep()
    {
      int? previousStepIndex = GetPreviousStepIndex();
      if (previousStepIndex.HasValue)
      {
        await SetTourStep(previousStepIndex.Value);
      }
    }

    internal async Task StartTour()
    {
      this.IsActive = true;
      
      this.CurrentStep = null;
      this.CurrentStepName = null;
      this.CurrentStepIndex = null;

      this.Steps.ToList().ForEach(async (step) => await step.Initialise());

      await this.OnTourStarted.InvokeAsync(this);

      await this.NextStep();

      StateHasChanged();
    }
    #endregion

    #region Register/Unregister
    protected override void OnInitialized()
    {
      this.TourService.RegisterTour(this);
      base.OnInitialized();
    }

    public void Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (_isDisposed == false && disposing)
      {
        CleanupTour();

        TourService?.DeRegisterTour(this);
        _isDisposed = true;
      }
    }
    #endregion

    #region Internal Methods

    /// <summary>
    /// Sets the Tour Step
    /// </summary>
    /// <param name="index"></param>
    /// <param name="onlyVisibleSteps"></param>
    /// <returns></returns>
    protected virtual async Task SetTourStep(int index)
    {
      var steps = (this.HasUnOrderedSteps ? this.Steps : this.OrderedSteps).ToList();

      if (this.CurrentStep != null)
      {
        await this.CurrentStep.DeActivate();
      }

      this.CurrentStep = steps[index];
      this.CurrentStepIndex = index;
      this.CurrentStepName = this.CurrentStep.StepName;

      this.IsOnFirstStep = (this.GetPreviousStepIndex() == null);
      this.IsOnLastStep = (this.GetNextStepIndex() == null);

      await this.CurrentStep.Activate(this.IsOnFirstStep, this.IsOnLastStep);
    }

    /// <summary>
    /// Calculates the previous step index from the visible steps
    /// </summary>
    /// <returns></returns>
    protected virtual int? GetPreviousStepIndex()
    {
      int? previousIndex = null;
      var steps = (this.HasUnOrderedSteps ? this.Steps : this.OrderedSteps).ToList();

      Logger?.LogInformation($"{nameof(GetPreviousStepIndex)}: Calculating previous step index.");

      if (steps != null && steps.Count() > 0)
      {
        if ((CurrentStepIndex ?? 0) == 0)
        {
          Logger?.LogInformation($"{nameof(GetPreviousStepIndex)}: No previous step, Tour is already at the start");
        }
        else
        {

          for (int i = CurrentStepIndex.Value - 1; i >= 0; i--)
          {
            if (steps[i].SkipStep == false)
            {
              previousIndex = i;
              break;
            }
          }
        }
      }
      else
        Logger?.LogInformation($"{nameof(GetPreviousStepIndex)}: No steps defined for the tour");

      return previousIndex;
    }

    /// <summary>
    /// Calculates the next step index from the visible steps
    /// </summary>
    /// <returns></returns>
    protected virtual int? GetNextStepIndex()
    {
      int? nextIndex = null;
      var steps = (this.HasUnOrderedSteps ? this.Steps : this.OrderedSteps).ToList();
      Logger?.LogInformation($"{nameof(GetNextStepIndex)}: Calculating next step index.");

      if (steps != null && steps.Count() > 0)
      {
        int fromStep = (CurrentStepIndex.HasValue ? CurrentStepIndex.Value + 1 : 0);

        for (int i = fromStep; i < steps.Count(); i++)
        {
          if (steps[i].SkipStep == false)
          {
            nextIndex = i;
            break;
          }
        }
        if (nextIndex.HasValue == false)
        {
          Logger?.LogInformation($"{nameof(GetNextStepIndex)}: No next step, Tour is already at the end");
        }
      }
      else
        Logger?.LogInformation($"{nameof(GetNextStepIndex)}: No steps defined for the tour");

      return nextIndex;
    }

    /// <summary>
    /// Calculates the step index from the all the steps regardless if it is not shown
    /// </summary>
    /// <param name="stepName"></param>
    /// <returns></returns>
    protected virtual int? GetStepWithNameIndex(string stepName)
    {

      if (string.IsNullOrEmpty(stepName))
        throw new ArgumentNullException(nameof(stepName));

      int? result = null;
      int localIndex = 0;
      var steps = (this.HasUnOrderedSteps ? this.Steps : this.OrderedSteps).ToList();
      // Search the visible steps for the step name index
      foreach (var step in steps)
      {
        if (step.StepName.ToLower() == stepName.ToLower())
        {
          result = localIndex;
          break;
        }
        localIndex++;
      }

      return result;
    }

    /// <summary>
    /// Registers the Step in the tour
    /// </summary>
    /// <param name="step"></param>
    protected void AddGTourStep(IGTourStep step)
    {
      var steps = this.Steps.ToList();
      steps.Add(step);

      this.OnTourStepRegistered.InvokeAsync(step);

      this.Steps = steps;
    }

    /// <summary>
    /// DeRegisters the Step in the tour
    /// </summary>
    /// <param name="step"></param>
    protected void RemoveGTourStep(IGTourStep step)
    {
      var steps = this.Steps.ToList();
      steps.Remove(step);

      this.OnTourStepDeRegistered.InvokeAsync(step);

      this.Steps = steps;
    }

    protected abstract Task CleanupTour();
    
    #endregion

  }
}
