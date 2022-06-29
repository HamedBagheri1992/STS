using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.CategoryModels.FormModels
{
    public class UpdateCategoryFormModel : AddCategoryFormModel
    {
        public long Id { get; set; }
    }
}
