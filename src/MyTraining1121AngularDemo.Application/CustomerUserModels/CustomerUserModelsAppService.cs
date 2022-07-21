using MyTraining1121AngularDemo.CustomerModel;
using MyTraining1121AngularDemo.CreateUserModel;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyTraining1121AngularDemo.CustomerUserModels.Exporting;
using MyTraining1121AngularDemo.CustomerUserModels.Dtos;
using MyTraining1121AngularDemo.Dto;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerUserModels
{
    [AbpAuthorize(AppPermissions.Pages_CustomerUserModels)]
    public class CustomerUserModelsAppService : MyTraining1121AngularDemoAppServiceBase, ICustomerUserModelsAppService
    {
        private readonly IRepository<CustomerUserModel, long> _customerUserModelRepository;
        private readonly ICustomerUserModelsExcelExporter _customerUserModelsExcelExporter;
        private readonly IRepository<Customer, long> _lookup_customerRepository;
        private readonly IRepository<UserModel, long> _lookup_userModelRepository;

        public CustomerUserModelsAppService(IRepository<CustomerUserModel, long> customerUserModelRepository, ICustomerUserModelsExcelExporter customerUserModelsExcelExporter, IRepository<Customer, long> lookup_customerRepository, IRepository<UserModel, long> lookup_userModelRepository)
        {
            _customerUserModelRepository = customerUserModelRepository;
            _customerUserModelsExcelExporter = customerUserModelsExcelExporter;
            _lookup_customerRepository = lookup_customerRepository;
            _lookup_userModelRepository = lookup_userModelRepository;

        }

        public async Task<PagedResultDto<GetCustomerUserModelForViewDto>> GetAll(GetAllCustomerUserModelsInput input)
        {

            var filteredCustomerUserModels = _customerUserModelRepository.GetAll()
                        .Include(e => e.CustomerFk)
                        .Include(e => e.UserModelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinCustomerUserIdFilter != null, e => e.CustomerUserId >= input.MinCustomerUserIdFilter)
                        .WhereIf(input.MaxCustomerUserIdFilter != null, e => e.CustomerUserId <= input.MaxCustomerUserIdFilter)
                        .WhereIf(input.MinTotalBillingAmountFilter != null, e => e.TotalBillingAmount >= input.MinTotalBillingAmountFilter)
                        .WhereIf(input.MaxTotalBillingAmountFilter != null, e => e.TotalBillingAmount <= input.MaxTotalBillingAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerCustomerNameFilter), e => e.CustomerFk != null && e.CustomerFk.CustomerName == input.CustomerCustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserModelFirstNameFilter), e => e.UserModelFk != null && e.UserModelFk.FirstName == input.UserModelFirstNameFilter);

            var pagedAndFilteredCustomerUserModels = filteredCustomerUserModels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerUserModels = from o in pagedAndFilteredCustomerUserModels
                                     join o1 in _lookup_customerRepository.GetAll() on o.CustomerId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_userModelRepository.GetAll() on o.UserModelId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     select new
                                     {

                                         o.CustomerUserId,
                                         o.TotalBillingAmount,
                                         Id = o.Id,
                                         CustomerCustomerName = s1 == null || s1.CustomerName == null ? "" : s1.CustomerName.ToString(),
                                         UserModelFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
                                     };

            var totalCount = await filteredCustomerUserModels.CountAsync();

            var dbList = await customerUserModels.ToListAsync();
            var results = new List<GetCustomerUserModelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerUserModelForViewDto()
                {
                    CustomerUserModel = new CustomerUserModelDto
                    {

                        CustomerUserId = o.CustomerUserId,
                        TotalBillingAmount = o.TotalBillingAmount,
                        Id = o.Id,
                    },
                    CustomerCustomerName = o.CustomerCustomerName,
                    UserModelFirstName = o.UserModelFirstName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerUserModelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomerUserModelForViewDto> GetCustomerUserModelForView(long id)
        {
            var customerUserModel = await _customerUserModelRepository.GetAsync(id);

            var output = new GetCustomerUserModelForViewDto { CustomerUserModel = ObjectMapper.Map<CustomerUserModelDto>(customerUserModel) };

            if (output.CustomerUserModel.CustomerId != null)
            {
                var _lookupCustomer = await _lookup_customerRepository.FirstOrDefaultAsync((long)output.CustomerUserModel.CustomerId);
                output.CustomerCustomerName = _lookupCustomer?.CustomerName?.ToString();
            }

            if (output.CustomerUserModel.UserModelId != null)
            {
                var _lookupUserModel = await _lookup_userModelRepository.FirstOrDefaultAsync((long)output.CustomerUserModel.UserModelId);
                output.UserModelFirstName = _lookupUserModel?.FirstName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels_Edit)]
        public async Task<GetCustomerUserModelForEditOutput> GetCustomerUserModelForEdit(EntityDto<long> input)
        {
            var customerUserModel = await _customerUserModelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerUserModelForEditOutput { CustomerUserModel = ObjectMapper.Map<CreateOrEditCustomerUserModelDto>(customerUserModel) };

            if (output.CustomerUserModel.CustomerId != null)
            {
                var _lookupCustomer = await _lookup_customerRepository.FirstOrDefaultAsync((long)output.CustomerUserModel.CustomerId);
                output.CustomerCustomerName = _lookupCustomer?.CustomerName?.ToString();
            }

            if (output.CustomerUserModel.UserModelId != null)
            {
                var _lookupUserModel = await _lookup_userModelRepository.FirstOrDefaultAsync((long)output.CustomerUserModel.UserModelId);
                output.UserModelFirstName = _lookupUserModel?.FirstName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerUserModelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels_Create)]
        protected virtual async Task Create(CreateOrEditCustomerUserModelDto input)
        {
            var customerUserModel = ObjectMapper.Map<CustomerUserModel>(input);

            await _customerUserModelRepository.InsertAsync(customerUserModel);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerUserModelDto input)
        {
            var customerUserModel = await _customerUserModelRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerUserModel);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerUserModelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomerUserModelsToExcel(GetAllCustomerUserModelsForExcelInput input)
        {

            var filteredCustomerUserModels = _customerUserModelRepository.GetAll()
                        .Include(e => e.CustomerFk)
                        .Include(e => e.UserModelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinCustomerUserIdFilter != null, e => e.CustomerUserId >= input.MinCustomerUserIdFilter)
                        .WhereIf(input.MaxCustomerUserIdFilter != null, e => e.CustomerUserId <= input.MaxCustomerUserIdFilter)
                        .WhereIf(input.MinTotalBillingAmountFilter != null, e => e.TotalBillingAmount >= input.MinTotalBillingAmountFilter)
                        .WhereIf(input.MaxTotalBillingAmountFilter != null, e => e.TotalBillingAmount <= input.MaxTotalBillingAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerCustomerNameFilter), e => e.CustomerFk != null && e.CustomerFk.CustomerName == input.CustomerCustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserModelFirstNameFilter), e => e.UserModelFk != null && e.UserModelFk.FirstName == input.UserModelFirstNameFilter);

            var query = (from o in filteredCustomerUserModels
                         join o1 in _lookup_customerRepository.GetAll() on o.CustomerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userModelRepository.GetAll() on o.UserModelId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCustomerUserModelForViewDto()
                         {
                             CustomerUserModel = new CustomerUserModelDto
                             {
                                 CustomerUserId = o.CustomerUserId,
                                 TotalBillingAmount = o.TotalBillingAmount,
                                 Id = o.Id
                             },
                             CustomerCustomerName = s1 == null || s1.CustomerName == null ? "" : s1.CustomerName.ToString(),
                             UserModelFirstName = s2 == null || s2.FirstName == null ? "" : s2.FirstName.ToString()
                         });

            var customerUserModelListDtos = await query.ToListAsync();

            return _customerUserModelsExcelExporter.ExportToFile(customerUserModelListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels)]
        public async Task<List<CustomerUserModelCustomerLookupTableDto>> GetAllCustomerForTableDropdown()
        {
            return await _lookup_customerRepository.GetAll()
                .Select(customer => new CustomerUserModelCustomerLookupTableDto
                {
                    Id = customer.Id,
                    DisplayName = customer == null || customer.CustomerName == null ? "" : customer.CustomerName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUserModels)]
        public async Task<List<CustomerUserModelUserModelLookupTableDto>> GetAllUserModelForTableDropdown()
        {
            return await _lookup_userModelRepository.GetAll()
                .Select(userModel => new CustomerUserModelUserModelLookupTableDto
                {
                    Id = userModel.Id,
                    DisplayName = userModel == null || userModel.FirstName == null ? "" : userModel.FirstName.ToString()
                }).ToListAsync();
        }

    }
}