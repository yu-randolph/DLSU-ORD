using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace App.Models
{
    public class DatabaseConnector
    {
        public string getConnString()
        {
            return ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
    }
}