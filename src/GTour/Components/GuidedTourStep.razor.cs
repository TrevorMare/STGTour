using GTour.Abstractions;
using GTour.Abstractions.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Components
{
  public partial class GuidedTourStep : GTourStepComponent
  {

    #region Members
    private bool _disposed = false;
    #endregion

    #region Parameters
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; }

    protected ElementReference StepWrapperElement { get; set; }

    [CascadingParameter]
    public GuidedTour ParentComponent { get; set; }

    /// <summary>
    /// Gets or sets the Main wrapper class
    /// </summary>
    [Parameter]
    public string WrapperClass { get; set; }

    /// <summary>
    /// Gets or sets the Header wrapper class 
    /// </summary>
    [Parameter]
    public string HeaderClass { get; set; }

    /// <summary>
    /// Gets or sets the Content wrapper class
    /// </summary>
    [Parameter]
    public string ContentClass { get; set; }

    /// <summary>
    /// Gets or sets the Footer wrapper class
    /// </summary>
    [Parameter]
    public string FooterClass { get; set; }

    /// <summary>
    /// Gets or sets the Cancel Tour Button Class
    /// </summary>
    [Parameter]
    public string CancelTourButtonClass { get; set; }

    /// <summary>
    /// Gets or sets the Previous Step Button Class
    /// </summary>
    [Parameter]
    public string PreviousStepButtonClass { get; set; }

    /// <summary>
    /// Gets or sets the Next Step Button Class
    /// </summary>
    [Parameter]
    public string NextStepButtonClass { get; set; }

    /// <summary>
    /// Gets or sets the Complete Tour Button Class
    /// </summary>
    [Parameter]
    public string CompleteTourButtonClass { get; set; }

    /// <summary>
    /// Gets or sets the Cancel Tour Button Class
    /// </summary>
    [Parameter]
    public string CancelTourButtonText { get; set; } = "Cancel Tour";

    /// <summary>
    /// Gets or sets the Previous Step Button Class
    /// </summary>
    [Parameter]
    public string PreviousStepButtonText { get; set; } = "Previous";

    /// <summary>
    /// Gets or sets the Next Step Button Class
    /// </summary>
    [Parameter]
    public string NextStepButtonText { get; set; } = "Next";

    /// <summary>
    /// Gets or sets the Next Step Button Class
    /// </summary>
    [Parameter]
    public string CompleteTourButtonText { get; set; } = "Finish";

    /// <summary>
    /// Gets or sets the title for the step
    /// </summary>
    [Parameter]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the optional header content for the step
    /// </summary>
    [Parameter]
    public RenderFragment<IGTourStep> HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the Footer Content 
    /// </summary>
    [Parameter]
    public RenderFragment<IGTourStep> FooterContent { get; set; }

    /// <summary>
    /// Gets or sets the element selector for this tooltip
    /// </summary>
    [Parameter]
    public string ElementSelector { get; set; }

    /// <summary>
    /// Gets or sets the Child Content of the step
    /// </summary>
    [Parameter]
    public RenderFragment<IGTourStep> ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the Popup Orientation
    /// </summary>
    [Parameter]
    public PopperPlacement PopupPlacement { get; set; } = PopperPlacement.Auto;

    /// <summary>
    /// Gets or sets the Popup Orientation
    /// </summary>
    [Parameter]
    public PopperStrategy PopupStrategy { get; set; } = PopperStrategy.Absolute;

    /// <summary>
    /// Gets or sets a value if the tour can be cancelled
    /// </summary>
    [Parameter]
    public bool CanCancelTour { get; set; } = true;

    public ITheme SelectedTheme => GTour.GTourService.Theme;
    #endregion

    #region Methods
    protected override void OnInitialized()
    {
      if (ParentComponent != null)
      {
        ParentComponent.StepRegistered(this);

        if (this.TourStepSequence.HasValue == false)
        {
          Logger?.LogWarning($"It is recommended that the tour step sequence be set for step {this.StepName}");
        }
      }
    }

    private async Task OnCancelTourClick()
    {
      await CancelTour();
    }

    private async Task OnNextStepClick()
    {
      await NextStep();
    }

    private async Task OnPreviousStepClick()
    {
      await PreviousStep();
    }

    private async Task OnCompleteTourClick()
    {
      await CompleteTour();
    }

    protected override async Task RunActivation()
    {
      await ParentComponent.JsInteropStart(this);
    }

    protected override async Task RunDeActivation()
    {
      await ParentComponent.JsInteropEnd(this);
    }
    #endregion

    #region Dispose

    protected override void Dispose(bool disposing)
    {
      if (disposing == true && _disposed == false)
      {
        _disposed = true;
        ParentComponent.StepUnRegistered(this);
      }
    }
    #endregion

  }
}
