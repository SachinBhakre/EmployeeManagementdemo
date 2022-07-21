using System.Collections.Generic;
using MyTraining1121AngularDemo.CustomerModel.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.CustomerModel.Exporting
{
    public interface ICustomersExcelExporter
    {
        FileDto ExportToFile(List<GetCustomerForViewDto> customers);
    }
}