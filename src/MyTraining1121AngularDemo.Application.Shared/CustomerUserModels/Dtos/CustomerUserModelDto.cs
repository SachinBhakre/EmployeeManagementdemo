using System;
using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CustomerUserModels.Dtos
{
    public class CustomerUserModelDto : EntityDto<long>
    {
        public long CustomerUserId { get; set; }

        public long TotalBillingAmount { get; set; }

        public long? CustomerId { get; set; }

        public long? UserModelId { get; set; }

    }
}