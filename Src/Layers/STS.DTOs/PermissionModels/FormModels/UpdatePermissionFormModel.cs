﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.DTOs.PermissionModels.FormModels
{
    public class UpdatePermissionFormModel : AddPermissionFormModel
    {
        [Required]
        public long Id { get; set; }
    }
}
