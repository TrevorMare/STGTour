using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Interops
{
  public class JsInteropPopper : Abstractions.JsInterop.IJsInteropPopper, IAsyncDisposable
  {

    #region Members
    private const string _basePath = "./_content/STGTour.GTour/js/JsInteropPopper.min.js";
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    #endregion

    #region ctor
    public JsInteropPopper(IJSRuntime jsRuntime)
    {
      _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", _basePath).AsTask());
    }
    #endregion

    #region Methods
    public async ValueTask<string> SetTourStepPopperByElement(ElementReference forElement, ElementReference gtourWrapper, string placement, string strategy)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("SetTourStepPopperByElement", forElement, gtourWrapper, placement, strategy);
    }

    public async ValueTask<string> SetTourStepPopperBySelector(string forElementSelector, ElementReference gtourWrapper, string placement, string strategy)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("SetTourStepPopperBySelector", forElementSelector, gtourWrapper, placement, strategy);
    }

    public async ValueTask<string> ResetTourStepPopper(ElementReference gtourWrapper, string placement, string strategy)
    {
      var module = await _moduleTask.Value;
      return await module.InvokeAsync<string>("ResetTourStepPopper", gtourWrapper, placement, strategy);
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
