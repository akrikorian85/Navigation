using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace MyFramework.Navigation
{
  public class Navigation
  {
    private List<Page> Pages { get; set; }
    private string CurrentPath { get; set; }

    public Navigation(string path)
    {
      Pages = GetJson(path);
      CurrentPath = HttpContext.Current.Request.Path;
      SetActivePage(Pages);
    }
    private List<Page> GetJson(string path)
    {
      using (StreamReader reader = new StreamReader(path))
      {
        string json = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<List<Page>>(json);
      }
    }
    private void SetActivePage(List<Page> pages, Page parentPage = null)
    {

      foreach (var page in pages)
      {
        if (page.ChildPages != null)
        {
          SetActivePage(page.ChildPages, page);
        }

        page.IsActive = page.Link.ToLower() == CurrentPath.ToLower();

        if (page.IsActive || page.HasActiveChild)
        {
          if (parentPage != null)
          {
            parentPage.HasActiveChild = true;
          }
        }
      }
    }
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
        buffer += "<a href='" + page.Link + "'";
        buffer += page.Target != null ? " target='" + page.Target + "'" : "";
        buffer += ">" + page.Title + "</a>";

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
    public HtmlString Render()
    {
      HtmlString menu = new HtmlString(BuildMenu(Pages));

      return menu;
    }
  }
}