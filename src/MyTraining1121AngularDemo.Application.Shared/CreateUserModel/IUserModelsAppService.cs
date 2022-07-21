using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.CreateUserModel.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.CreateUserModel
{
    public interface IUserModelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetUserModelForViewDto>> GetAll(GetAllUserModelsInput input);

        Task<GetUserModelForViewDto> GetUserModelForView(long id);

        Task<GetUserModelForEditOutput> GetUserModelForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditUserModelDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetUserModelsToExcel(GetAllUserModelsForExcelInput input);

    }
}