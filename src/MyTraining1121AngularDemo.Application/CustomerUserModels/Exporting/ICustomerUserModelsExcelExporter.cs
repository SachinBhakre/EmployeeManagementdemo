using System.Collections.Generic;
using MyTraining1121AngularDemo.CustomerUserModels.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.CustomerUserModels.Exporting
{
    public interface ICustomerUserModelsExcelExporter
    {
        FileDto ExportToFile(List<GetCustomerUserModelForViewDto> customerUserModels);
    }
}