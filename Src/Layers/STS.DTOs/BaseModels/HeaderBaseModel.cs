using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.BaseModels
{
    public class HeaderBaseModel
    {
        public long AppId { get; set; }
        public Guid SecretKey { get; set; }
    }
}
