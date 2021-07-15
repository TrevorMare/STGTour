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
  public abstract class GTourStepComponent : ComponentBase, IGTourStep, IDisposable
  {

    #region Properties

    [Inject]
    public Abstractions.IGTourService GTourService { get; set; }

    [Inject]
    protected ILogger<GTourStepComponent> Logger { get; set; }

    /// <summary>
    /// Gets or sets the Unique step name
    /// </summary>
    [Parameter]
    public string StepName { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the tour step sequence
    /// </summary>
    [Parameter]
    public int? TourStepSequence { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the step should be skipped
    /// </summary>
    [Parameter]
    public bool SkipStep { get; set; }

    public bool IsFirstStep { get; private set; }

    public bool IsLastStep { get; private set; }

    public bool IsActiveStep { get; private set; }
    #endregion

    #region Event Callbacks
    [Parameter]
    public EventCallback<IGTourStep> OnStepActivated { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnStepDeActivated { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnTourCanceled { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnTourCompleted { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnNavigatePrevious { get; set; }

    [Parameter]
    public EventCallback<IGTourStep> OnNavigateNext { get; set; }
    #endregion

    #region Methods
    public async Task CancelTour()
    {
      await this.OnTourCanceled.InvokeAsync(this);
      await this.GTourService?.CancelTour();
    }

    public async Task PreviousStep()
    {
      await this.OnNavigatePrevious.InvokeAsync(this);
      await this.GTourService?.PreviousStep();
    }

    public async Task NextStep()
    {
      await this.OnNavigateNext.InvokeAsync(this);
      await this.GTourService?.NextStep();
    }

    public async Task GoToStep(string stepName)
    {
      await this.GTourService?.GoToStep(stepName);
    }

    public async Task CompleteTour()
    {
      await this.OnTourCompleted.InvokeAsync(this);
      await this.GTourService?.CompleteTour();
    }

    public Task Initialise()
    {
      this.IsActiveStep = false;
      this.IsFirstStep = false;
      this.IsLastStep = false;

      StateHasChanged();

      return Task.CompletedTask;
    }

    protected virtual Task RunActivation()
    {
      return Task.CompletedTask;
    }

    protected virtual Task RunDeActivation()
    {
      return Task.CompletedTask;
    }

    public async Task DeActivate()
    {
      this.IsActiveStep = false;

      await this.RunDeActivation();

      await this.OnStepDeActivated.InvokeAsync(this);

      StateHasChanged();
    }

    public async Task Activate(bool isFirstStep, bool isLastStep)
    {
      this.IsActiveStep = true;
      this.IsFirstStep = isFirstStep;
      this.IsLastStep = isLastStep;

      await this.RunActivation();

      await this.OnStepActivated.InvokeAsync(this);

      StateHasChanged();
    }
    #endregion

    #region Dispose
    public void Dispose()
    {
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
    #endregion

  }
}
