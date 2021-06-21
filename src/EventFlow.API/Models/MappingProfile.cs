using AutoMapper;
using TicketManagement.Dto;

namespace TicketManagement.Services.EventFlow.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EventAreaDto, EventAreaItem>();
            CreateMap<EventAreaItem, EventAreaDto>();
        }
    }
}
