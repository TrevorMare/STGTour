using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Abstractions
{
  public interface IGTour
  {

    #region Properties
    string TourId { get; set; }

    bool IsOnLastStep { get; }

    bool IsOnFirstStep { get; }

    int? CurrentStepIndex { get; }

    string CurrentStepName { get; }
    #endregion

  }
}
