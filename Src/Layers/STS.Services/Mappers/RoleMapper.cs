﻿using STS.DataAccessLayer.Entities;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Services.Mappers
{
    public static class RoleMapper
    {
        public static IQueryable<RoleViewModel> ToViewModel(this IQueryable<Role> query)
        {
            return query.Select(item => new RoleViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                Permissions = item.Permissions.ToViewModel()
            });
        }

        public static RoleViewModel ToViewModel(this Role item)
        {
            return new RoleViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                Permissions = item.Permissions.ToViewModel()
            };
        }
    }
}