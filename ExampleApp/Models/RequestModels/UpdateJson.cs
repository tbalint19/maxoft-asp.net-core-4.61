using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleApp.Models.RequestModels
{
    public class UpdateJson
    {
        public string username { get; set; }
        public string role { get; set; }
        public bool setTo { get; set; }
    }
}
