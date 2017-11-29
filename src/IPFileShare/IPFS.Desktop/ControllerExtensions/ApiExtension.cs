using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using IPFS.Results;
using Microsoft.AspNetCore.Builder;

namespace IPFS.Desktop.ControllerExtensions
{
    public static class ApiExtension
    {
        private static IServiceProvider provider;
        
        public static void UseApiExtension (this IApplicationBuilder app)
        {
            provider = app.ApplicationServices;    
        }
        
        public static T Handler<T> (this Controller controller)
        {
            return provider.GetService<T>();
        }
        
        public static IActionResult Result(this Controller controller, VoidResult result)
        {
            var response = new ObjectResult(result);
            response.StatusCode = result.Success ? 200 : 500;
            return response;
        }
    }
}