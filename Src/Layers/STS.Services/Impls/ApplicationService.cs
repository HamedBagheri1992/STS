﻿using Microsoft.EntityFrameworkCore;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.ApplicationModels.FormModels;
using STS.DTOs.ApplicationModels.ViewModels;
using STS.DTOs.ResultModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;

namespace STS.Services.Impls
{
    public class ApplicationService : IApplicationService
    {
        private readonly STSDbContext _context;

        public ApplicationService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<ApplicationViewModel>> GetAsync(PaginationParam pagination)
        {
            try
            {
                var applications = _context.Applications.OrderBy(a => a.Id).ToViewModel();
                return await applications.ToPagedListAsync<ApplicationViewModel>(pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : GetError", ex);
            }
        }

        public async Task<ApplicationViewModel?> GetAsync(long id)
        {
            try
            {
                var application = await _context.Applications.Include(a => a.Permissions).Include(a => a.Roles).FirstOrDefaultAsync(a => a.Id == id);
                return application?.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : Get(id)Error", ex);
            }
        }

        public async Task<long> AddAsync(AddApplicationFormModel addFormModel)
        {
            try
            {
                var application = new Application
                {
                    Title = addFormModel.Title,
                    SecretKey = Guid.NewGuid(),
                    Description = addFormModel.Description,
                    CreatedDate = DateTime.Now
                };

                await _context.Applications.AddAsync(application);
                await _context.SaveChangesAsync();

                return application.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : AddError", ex);
            }
        }

        public async Task UpdateAsync(UpdateApplicationFormModel updateFormModel)
        {
            try
            {
                var application = await _context.Applications.FindAsync(updateFormModel.Id);
                if (application is null)
                    throw new Exception("ApplicationService : Application is Invalid");

                if (application.Title != updateFormModel.Title)
                    application.Title = updateFormModel.Title;

                if (application.Description != updateFormModel.Description)
                    application.Description = updateFormModel.Description;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : UpdateError", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var application = await _context.Applications.FindAsync(id);
                if (application is null)
                    throw new Exception("ApplicationService : Application is Invalid");

                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : DeleteError", ex);
            }
        }

        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Applications.AnyAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : IsExistError", ex);
            }
        }

        public Task<bool> IsExistAsync(long id, Guid secretKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsTitleDuplicateAsync(string title)
        {
            try
            {
                return await _context.Applications.AnyAsync(a => a.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : IsTitleDuplicate(title)Error", ex);
            }
        }

        public async Task<bool> IsTitleDuplicateAsync(long id, string title)
        {
            try
            {
                return await _context.Applications.AnyAsync(a => a.Id != id && a.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("ApplicationService : IsTitleDuplicate(id,title)Error", ex);
            }
        }
    }
}
