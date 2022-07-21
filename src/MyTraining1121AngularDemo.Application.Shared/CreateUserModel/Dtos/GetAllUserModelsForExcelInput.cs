using Abp.Application.Services.Dto;
using System;

namespace MyTraining1121AngularDemo.CreateUserModel.Dtos
{
    public class GetAllUserModelsForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxUserIdFilter { get; set; }
        public long? MinUserIdFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string EmailFilter { get; set; }

    }
}