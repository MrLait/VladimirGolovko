using System.Web.Mvc;
using ClassicMvc.Models;
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
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}