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

      serviceCollection.AddScoped<Abstractions.JsInterop.IJsInteropPopper, Interops.JsInteropPopper>();
      serviceCollection.AddScoped<Abstractions.JsInterop.IJsInteropCommon, Interops.JsInteropCommon>();

      return serviceCollection;
    }

  }
}
