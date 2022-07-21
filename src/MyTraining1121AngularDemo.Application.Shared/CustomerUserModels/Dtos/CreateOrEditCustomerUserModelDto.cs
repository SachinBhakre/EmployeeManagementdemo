using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CustomerUserModels.Dtos
{
    public class CreateOrEditCustomerUserModelDto : EntityDto<long?>
    {

        public long CustomerUserId { get; set; }

        public long TotalBillingAmount { get; set; }

        public long? CustomerId { get; set; }

        public long? UserModelId { get; set; }

    }
}