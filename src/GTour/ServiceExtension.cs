using GTour.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour
{
  public static class ServiceExtension
  {

    public static IServiceCollection UseGTour(this IServiceCollection serviceCollection)
    {

      serviceCollection.AddSingleton<Abstractions.IGTourService, GTourService>();

      serviceCollection.UseGTourDependencies();

      return serviceCollection;
    }

  }
}
