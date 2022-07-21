using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyTraining1121AngularDemo.DataExporting.Excel.NPOI;
using MyTraining1121AngularDemo.CustomerUserModels.Dtos;
using MyTraining1121AngularDemo.Dto;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerUserModels.Exporting
{
    public class CustomerUserModelsExcelExporter : NpoiExcelExporterBase, ICustomerUserModelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomerUserModelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCustomerUserModelForViewDto> customerUserModels)
        {
            return CreateExcelPackage(
                "CustomerUserModels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CustomerUserModels"));

                    AddHeader(
                        sheet,
                        L("CustomerUserId"),
                        L("TotalBillingAmount"),
                        (L("Customer")) + L("CustomerName"),
                        (L("UserModel")) + L("FirstName")
                        );

                    AddObjects(
                        sheet, customerUserModels,
                        _ => _.CustomerUserModel.CustomerUserId,
                        _ => _.CustomerUserModel.TotalBillingAmount,
                        _ => _.CustomerCustomerName,
                        _ => _.UserModelFirstName
                        );

                });
        }
    }
}