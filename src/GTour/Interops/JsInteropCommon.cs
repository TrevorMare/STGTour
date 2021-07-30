using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Interops
{
  public class JsInteropCommon : Abstractions.JsInterop.IJsInteropCommon, IAsyncDisposable
  {

    #region Members
    private const string _basePath = "./_content/STGTour.GTour/js/JsInteropCommon.min.js";
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    #endregion

    #region ctor
    public JsInteropCommon(IJSRuntime jsRuntime)
    {
      _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", _basePath).AsTask());
    }
    #endregion

    #region Methods
    public async ValueTask<string> ScrollToElement(string elementSelector)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("ScrollToElement", elementSelector);
    }

    public async ValueTask<string> AddClassToElement(string elementSelector, string className)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("AddClassToElement", elementSelector, className);
    }


    public async ValueTask<string> RemoveClassFromElement(string elementSelector, string className)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("RemoveClassFromElement", elementSelector, className);
    }
    #endregion

    #region Dispose
    public async ValueTask DisposeAsync()
    {
      if (_moduleTask.IsValueCreated)
      {
        var module = await _moduleTask.Value;
        await module.DisposeAsync();
      }
    }

    #endregion

  }
}
