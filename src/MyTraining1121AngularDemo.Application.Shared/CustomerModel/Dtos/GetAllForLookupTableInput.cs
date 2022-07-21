using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CustomerModel.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}