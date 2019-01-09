using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFramework.Navigation
{
  public class Page
  {
    public string Title { get; set; }
    public string Link { get; set; }
    public string Target { get; set; }
    public string Class { get; set; }
    public bool HasActiveChild { get; set; }
    public bool IsActive { get; set; }
    public List<Page> ChildPages { get; set; }
  }
}