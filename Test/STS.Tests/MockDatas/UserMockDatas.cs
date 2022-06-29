using STS.DataAccessLayer.Entities;
using STS.DTOs.UserModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.DTOs.RoleModels.ViewModels;
using System;
using System.Collections.Generic;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.UserModels.FormModels;
using STS.Common.Extensions;

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
                            UserName = "U1",
                            FullName = "Full_1",
                            IsActive = true,
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
                     UserName = "U1",
                     FullName = "F_1 L_1",
                     IsActive = true,
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
                    UserName = "U1",
                    IsActive = true,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = "123".ToHashPassword()
                },
                new User
                {
                    Id = 2,
                    FirstName = "F_2",
                    LastName = "L_2",
                    UserName = "U_2",
                    IsActive = true,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = "123".ToHashPassword()
                },
                new User
                {
                    Id = 3,
                    FirstName = "F_3",
                    LastName = "L_3",
                    UserName = "U_3",
                    IsActive = true,
                    LastLogin = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate= DateTime.Now,
                    Password = "123".ToHashPassword()
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
                UserName = "U1",
                FullName = "F_1 L_1",
                IsActive = true,
                LastLogin = null,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Roles = new List<RoleViewModel>(),
                Applications = new List<ApplicationViewModel>(),
                OrganizationIds = new List<long> { 1, 2, 3, 4 }
            };
        }

        public static UserViewModel UserSingleViewModel(AddUserFormModel formModel)
        {
            return new UserViewModel
            {
                Id = 1,
                FirstName = formModel.FirstName,
                LastName = formModel.LastName,
                UserName = formModel.UserName,
                FullName = $"{formModel.FirstName} {formModel.LastName}",
                IsActive = formModel.IsActive,
                LastLogin = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Roles = new List<RoleViewModel>(),
                Applications = new List<ApplicationViewModel>()
            };
        }

        public static AddUserFormModel AddFormModel()
        {
            return new AddUserFormModel { FirstName = "F_1", LastName = "L_1", UserName = "U1", Password = "123".ToHashPassword(), IsActive = false };
        }

        public static UpdateUserFormModel UpdateFormModel()
        {
            return new UpdateUserFormModel { Id = 1, FirstName = "F_1", LastName = "L_1", UserName = "U1", Password = "123".ToHashPassword(), IsActive = false };
        }

        public static ChangePasswordFormModel changePasswordFormModel(string oldpass = "123", string newpass = "321")
        {
            return new ChangePasswordFormModel { Id = 1, oldPassword = oldpass, NewPassword = newpass };
        }

        public static User EntityModel()
        {
            return new User
            {
                Id = 1,
                FirstName = "F_1",
                LastName = "L_1",
                UserName = "U1",
                Password = "123".ToHashPassword(),
                IsActive = false,
                LastLogin = DateTime.Now,
                ModifiedDate = DateTime.Now,
                CreatedDate = DateTime.Now
            };
        }

    }
}
