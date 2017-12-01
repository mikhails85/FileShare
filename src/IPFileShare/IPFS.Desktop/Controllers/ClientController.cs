using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IPFS.Services.Handlers;
using IPFS.Utils.Logger;
using IPFS.Desktop.ControllerExtensions; 

namespace IPFS.Desktop.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> logger;
        
        public ClientController(ILogger<ClientController> logger)
        {
            this.logger = logger;
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> Info()
        {
            var result = await this.Handler<GetClientInformation>().Get();
            
            return this.Result(result);
        }
    }
}