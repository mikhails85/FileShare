using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace IPFS.Utils.DI
{
    public static class AutoRegisterInstanceExtension
    {
        public static IServiceCollection AutoRegisterInstanceOf<T>(this IServiceCollection serviceCollection)
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface);
            foreach(var item in types)    
            {
                serviceCollection.AddTransient(item);
            }
            
            return serviceCollection;
        }
        
        public static IServiceCollection AutoRegisterInjectable(this IServiceCollection serviceCollection)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsDefined(typeof(InjectableAttribute),true) && !p.IsAbstract);
            foreach(var item in types)    
            {
                serviceCollection.AddTransient(item);
            }
            
            return serviceCollection;
        }
    }
}