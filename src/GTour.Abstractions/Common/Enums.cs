using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTour.Abstractions.Common
{
  public enum PopperPlacement
  {
    [Display(Name = "none")]
    None,
    [Display(Name = "auto")]
    Auto,
    [Display(Name = "auto-start")]
    AutoStart,
    [Display(Name = "auto-end")]
    AutoEnd,
    [Display(Name = "top")]
    Top,
    [Display(Name = "top-start")]
    TopStart,
    [Display(Name = "top-end")]
    TopEnd,
    [Display(Name = "bottom")]
    Bottom,
    [Display(Name = "bottom-start")]
    BottomStart,
    [Display(Name = "bottom-end")]
    BottomEnd,
    [Display(Name = "right")]
    Right,
    [Display(Name = "right-start")]
    RightStart,
    [Display(Name = "right-end")]
    RightEnd,
    [Display(Name = "left")]
    Left,
    [Display(Name = "left-start")]
    LeftStart,
    [Display(Name = "left-end")]
    LeftEnd
  }

  public enum PopperStrategy
  {
    [Display(Name = "fixed")]
    Fixed,
    [Display(Name = "absolute")]
    Absolute
  }

  public static class EnumExtensions
  {
    public static string GetEnumDisplay(this Enum enumValue)
    {

      return enumValue.GetType()
                      .GetMember(enumValue.ToString())
                      .FirstOrDefault()?
                      .GetCustomAttribute<DisplayAttribute>()?
                      .Name ?? enumValue.ToString();
    }
  }

}
