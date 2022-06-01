using Microsoft.EntityFrameworkCore;
using STS.Common.BaseModels;
using STS.Common.Extensions;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ResultModels;
using STS.DTOs.UserModels.FormModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;

namespace STS.Services.Impls
{
    public class UserService : IUserService
    {
        private readonly STSDbContext _context;

        public UserService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<UserViewModel>> GetAsync(PaginationParam pagination)
        {
            try
            {
                var users = _context.Users.Include(a => a.Applications).Include(a => a.Roles).OrderBy(a => a.Id).ToViewModel();
                return await users.ToPagedListAsync<UserViewModel>(pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : Get(Pagination)Error", ex);
            }
        }

        public async Task<UserViewModel?> GetAsync(long id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Applications).Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id);
                return user?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : Get(Id)Error", ex);
            }
        }

        public async Task<long> AddAsync(AddUserFormModel addFormModel)
        {
            try
            {
                var user = new User
                {
                    FirstName = addFormModel.FirstName,
                    LastName = addFormModel.LastName,
                    UserName = addFormModel.UserName,
                    Password = addFormModel.Password.ToHashPassword(),
                    IsActive = addFormModel.IsActive,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : AddError", ex);
            }
        }

        public async Task UpdateAsync(UpdateUserFormModel updateFormModel)
        {
            try
            {
                var user = await _context.Users.FindAsync(updateFormModel.Id);
                if (user is null)
                    throw new Exception("UserService : User is Invalid");

                if (user.FirstName != updateFormModel.FirstName)
                    user.FirstName = updateFormModel.FirstName;

                if (user.LastName != updateFormModel.LastName)
                    user.LastName = updateFormModel.LastName;

                if (user.UserName != updateFormModel.UserName)
                    user.UserName = updateFormModel.UserName;

                if (user.IsActive != updateFormModel.IsActive)
                    user.IsActive = updateFormModel.IsActive;

                user.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : UpdateError", ex);
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordFormModel changePasswordFormModel)
        {
            try
            {
                var user = await _context.Users.FindAsync(changePasswordFormModel.Id);
                if (user is null)
                    throw new Exception("UserService : User is Invalid");

                user.Password = changePasswordFormModel.NewPassword.ToHashPassword();

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : ChangePasswordError", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user is null)
                    throw new Exception("UserService : User is Invalid");

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : ChangePasswordError", ex);
            }
        }

        public async Task<bool> IsUserNameDuplicateAsync(string userName)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : IsUserNameDuplicate(userName)Error", ex);
            }
        }

        public async Task<bool> IsUserNameDuplicateAsync(long id, string userName)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.UserName == userName && u.Id != id);
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : IsUserNameDuplicate(id,userName)Error", ex);
            }
        }
        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : IsExistError", ex);
            }
        }

        public async Task<bool> IsPasswordValidAsync(long id, string oldPassword)
        {
            try
            {
                string pass = oldPassword.ToHashPassword();
                return await _context.Users.AnyAsync(u => u.Id == id && u.Password == pass);
            }
            catch (Exception ex)
            {
                throw new Exception("UserService : IsPasswordValidError", ex);
            }
        }
    }
}
