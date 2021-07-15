using GTour.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Themes
{
  public class Default : ITheme
  {
    public string GTourOverlay { get; set; } = "theme-default-overlay ";
    public string GTourWrapper { get; set; } = "theme-default-wrapper ";
    public string GTourArrow { get; set; } = "theme-default-arrow ";
    public string GTourStepWrapper { get; set; } = "theme-default-step-wrapper ";
    public string GTourStepHeaderWrapper { get; set; } = "theme-default-step-header ";
    public string GTourStepContentWrapper { get; set; } = "theme-default-step-content ";
    public string GTourStepFooterWrapper { get; set; } = "theme-default-step-footer ";
    public string GTourStepHeaderTitle { get; set; } = "theme-default-step-header-title ";
    public string GTourStepCancelButton { get; set; } = "theme-default-button cancel-button ";
    public string GTourStepPreviousButton { get; set; } = "theme-default-button previous-button ";
    public string GTourStepNextButton { get; set; } = "theme-default-button next-button ";
    public string GTourStepCompleteButton { get; set; } = "theme-default-button complete-button ";
  }
}
