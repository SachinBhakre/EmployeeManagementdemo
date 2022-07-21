using System;
using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CreateUserModel.Dtos
{
    public class UserModelDto : EntityDto<long>
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

    }
}