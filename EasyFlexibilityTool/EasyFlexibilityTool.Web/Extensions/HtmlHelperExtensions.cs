namespace EasyFlexibilityTool.Web.Extensions
{
    #region Usings

    using System;
    using System.Drawing;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using System.Web.WebPages;

    #endregion

    public static class HtmlHelperExtensions
    {
        #region Public Static Methods

        public static string GetSelectedCss(this HtmlHelper html, string action = null, string controller = null)
        {
            // From: http://stackoverflow.com/questions/20410623/how-to-add-active-class-to-html-actionlink-in-asp-net-mvc
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (string.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }

            if (string.IsNullOrEmpty(action))
            {
                action = currentAction;
            }

            return controller == currentController && action == currentAction
                ? cssClass
                : string.Empty;
        }
        #endregion
    }
}