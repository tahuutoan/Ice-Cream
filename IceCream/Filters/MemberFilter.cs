using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
  
namespace IceCream.Filters
{
    public class MemberFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {{"action", "Login"}, {"controller", "Account"}});
            }
            else
            {
                var ticketInfo = FormsAuthentication.Decrypt(cookie.Value);

                filterContext.RouteData.Values["Username"] = ticketInfo?.Name;
            }
            base.OnActionExecuting(filterContext);
        }
    }
     
    public class IsMemberLoginFiler : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies[".ASPXAUTHMEMBER"];
            if (cookie == null)
            {
                filterContext.RouteData.Values["Username"] = "";
            } 
            else
            {
                var ticketInfo = FormsAuthentication.Decrypt(cookie.Value);
                filterContext.RouteData.Values["Username"] = ticketInfo?.Name;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}