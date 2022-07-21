using MyTraining1121AngularDemo.CustomerModel;
using MyTraining1121AngularDemo.CreateUserModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyTraining1121AngularDemo.CustomerUserModels
{
    [Table("CustomerUserModels")]
    public class CustomerUserModel : FullAuditedEntity<long>
    {

        public virtual long CustomerUserId { get; set; }

        public virtual long TotalBillingAmount { get; set; }

        public virtual long? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer CustomerFk { get; set; }

        public virtual long? UserModelId { get; set; }

        [ForeignKey("UserModelId")]
        public UserModel UserModelFk { get; set; }

    }
}