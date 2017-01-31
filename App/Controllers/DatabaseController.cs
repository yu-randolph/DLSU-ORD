using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class DatabaseController : Controller
    {
        public string getConnString()
        {
            return ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
    }
}