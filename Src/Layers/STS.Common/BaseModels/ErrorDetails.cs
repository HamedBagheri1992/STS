using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Common.BaseModels
{
    public class ErrorDetails
    {
        public ErrorDetails()
        {
            Error = new List<string>();
        }

        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Message { get; set; }
        public List<string> Error { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
