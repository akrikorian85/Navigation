using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace EnviventDotNet.Navigation
{
  /// <summary>
  /// A HTML navigation builder that uses JSON to create an unordered list of navigation links
  /// </summary>
  public class Navigation
  {
    /// <summary>
    /// List of Page objects that are created from JSON
    /// </summary>
    private List<Page> Pages { get; set; }
    /// <summary>
    /// Path of the Http request
    /// </summary>
    private string CurrentPath { get; set; }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path">String The path to the JSON file</param>
    public Navigation(string path)
    {
      Pages = JsonToPages(path);
      CurrentPath = HttpContext.Current.Request.Path;
      SetActivePage(Pages);
    }
    /// <summary>
    /// Converts JSON to a list of Page objects to be worked with 
    /// </summary>
    /// <param name="path">String The path to the JSON file</param>
    /// <returns>List List of Page objects</returns>
    private List<Page> JsonToPages(string path)
    {
      using (StreamReader reader = new StreamReader(path))
      {
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<List<Page>>(json);
      }
    }
    /// <summary>
    /// Sets the active page by comparing each page's URL with the current requested URL. It also sets a flag for the active page's descendants for CSS styling
    /// </summary>
    /// <param name="pages"> A list of Page objects</param>
    /// <param name="parentPage">The parent Page object</param>
    private void SetActivePage(List<Page> pages, Page parentPage = null)
    {

      foreach (var page in pages)
      {
        if (page.ChildPages != null)
        {
          SetActivePage(page.ChildPages, page);
        }

        page.IsActive = page.Href.ToLower() == CurrentPath.ToLower();

        // assigns all the ancestors' of the active page property HasActiveChild to true to flag BuildMenu to add a CSS class
        if (page.IsActive || page.HasActiveChild)
        {
          if (parentPage != null)
          {
            parentPage.HasActiveChild = true;
          }
        }
      }
    }
    /// <summary>
    /// Builds a the nav as a string that is an HTML unordered list
    /// </summary>
    /// <param name="pages">List<Page> A list of Page objects</param>
    /// <param name="isSubmenu">Boolean A flag that tells the builder that the menu is a submenu (not at the root of the menu tree)</param>
    /// <returns>String The nav as an unordered list</returns>
    private string BuildMenu(List<Page> pages, bool isSubmenu = false)
    {
      string buffer = "";
      buffer += isSubmenu ? "<ul class='submenu'>" : "<ul class='menu'>";

      foreach (var page in pages)
      {
        string classes = GetClasses(page);
        buffer += "<li";

        buffer += !String.IsNullOrEmpty(classes) ? " class='" + classes + "'" : "";
        buffer += ">";
        buffer += "<a href='" + page.Href + "'";
        buffer += page.Target != null ? " target='" + page.Target + "'" : "";
        buffer += ">" + page.Text + "</a>";

        if (page.ChildPages != null)
        {
          string submenu = BuildMenu(page.ChildPages, true);
          buffer += submenu;
        }
        buffer += "</li>";
      }
      buffer += "</ul>";

      return buffer;
    }
    /// <summary>
    /// Checks all flags of the Page objects and returns a list of HTML classes
    /// </summary>
    /// <param name="page">Page The page data</param>
    /// <returns>String a string of HTML classes that are seperated by a space</returns>
    private string GetClasses(Page page)
    {
      List<string> classes = new List<string>();

      if (page.IsActive)
      {
        classes.Add("active");
      }
      if (page.HasActiveChild)
      {
        classes.Add("has-active-child");
      }
      if (page.Class != null)
      {
        classes.Add(page.Class);
      }
      if (page.ChildPages != null)
      {
        classes.Add("has-children");
      }

      return String.Join(" ", classes);
    }
    /// <summary>
    /// Returns an HTML encoded string of the nav
    /// </summary>
    /// <returns>HtmlString the nav</returns>
    public HtmlString Render()
    {
      HtmlString menu = new HtmlString(BuildMenu(Pages));
      
      return menu;
    }
  }
}