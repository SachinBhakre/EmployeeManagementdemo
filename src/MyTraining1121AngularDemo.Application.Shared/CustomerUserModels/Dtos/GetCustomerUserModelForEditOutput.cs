using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CustomerUserModels.Dtos
{
    public class GetCustomerUserModelForEditOutput
    {
        public CreateOrEditCustomerUserModelDto CustomerUserModel { get; set; }

        public string CustomerCustomerName { get; set; }

        public string UserModelFirstName { get; set; }

    }
}