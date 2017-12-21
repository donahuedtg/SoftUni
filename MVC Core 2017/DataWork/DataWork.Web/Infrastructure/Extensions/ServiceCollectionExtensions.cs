namespace DataWork.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using System.Reflection;
    using System.Linq;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices
            (this IServiceCollection service)
        {
            Assembly.GetAssembly(typeof(IService))
                .GetTypes()
                .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                .Select(t => new
                {
                    Interface = t.GetInterface($"I{t.Name}"),
                    Implemantation = t
                })
                .ToList()
                .ForEach(s => service.AddTransient(s.Interface, s.Implemantation));


            return service;
        }
    }
}
