using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.CustomerUserModels.Dtos;
using MyTraining1121AngularDemo.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MyTraining1121AngularDemo.CustomerUserModels
{
    public interface ICustomerUserModelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerUserModelForViewDto>> GetAll(GetAllCustomerUserModelsInput input);

        Task<GetCustomerUserModelForViewDto> GetCustomerUserModelForView(long id);

        Task<GetCustomerUserModelForEditOutput> GetCustomerUserModelForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerUserModelDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCustomerUserModelsToExcel(GetAllCustomerUserModelsForExcelInput input);

        Task<List<CustomerUserModelCustomerLookupTableDto>> GetAllCustomerForTableDropdown();

        Task<List<CustomerUserModelUserModelLookupTableDto>> GetAllUserModelForTableDropdown();

    }
}