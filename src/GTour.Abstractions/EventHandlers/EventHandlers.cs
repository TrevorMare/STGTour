using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Abstractions.EventHandlers
{

  public delegate void TourDeRegisteredHandler(IGTourService sender, IGTour gTour);

  public delegate void TourRegisteredHandler(IGTourService sender, IGTour gTour);

  public delegate void TourStartedHandler(IGTourService sender, IGTour gTour);

  public delegate void TourStartingHandler(IGTourService sender, IGTour gTour);

  public delegate void TourCanceledHandler(IGTourService sender, IGTour gTour);
  
  public delegate void TourCancelingHandler(IGTourService sender, IGTour gTour);

  public delegate void TourCompletingHandler(IGTourService sender, IGTour gTour);
  
  public delegate void TourCompletedHandler(IGTourService sender, IGTour gTour);

}
