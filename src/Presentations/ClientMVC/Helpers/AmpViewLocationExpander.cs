using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace ClientMVC.Helpers
{
    public class AmpViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (!(context.ActionContext.ActionDescriptor is ControllerActionDescriptor descriptor)) { return viewLocations; }

            if (context.ActionContext.HttpContext.Request.Query.ContainsKey("amp")
                || context.ActionContext.HttpContext.Request.Path.StartsWithSegments("/amp"))
            {
                return viewLocations.Select(x => x.Replace("{0}", "{0}.amp"));
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var contains = context.ActionContext.HttpContext.Request.Query.ContainsKey("amp");
            context.Values.Add("AmpKey", contains.ToString());

            var containsStem = context.ActionContext.HttpContext.Request.Path.StartsWithSegments("/amp");
            context.Values.Add("AmpStem", containsStem.ToString());
        }
    }
}
