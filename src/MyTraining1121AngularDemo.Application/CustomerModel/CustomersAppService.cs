using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyTraining1121AngularDemo.CustomerModel.Exporting;
using MyTraining1121AngularDemo.CustomerModel.Dtos;
using MyTraining1121AngularDemo.Dto;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerModel
{
    [AbpAuthorize(AppPermissions.Pages_Customers)]
    public class CustomersAppService : MyTraining1121AngularDemoAppServiceBase, ICustomersAppService
    {
        private readonly IRepository<Customer, long> _customerRepository;
        private readonly ICustomersExcelExporter _customersExcelExporter;

        public CustomersAppService(IRepository<Customer, long> customerRepository, ICustomersExcelExporter customersExcelExporter)
        {
            _customerRepository = customerRepository;
            _customersExcelExporter = customersExcelExporter;

        }

        public async Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomersInput input)
        {

            var filteredCustomers = _customerRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerName.Contains(input.Filter) || e.CustomerEmail.Contains(input.Filter) || e.CustomerAddress.Contains(input.Filter))
                        .WhereIf(input.MinCustomerIdFilter != null, e => e.CustomerId >= input.MinCustomerIdFilter)
                        .WhereIf(input.MaxCustomerIdFilter != null, e => e.CustomerId <= input.MaxCustomerIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNameFilter), e => e.CustomerName == input.CustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerEmailFilter), e => e.CustomerEmail == input.CustomerEmailFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerAddressFilter), e => e.CustomerAddress == input.CustomerAddressFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter);

            var pagedAndFilteredCustomers = filteredCustomers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customers = from o in pagedAndFilteredCustomers
                            select new
                            {

                                o.CustomerId,
                                o.CustomerName,
                                o.CustomerEmail,
                                o.CustomerAddress,
                                o.RegistrationDate,
                                Id = o.Id
                            };

            var totalCount = await filteredCustomers.CountAsync();

            var dbList = await customers.ToListAsync();
            var results = new List<GetCustomerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerForViewDto()
                {
                    Customer = new CustomerDto
                    {

                        CustomerId = o.CustomerId,
                        CustomerName = o.CustomerName,
                        CustomerEmail = o.CustomerEmail,
                        CustomerAddress = o.CustomerAddress,
                        RegistrationDate = o.RegistrationDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomerForViewDto> GetCustomerForView(long id)
        {
            var customer = await _customerRepository.GetAsync(id);

            var output = new GetCustomerForViewDto { Customer = ObjectMapper.Map<CustomerDto>(customer) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Edit)]
        public async Task<GetCustomerForEditOutput> GetCustomerForEdit(EntityDto<long> input)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerForEditOutput { Customer = ObjectMapper.Map<CreateOrEditCustomerDto>(customer) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Customers_Create)]
        protected virtual async Task Create(CreateOrEditCustomerDto input)
        {
            var customer = ObjectMapper.Map<Customer>(input);

            await _customerRepository.InsertAsync(customer);

        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerDto input)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customer);

        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomersToExcel(GetAllCustomersForExcelInput input)
        {

            var filteredCustomers = _customerRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerName.Contains(input.Filter) || e.CustomerEmail.Contains(input.Filter) || e.CustomerAddress.Contains(input.Filter))
                        .WhereIf(input.MinCustomerIdFilter != null, e => e.CustomerId >= input.MinCustomerIdFilter)
                        .WhereIf(input.MaxCustomerIdFilter != null, e => e.CustomerId <= input.MaxCustomerIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNameFilter), e => e.CustomerName == input.CustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerEmailFilter), e => e.CustomerEmail == input.CustomerEmailFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerAddressFilter), e => e.CustomerAddress == input.CustomerAddressFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter);

            var query = (from o in filteredCustomers
                         select new GetCustomerForViewDto()
                         {
                             Customer = new CustomerDto
                             {
                                 CustomerId = o.CustomerId,
                                 CustomerName = o.CustomerName,
                                 CustomerEmail = o.CustomerEmail,
                                 CustomerAddress = o.CustomerAddress,
                                 RegistrationDate = o.RegistrationDate,
                                 Id = o.Id
                             }
                         });

            var customerListDtos = await query.ToListAsync();

            return _customersExcelExporter.ExportToFile(customerListDtos);
        }

    }
}