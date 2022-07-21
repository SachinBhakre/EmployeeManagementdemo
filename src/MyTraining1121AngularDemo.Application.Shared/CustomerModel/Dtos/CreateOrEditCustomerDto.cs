using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CustomerModel.Dtos
{
    public class CreateOrEditCustomerDto : EntityDto<long?>
    {

        public long CustomerId { get; set; }

        [Required]
        [StringLength(CustomerConsts.MaxCustomerNameLength, MinimumLength = CustomerConsts.MinCustomerNameLength)]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        [StringLength(CustomerConsts.MaxCustomerAddressLength, MinimumLength = CustomerConsts.MinCustomerAddressLength)]
        public string CustomerAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}