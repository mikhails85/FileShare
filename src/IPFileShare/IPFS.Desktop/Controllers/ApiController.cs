using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IPFS.Results;
namespace IPFS.Desktop.Controllers
{
    public abstract class ApiController : Controller
    {
        protected IActionResult Result<T>(Result<T> result) 
        {
            
        }
        
        protected IActionResult Result(VoidResult result)
        {
            
        }
        
        protected T Instance<T>() 
        {
            
        }
    }
}