using System.Web.Mvc;
using ClassicMvc.Infrastructure.Loggers;
using ClassicMvc.Services;
using ThirdPartyEventEditor.Models;
using Unity;
using Unity.Mvc5;

namespace ClassicMvc.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IThirdPartyEventRepository, ThirdPartyEventRepository>();
            container.RegisterType<IJsonSerializerService<ThirdPartyEvent>, JsonSerializerService<ThirdPartyEvent>>();
            container.RegisterType<ILogger, FileLogger>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}