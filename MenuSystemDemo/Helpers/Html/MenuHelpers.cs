using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MenuSystemDemo.GeneratedClasses;
using WebGrease.Css.Extensions;

namespace MenuSystemDemo.Helpers.Html
{
    public static class MenuHelpers
    {
        #region Menus

        #region Horizontal Menu

        public static IHtmlString HorizontalMenu(this HtmlHelper helper,
            IEnumerable<MenuItem> items)
        {
            if (items == null || !items.Any())
            {
                return new HtmlString(String.Empty);
            }
            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-inline list-unstyled");

            var sb = new StringBuilder();
            items.ForEach(e => CreateMenuItem(e, sb));

            ul.InnerHtml = sb.ToString();

            return new HtmlString(ul.ToString(TagRenderMode.Normal));
        }

        #endregion

        #region Vertical Menu

        public static IHtmlString VerticalMenu(this HtmlHelper helper,
            IEnumerable<MenuItem> items)
        {
            if (items == null || !items.Any())
                return new HtmlString(String.Empty);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-unstyled");

            var sb = new StringBuilder();
            items.ForEach(e => CreateMenuItem(e, sb));

            ul.InnerHtml = sb.ToString();

            return new HtmlString(ul.ToString(TagRenderMode.Normal));
        }

        #endregion


        private static void CreateMenuItem(MenuItem menuItem, StringBuilder sb)
        {
            if (String.IsNullOrEmpty(menuItem.Url))
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml = $"<i class=\"fa fa-fw fa-{menuItem.Icon}\"></i> {menuItem.Title}"
                };
                sb.Append(li.ToString(TagRenderMode.Normal));
            }
            else
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml =
                        $"<a href=\"{menuItem.Url}\" title=\"{menuItem.Description}\"><i class=\"fa fa-fw fa-{menuItem.Icon}\"></i> {menuItem.Title}</a>"
                };
                sb.Append(li.ToString(TagRenderMode.Normal));
            }
        }

        #endregion

        #region Toolbar

        public static IHtmlString Toolbar(this HtmlHelper helper,
            IEnumerable<MenuItem> items)
        {
            if (items == null || !items.Any())
                return new HtmlString(String.Empty);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("list-inline list-unstyled toolbar");

            var sb = new StringBuilder();
            items.ForEach(e => CreateToolbarItem(e, sb));

            ul.InnerHtml = sb.ToString();
            return new HtmlString(ul.ToString(TagRenderMode.Normal));
        }

        private static void CreateToolbarItem(MenuItem menuItem, StringBuilder sb)
        {
            if (String.IsNullOrEmpty(menuItem.Url))
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml = $"<i title=\"{menuItem.Description}\" class=\"fa fa-{menuItem.Icon}\"></i>"
                };
                sb.Append(li.ToString(TagRenderMode.Normal));
            }
            else
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml =
                        $"<a class=\"btn btn-default btn-sm\" href=\"{menuItem.Url}\" " +
                        $"title=\"{menuItem.Description}\"><i class=\"fa fa-{menuItem.Icon}\"></i></a>"
                };
                sb.Append(li.ToString(TagRenderMode.Normal));
            }
        }
        #endregion

    }
}