namespace fMailer.Web.Core
{
    using System.Web.Mvc;

    public class JsonValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new JsonParamsValueProvider(controllerContext);
        }
    }
}