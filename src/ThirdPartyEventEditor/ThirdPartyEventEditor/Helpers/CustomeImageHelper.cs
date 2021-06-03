using System.Web.Mvc;

namespace ClassicMvc.Helpers
{
    public static class CustomeImageHelper
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string src)
        {
            var builder = new TagBuilder("img");

            builder.MergeAttribute("src", src);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}