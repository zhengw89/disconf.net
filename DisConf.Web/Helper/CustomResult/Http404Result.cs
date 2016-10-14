using System.Web.Mvc;

namespace DisConf.Web.Helper.CustomResult
{
    public class Http404Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 404;
        }
    }
}