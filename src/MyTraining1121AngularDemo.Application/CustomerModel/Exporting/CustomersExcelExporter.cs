using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyTraining1121AngularDemo.DataExporting.Excel.NPOI;
using MyTraining1121AngularDemo.CustomerModel.Dtos;
using MyTraining1121AngularDemo.Dto;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerModel.Exporting
{
    public class CustomersExcelExporter : NpoiExcelExporterBase, ICustomersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCustomerForViewDto> customers)
        {
            return CreateExcelPackage(
                "Customers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Customers"));

                    AddHeader(
                        sheet,
                        L("CustomerId"),
                        L("CustomerName"),
                        L("CustomerEmail"),
                        L("CustomerAddress"),
                        L("RegistrationDate")
                        );

                    AddObjects(
                        sheet, customers,
                        _ => _.Customer.CustomerId,
                        _ => _.Customer.CustomerName,
                        _ => _.Customer.CustomerEmail,
                        _ => _.Customer.CustomerAddress,
                        _ => _timeZoneConverter.Convert(_.Customer.RegistrationDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    for (var i = 1; i <= customers.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5);
                });
        }
    }
}