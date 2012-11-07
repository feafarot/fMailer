namespace Citadels.WebUI.Configuration
{
    using System.Text;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JSResult : ActionResult
    {
        public static JSResult Empty = new JSResult(null);

        public static JSResult True = new JSResult(true);

        public static JSResult False = new JSResult(false);

        public JSResult()
        {
        }

        public JSResult(object data)
        {
            Data = data;
        }

        public virtual object Data { get; private set; }

        public static implicit operator JSResult(bool obj)
        {
            return new JSResult(obj);
        }
        
        public static implicit operator JSResult(string obj)
        {
            return new JSResult(obj);
        }

        public T CastData<T>()
        {
            return (T)Data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "utf-8";

            if (Data == null)
            {
                response.ContentType = "application/javascript";
                response.Write("null");
                return;
            }

            if (Data is string)
            {
                response.ContentType = "text/text";
                response.Write(Data);
                return;
            }

            if (Data is bool)
            {
                response.ContentType = "application/javascript";
                response.Write((bool)Data ? "true" : "false");
                return;
            }

            response.ContentType = "json";
            var serializedObject = JsonConvert.SerializeObject(Data, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Error });
            response.CacheControl = "no-cache";
            response.AddHeader("Pragma", "no-cache");
            response.Expires = -1;

            response.Write(serializedObject);
        }
    }
}