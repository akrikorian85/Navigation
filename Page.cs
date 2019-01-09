using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFramework.Navigation
{
  /// <summary>
  /// Represents a navigation link for the Navigation object, eventually being outputted as HTML
  /// </summary>
  public class Page
  {
    /// <summary>
    /// The text of the anchor tag
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// The value of the href attribute in the anchor tag
    /// </summary>
    public string Href { get; set; }
    /// <summary>
    /// The value of the target attribute in the anchor tag
    /// </summary>
    public string Target { get; set; }
    /// <summary>
    /// The value of the class attribute in the anchor tag
    /// </summary>
    public string Class { get; set; }
    /// <summary>
    /// Flag used for assigning whether this page has a child element that is 'active'
    /// </summary>
    public bool HasActiveChild { get; set; }
    /// <summary>
    /// Flag used for assigning whether this page is the active page
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// A list of child Page objects. Used with other Page objects, it creates a tree-like navigation structure
    /// </summary>
    public List<Page> ChildPages { get; set; }
  }
}