using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CreateUserModel.Dtos
{
    public class GetUserModelForEditOutput
    {
        public CreateOrEditUserModelDto UserModel { get; set; }

    }
}