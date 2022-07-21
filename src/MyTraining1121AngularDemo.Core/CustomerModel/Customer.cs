using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyTraining1121AngularDemo.CustomerModel
{
    [Table("Customers")]
    public class Customer : FullAuditedEntity<long>
    {

        public virtual long CustomerId { get; set; }

        [Required]
        [StringLength(CustomerConsts.MaxCustomerNameLength, MinimumLength = CustomerConsts.MinCustomerNameLength)]
        public virtual string CustomerName { get; set; }

        [Required]
        public virtual string CustomerEmail { get; set; }

        [StringLength(CustomerConsts.MaxCustomerAddressLength, MinimumLength = CustomerConsts.MinCustomerAddressLength)]
        public virtual string CustomerAddress { get; set; }

        public virtual DateTime RegistrationDate { get; set; }

    }
}