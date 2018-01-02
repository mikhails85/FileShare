using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IPFS.Services.Handlers;
using IPFS.Utils.Logger;
using IPFS.Desktop.Api.ControllerExtensions; 
using IPFS.Desktop.Api.Models;

namespace IPFS.Desktop.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
       private readonly ILogger<ApiController> logger;
        
        public ApiController(ILogger<ApiController> logger)
        {
            this.logger = logger;
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> Info()
        {
            var result = await this.Handler<GetClientInformation>().Get();
            
            return this.Result(result);
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> AddNewContent(ContentManifestItem item)
        {
            var result = await this.Handler<AddContentManifestItem>().Add(item);
            
            return this.Result(result);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> LocalContent()
        {
            var result = await this.Handler<GetLocalManifestItems>().Get();
            
            return this.Result(result);
        }
    }
}
