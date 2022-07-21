using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CreateUserModel.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}