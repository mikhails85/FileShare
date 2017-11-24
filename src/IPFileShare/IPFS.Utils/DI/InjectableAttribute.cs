using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Utils.DI
{
    [System.AttributeUsage(System.AttributeTargets.Class)]  
    public class InjectableAttribute: System.Attribute  
    {
    }
}