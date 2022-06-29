using Microsoft.EntityFrameworkCore;
using STS.Common.BaseModels;
using STS.DataAccessLayer;
using STS.DataAccessLayer.Entities;
using STS.DTOs.OrganizationModel.FormModels;
using STS.DTOs.OrganizationModel.ViewModels;
using STS.DTOs.UserModels.ViewModels;
using STS.Interfaces.Contracts;
using STS.Services.Mappers;

namespace STS.Services.Impls
{
    public class OrganizationService : IOrganizationService
    {
        private readonly STSDbContext _context;

        public OrganizationService(STSDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrganizationViewModel>> GetAsync()
        {
            try
            {
                return await _context.Organizations.Include(o => o.Parent).ToViewModel().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : GetError", ex);
            }
        }

        public async Task<List<OrganizationViewModel>> GetAsync(long id)
        {
            try
            {
                var orgs = await _context.Organizations.Include(o => o.Parent).ToViewModel().ToListAsync();
                return orgs.ToManageOrganizations(id);
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : GetError", ex);
            }
        }

        public async Task<List<OrganizationViewModel>> GetAsync(UserViewModel user)
        {
            try
            {
                var orgs = await _context.Organizations.Include(o => o.Parent).ToViewModel().ToListAsync();

                var listOrgs = new List<OrganizationViewModel>();
                user.OrganizationIds.ToList().ForEach(item =>
                {
                    listOrgs.AddRange(orgs.ToManageOrganizations(item));
                });

                listOrgs = listOrgs.GroupBy(o => o.Id).Select(s => s.First()).OrderBy(x => x.Id).ToList();
                return listOrgs;
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : GetError", ex);
            }
        }

        public async Task<OrganizationViewModel?> GetSingleOrganizationAsync(long id)
        {
            try
            {
                var organization = await _context.Organizations.FindAsync(id);
                if (organization is null)
                    return null;

                return organization.ToViewModel();
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : GetSingleOrganizationError", ex);
            }
        }

        public async Task<long> AddAsync(AddOrganizationFormModel addFormModel)
        {
            try
            {
                var organization = new Organization()
                {
                    Title = addFormModel.Title,
                    ParentId = addFormModel.ParentId,
                    Tag = addFormModel.Tag,
                    Description = addFormModel.Description
                };

                await _context.Organizations.AddAsync(organization);
                await _context.SaveChangesAsync();
                return organization.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : AddError", ex);
            }
        }

        public async Task UpdateAsync(UpdateOrganizationFormModel updateFormModel)
        {
            try
            {
                var organization = await _context.Organizations.FindAsync(updateFormModel.Id);
                if (organization is null)
                    throw new STSException("Organization is Invalid");

                if (organization.Title != updateFormModel.Title)
                    organization.Title = updateFormModel.Title;

                if (organization.ParentId != updateFormModel.ParentId)
                    organization.ParentId = updateFormModel.ParentId;

                if (organization.Tag != updateFormModel.Tag)
                    organization.Tag = updateFormModel.Tag;

                if (organization.Description != updateFormModel.Description)
                    organization.Description = updateFormModel.Description;

                await _context.SaveChangesAsync();
            }
            catch (STSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : UpdateError", ex);
            }
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExistAsync(long id)
        {
            try
            {
                return await _context.Organizations.AnyAsync(o => o.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("OrganizationService : IsExistError", ex);
            }
        }
    }
}
