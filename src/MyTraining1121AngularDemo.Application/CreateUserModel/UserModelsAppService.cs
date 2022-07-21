using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyTraining1121AngularDemo.CreateUserModel.Exporting;
using MyTraining1121AngularDemo.CreateUserModel.Dtos;
using MyTraining1121AngularDemo.Dto;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CreateUserModel
{
    [AbpAuthorize(AppPermissions.Pages_UserModels)]
    public class UserModelsAppService : MyTraining1121AngularDemoAppServiceBase, IUserModelsAppService
    {
        private readonly IRepository<UserModel, long> _userModelRepository;
        private readonly IUserModelsExcelExporter _userModelsExcelExporter;

        public UserModelsAppService(IRepository<UserModel, long> userModelRepository, IUserModelsExcelExporter userModelsExcelExporter)
        {
            _userModelRepository = userModelRepository;
            _userModelsExcelExporter = userModelsExcelExporter;

        }

        public async Task<PagedResultDto<GetUserModelForViewDto>> GetAll(GetAllUserModelsInput input)
        {

            var filteredUserModels = _userModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email == input.EmailFilter);

            var pagedAndFilteredUserModels = filteredUserModels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var userModels = from o in pagedAndFilteredUserModels
                             select new
                             {

                                 o.UserId,
                                 o.FirstName,
                                 o.LastName,
                                 o.Email,
                                 Id = o.Id
                             };

            var totalCount = await filteredUserModels.CountAsync();

            var dbList = await userModels.ToListAsync();
            var results = new List<GetUserModelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetUserModelForViewDto()
                {
                    UserModel = new UserModelDto
                    {

                        UserId = o.UserId,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        Email = o.Email,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetUserModelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetUserModelForViewDto> GetUserModelForView(long id)
        {
            var userModel = await _userModelRepository.GetAsync(id);

            var output = new GetUserModelForViewDto { UserModel = ObjectMapper.Map<UserModelDto>(userModel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UserModels_Edit)]
        public async Task<GetUserModelForEditOutput> GetUserModelForEdit(EntityDto<long> input)
        {
            var userModel = await _userModelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUserModelForEditOutput { UserModel = ObjectMapper.Map<CreateOrEditUserModelDto>(userModel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUserModelDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_UserModels_Create)]
        protected virtual async Task Create(CreateOrEditUserModelDto input)
        {
            var userModel = ObjectMapper.Map<UserModel>(input);

            await _userModelRepository.InsertAsync(userModel);

        }

        [AbpAuthorize(AppPermissions.Pages_UserModels_Edit)]
        protected virtual async Task Update(CreateOrEditUserModelDto input)
        {
            var userModel = await _userModelRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, userModel);

        }

        [AbpAuthorize(AppPermissions.Pages_UserModels_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _userModelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUserModelsToExcel(GetAllUserModelsForExcelInput input)
        {

            var filteredUserModels = _userModelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
                        .WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName == input.FirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName == input.LastNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email == input.EmailFilter);

            var query = (from o in filteredUserModels
                         select new GetUserModelForViewDto()
                         {
                             UserModel = new UserModelDto
                             {
                                 UserId = o.UserId,
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 Email = o.Email,
                                 Id = o.Id
                             }
                         });

            var userModelListDtos = await query.ToListAsync();

            return _userModelsExcelExporter.ExportToFile(userModelListDtos);
        }

    }
}