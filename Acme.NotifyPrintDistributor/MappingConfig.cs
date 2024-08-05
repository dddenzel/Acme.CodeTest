using Acme.NotifyPrintDistributor.Handlers.FirstPrintDistributor;
using Acme.NotifyPrintDistributor.Handlers.SecondPrintDistributor;
using AutoMapper;

namespace Acme.NotifyPrintDistributor
{
    /// <summary>
    /// Mapping configuration
    /// </summary>
    public static class MappingConfig
    {
        /// <summary>
        /// Get mapping configuration
        /// </summary>
        /// <returns>IMapper</returns>
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => 
            { 
                cfg.AddProfile(new FirstPrintDistributorMappingProfile());
                cfg.AddProfile(new SecondPrintDistributorMappingProfile());
            });
            

            return config.CreateMapper();
        }
    }
}
