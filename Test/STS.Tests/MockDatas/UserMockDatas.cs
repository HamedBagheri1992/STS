using STS.DataAccessLayer.Entities;
using STS.DTOs.UserModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using STS.DTOs.ApplicationModels.ViewModels;

namespace STS.Tests.MockDatas
{
    public static class UserMockDatas
    {
        public static PaginatedResult<UserViewModel> UserPagedCollectionViewModels(PaginationParam pagination)
        {
            var Items = new List<UserViewModel>{
                    new UserViewModel
                        {
                            Id = 1,
                            FirstName = "F_1",
                            LastName = "L_1",
                            UserName = "U_1",
                            FullName = "Full_1",
                            IsActive = true,
                            IsDeleted = false,
                            LastLogin = null,
                            CreatedDate = DateTime.Now,
                            ModifiedDate= DateTime.Now,
                            Roles= new List<RoleViewModel>(),
                            Applications= new List<ApplicationViewModel>()
                        },
                        new UserViewModel
                        {
                            Id = 2,
                            FirstName = "F_2",
                            LastName = "L_2",
                            UserName = "U_2",
                            FullName = "Full_2",
                            IsActive = true,
                            IsDeleted = false,
                            LastLogin = null,
                            CreatedDate = DateTime.Now,
                            ModifiedDate= DateTime.Now,
                            Roles= new List<RoleViewModel>(),
                            Applications= new List<ApplicationViewModel>()
                        },
                        new UserViewModel
                        {
                            Id = 3,
                            FirstName = "F_3",
                            LastName = "L_3",
                            UserName = "U_3",
                            FullName = "Full_3",
                            IsActive = true,
                            IsDeleted = false,
                            LastLogin = null,
                            CreatedDate = DateTime.Now,
                            ModifiedDate= DateTime.Now,
                            Roles= new List<RoleViewModel>(),
                            Applications= new List<ApplicationViewModel>()
                        }
                };

            return new PaginatedResult<UserViewModel>(Items, 3, pagination.PageNumber, pagination.PageSize);
        }

        public static List<UserViewModel> UserCollectionViewModels()
        {
            return new List<UserViewModel>
            {
                new UserViewModel
                {
                     Id = 1,
                     FirstName = "F_1",
                     LastName = "L_1",
                     UserName = "U_1",
                     FullName = "F_1 L_1",
                     IsActive = true,
                     IsDeleted = false,
                     LastLogin = null,
                     CreatedDate = DateTime.Now,
                     ModifiedDate= DateTime.Now,
                     Roles= new List<RoleViewModel>(),
                     Applications= new List<ApplicationViewModel>()
                },
                new UserViewModel
                {
                     Id = 2,
                     FirstName = "F_2",
                     LastName = "L_2",
                     UserName = "U_2",
                     FullName = "F_2 L_2",
                     IsActive = true,
                     IsDeleted = false,
                     LastLogin = null,
                     CreatedDate = DateTime.Now,
                     ModifiedDate= DateTime.Now,
                     Roles= new List<RoleViewModel>(),
                     Applications= new List<ApplicationViewModel>()
                },
                new UserViewModel
                {
                     Id = 3,
                     FirstName = "F_3",
                     LastName = "L_3",
                     UserName = "U_3",
                      FullName = "F_3 L_3",
                     IsActive = true,
                     IsDeleted = false,
                     LastLogin = null,
                     CreatedDate = DateTime.Now,
                     ModifiedDate= DateTime.Now,
                     Roles= new List<RoleViewModel>(),
                     Applications= new List<ApplicationViewModel>()
                }
            };
        }

        public static List<User> UserCollectionEntityModels()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "F_1",
                    LastName = "L_1",
                    UserName = "U_1",
                    IsActive = true,
                    IsDeleted = false,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = string.Empty
                },
                new User
                {
                    Id = 2,
                    FirstName = "F_2",
                    LastName = "L_2",
                    UserName = "U_2",
                    IsActive = true,
                    IsDeleted = false,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = string.Empty
                },
                new User
                {
                    Id = 3,
                    FirstName = "F_3",
                    LastName = "L_3",
                    UserName = "U_3",
                    IsActive = true,
                    IsDeleted = false,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = string.Empty
                }
            };
        }

        public static UserViewModel UserSingleViewModel()
        {
            return new UserViewModel
            {
                Id = 1,
                FirstName = "F_1",
                LastName = "L_1",
                UserName = "U_1",
                FullName = "F_1 L_1",
                IsActive = true,
                IsDeleted = false,
                LastLogin = null,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Roles = new List<RoleViewModel>(),
                Applications = new List<ApplicationViewModel>()
            };
        }
    }
}
