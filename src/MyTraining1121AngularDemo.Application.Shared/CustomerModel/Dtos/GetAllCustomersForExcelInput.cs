using Abp.Application.Services.Dto;
using System;

namespace MyTraining1121AngularDemo.CustomerModel.Dtos
{
    public class GetAllCustomersForExcelInput
    {
        public string Filter { get; set; }

        public long? MaxCustomerIdFilter { get; set; }
        public long? MinCustomerIdFilter { get; set; }

        public string CustomerNameFilter { get; set; }

        public string CustomerEmailFilter { get; set; }

        public string CustomerAddressFilter { get; set; }

        public DateTime? MaxRegistrationDateFilter { get; set; }
        public DateTime? MinRegistrationDateFilter { get; set; }

    }
}