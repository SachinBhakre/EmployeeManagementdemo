using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CreateUserModel.Dtos
{
    public class CreateOrEditUserModelDto : EntityDto<long?>
    {

        public long UserId { get; set; }

        [StringLength(UserModelConsts.MaxFirstNameLength, MinimumLength = UserModelConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(UserModelConsts.MaxLastNameLength, MinimumLength = UserModelConsts.MinLastNameLength)]
        public string LastName { get; set; }

        public string Email { get; set; }

    }
}