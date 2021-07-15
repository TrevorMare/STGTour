using GTour.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Themes
{
  public class Bootstrap : ITheme
  {
    public string GTourOverlay { get; set; }
    public string GTourWrapper { get; set; }
    public string GTourArrow { get; set; }
    public string GTourStepWrapper { get; set; } = "modal-content ";
    public string GTourStepHeaderWrapper { get; set; } = "modal-header ";
    public string GTourStepContentWrapper { get; set; } = "modal-body ";
    public string GTourStepFooterWrapper { get; set; } = "modal-footer ";
    public string GTourStepHeaderTitle { get; set; } = "modal-title ";
    public string GTourStepCancelButton { get; set; } = "btn btn-warning ";
    public string GTourStepPreviousButton { get; set; } = "btn btn-secondary ";
    public string GTourStepNextButton { get; set; } = "btn btn-primary ";
    public string GTourStepCompleteButton { get; set; } = "btn btn-success ";
  }
}
