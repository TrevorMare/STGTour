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
  public partial class GuidedTour : GTourComponent
  {

    #region Properties
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; }

    [Inject]
    internal Abstractions.JsInterop.IJsInteropPopper JsInteropPopper { get; set; }

    [Inject]
    internal Abstractions.JsInterop.IJsInteropCommon JsInteropCommon { get; set; }

    internal ElementReference OverlayElement { get; set; }

    internal ElementReference WrapperElement { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string OverlayClass { get; set; }

    [Parameter]
    public string ArrowClass { get; set; }

    [Parameter]
    public RenderFragment ArrowContent { get; set; }

    [Parameter]
    public bool CloseOnOverlayClick { get; set; } = false;

    [Parameter]
    public bool OverlayEnabled { get; set; } = true;

    [Parameter]
    public bool ShowArrow { get; set; } = true;

    [Parameter]
    public string TourWrapperClass { get; set; }

    [Parameter]
    public string HighlightClass { get; set; } = "element-highlight";

    [Parameter]
    public bool HighlightEnabled { get; set; } = true;

    public ITheme SelectedTheme => GTourService.Theme;
    #endregion

    #region Methods
    internal void StepUnRegistered(GuidedTourStep guidedTourStep)
    {
      if (Steps != null)
      {
        base.RemoveGTourStep(guidedTourStep);
      }
    }

    internal void StepRegistered(GuidedTourStep guidedTourStep)
    {
      if (Steps != null)
      {
        base.AddGTourStep(guidedTourStep);
      }
    }

    internal async Task JsInteropStart(GuidedTourStep guidedTourStep)
    {
      if (guidedTourStep == null)
      {
        Logger?.LogWarning($"{nameof(JsInteropStart)}: Guided tour step is null");
        return;
      }
      
      if (!string.IsNullOrEmpty(guidedTourStep.ElementSelector))
      {
        await JsInteropCommon.ScrollToElement(guidedTourStep.ElementSelector);

        await JsInteropPopper.SetTourStepPopperBySelector(guidedTourStep.ElementSelector, this.WrapperElement, guidedTourStep.PopupPlacement.GetEnumDisplay(), guidedTourStep.PopupStrategy.GetEnumDisplay());

        if (this.HighlightEnabled && !string.IsNullOrEmpty(this.HighlightClass))
        {
          await JsInteropCommon.AddClassToElement(guidedTourStep.ElementSelector, this.HighlightClass);
        }

      }
      else
      {
        await JsInteropPopper.ResetTourStepPopper(this.WrapperElement, guidedTourStep.PopupPlacement.GetEnumDisplay(), guidedTourStep.PopupStrategy.GetEnumDisplay());
      }
    }

    internal async Task JsInteropEnd(GuidedTourStep guidedTourStep)
    {
      if (guidedTourStep == null)
      {
        Logger?.LogWarning($"{nameof(JsInteropEnd)}: Guided tour step is null");
        return;
      }

      if (this.HighlightEnabled && !string.IsNullOrEmpty(this.HighlightClass))
      {
        await JsInteropCommon.RemoveClassFromElement(guidedTourStep.ElementSelector, this.HighlightClass);
      }
    }

    protected override async Task CleanupTour()
    {
      if (this.CurrentStep != null)
      {
        await JsInteropEnd(this.CurrentStep as GuidedTourStep);
      }
    }
    #endregion

  }
}
