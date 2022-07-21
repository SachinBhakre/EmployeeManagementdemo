using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyTraining1121AngularDemo.CreateUserModel
{
    [Table("UserModels")]
    public class UserModel : FullAuditedEntity<long>
    {

        public virtual long UserId { get; set; }

        [StringLength(UserModelConsts.MaxFirstNameLength, MinimumLength = UserModelConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(UserModelConsts.MaxLastNameLength, MinimumLength = UserModelConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        public virtual string Email { get; set; }

    }
}