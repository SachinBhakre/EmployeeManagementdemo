using Abp.Application.Services.Dto;
using System;

namespace MyTraining1121AngularDemo.CustomerUserModels.Dtos
{
    public class GetAllCustomerUserModelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public long? MaxCustomerUserIdFilter { get; set; }
        public long? MinCustomerUserIdFilter { get; set; }

        public long? MaxTotalBillingAmountFilter { get; set; }
        public long? MinTotalBillingAmountFilter { get; set; }

        public string CustomerCustomerNameFilter { get; set; }

        public string UserModelFirstNameFilter { get; set; }

    }
}