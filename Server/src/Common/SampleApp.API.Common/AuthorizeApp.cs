using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.API.Common
{
   
    public class AuthorizeApp : AuthorizeAttribute
    {
       // public string Roles { get; set; }

            
        public AuthorizeApp(): base()
        {
            //this.Roles =  "LabUser,LabAdmin,SysAdmin";
        }        
    }

    public class AuthorizeLabAdmin : AuthorizeApp
    {
        public AuthorizeLabAdmin(): base()
        {
            this.Roles = "LabAdmin,SysAdmin";
        }
    }

    public class AuthorizeSystemAdmin : AuthorizeApp
    {
        public AuthorizeSystemAdmin() : base()
        {
            this.Roles = "SysAdmin";
        }
    }

}
