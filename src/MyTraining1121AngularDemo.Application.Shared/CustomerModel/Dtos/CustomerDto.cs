using System;
using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CustomerModel.Dtos
{
    public class CustomerDto : EntityDto<long>
    {
        public long CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}