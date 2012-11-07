namespace fMailer.Web.Core
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonParamsValueProvider : IValueProvider
    {
        private readonly ControllerContext controllerContext;

        private object temp;

        public JsonParamsValueProvider(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }

        public bool ContainsPrefix(string prefix)
        {
            try
            {
                var actionMethodName = this.controllerContext.RouteData.Values["action"].ToString();
                var actionMethodInfo = this.controllerContext.Controller.GetType().GetMethod(actionMethodName);
                if (actionMethodInfo == null)
                {
                    return false;
                }

                var parameterInfo = actionMethodInfo.GetParameters().FirstOrDefault(x => x.Name == prefix);
                if (parameterInfo == null)
                {
                    return false;
                }

                object deserializedObject = null;
                var parameterType = parameterInfo.ParameterType;
                if (this.controllerContext.RequestContext.HttpContext.Request.Params[0] == null)
                {
                    return false;
                }

                var value = JObject.Parse(this.controllerContext.RequestContext.HttpContext.Request.Params[0])[prefix].ToString();
                
                if (parameterType.FullName == string.Empty.GetType().FullName)
                {
                    deserializedObject = value;
                }
                else
                {
                    if (parameterType.IsPrimitive)
                    {
                        var parseMethodInfo = parameterType.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == "Parse");
                        if (parseMethodInfo != null)
                        {
                            deserializedObject = parseMethodInfo.Invoke(null, new[] { value });
                        }
                    }
                    else
                    {
                        deserializedObject = JsonConvert.DeserializeObject(value, parameterType);
                    }
                }

                if (deserializedObject == null)
                {
                    return false;
                }

                temp = deserializedObject;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ValueProviderResult GetValue(string key)
        {
            var result = new ValueProviderResult(temp, key, default(CultureInfo));
            temp = null;
            return result;
        }
    }
}