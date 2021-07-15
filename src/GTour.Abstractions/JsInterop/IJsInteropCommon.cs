using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Abstractions.JsInterop
{
  public interface IJsInteropCommon
  {

    ValueTask<string> ScrollToElement(string elementSelector);

    ValueTask<string> AddClassToElement(string elementSelector, string className);

    ValueTask<string> RemoveClassFromElement(string elementSelector, string className);

  }
}
