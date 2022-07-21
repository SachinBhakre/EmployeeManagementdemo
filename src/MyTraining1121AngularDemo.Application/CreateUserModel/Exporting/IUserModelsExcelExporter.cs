using System.Collections.Generic;
using MyTraining1121AngularDemo.CreateUserModel.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.CreateUserModel.Exporting
{
    public interface IUserModelsExcelExporter
    {
        FileDto ExportToFile(List<GetUserModelForViewDto> userModels);
    }
}