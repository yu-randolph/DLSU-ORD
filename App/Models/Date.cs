using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Date
    {
        private int month { get; set; }
        private int day { get; set; }
        private int year { get; set; }

        public string toString()
        {
            return year + ", " + month + ", " + day;
        }
    }
}