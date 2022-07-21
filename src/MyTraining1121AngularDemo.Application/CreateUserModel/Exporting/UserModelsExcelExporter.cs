using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyTraining1121AngularDemo.DataExporting.Excel.NPOI;
using MyTraining1121AngularDemo.CreateUserModel.Dtos;
using MyTraining1121AngularDemo.Dto;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CreateUserModel.Exporting
{
    public class UserModelsExcelExporter : NpoiExcelExporterBase, IUserModelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserModelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserModelForViewDto> userModels)
        {
            return CreateExcelPackage(
                "UserModels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("UserModels"));

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("FirstName"),
                        L("LastName"),
                        L("Email")
                        );

                    AddObjects(
                        sheet, userModels,
                        _ => _.UserModel.UserId,
                        _ => _.UserModel.FirstName,
                        _ => _.UserModel.LastName,
                        _ => _.UserModel.Email
                        );

                });
        }
    }
}