using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wecheck_project.Models
{
    public class Forecast
    {
        public string weatherIcon {  get; set; }
        public string date {  get; set; }
        public string temp {  get; set; }
    }
}
