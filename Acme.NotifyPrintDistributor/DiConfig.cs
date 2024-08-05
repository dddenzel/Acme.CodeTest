using Acme.CodeTest.Api.Api;
using Acme.NotifyPrintDistributor.Interfaces;
using AutoMapper;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Acme.NotifyPrintDistributor
{
    public static class DiConfig
    {
        /// <summary>
        /// Performs the configuration.
        /// </summary>
        /// <returns>A configured SimpleInjector Container</returns>
        public static Container Configure()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.CreateHybrid(Lifestyle.Scoped, Lifestyle.Singleton);
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register singleton services
            container.RegisterSingleton<IMapper>(() => MappingConfig.GetMapper());

            // Register scoped services
            container.Register<ISubscriptionApi, SubscriptionApi>();
            
            // Register handlers
            typeof(DiConfig).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(ISubscriptionHandler))).ToList()
                .ForEach(x => container.Register(x));

            return container;
        }
    }
}
