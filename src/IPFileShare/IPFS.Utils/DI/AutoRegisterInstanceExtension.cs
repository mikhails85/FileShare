using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace IPFS.Utils.DI
{
    public static class AutoRegisterInstanceExtension
    {
        public static IServiceCollection AutoRegisterInstanceOf<T>(this IServiceCollection serviceCollection)
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.TryGetTypes())
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
                .SelectMany(s => s.TryGetTypes())
                .Where(p => p.IsDefined(typeof(InjectableAttribute),true) && !p.IsAbstract);
            foreach(var item in types)    
            {
                serviceCollection.AddTransient(item);
            }
            
            return serviceCollection;
        }
        
        public static List<Type> TryGetTypes(this Assembly assembly)
        {
            var types = new List<Type>();
            try
            {
                types.AddRange(assembly.GetTypes());
            }
            catch
            {
                
            }
            return types;
        }
    }
}