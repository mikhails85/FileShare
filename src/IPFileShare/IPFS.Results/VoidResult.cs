using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPFS.Results
{
    public class VoidResult
    {
        public Error[] Errors => this.errors.ToArray();
        
        public virtual bool Success => !Errors.Any();
     
        private readonly List<Error> errors;   
        public VoidResult()
        {
            this.errors = new List<Error>();
        }
        
        public void AddErrors(params Error[] errors)
        {
            this.errors.AddRange(errors);
        }
    }
}