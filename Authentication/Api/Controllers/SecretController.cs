using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SecretController : Controller
    {
        // GET: SecretController
        [Authorize]
        public string Index()
        {
            return "Secret Message";
        }

      
    }
}
