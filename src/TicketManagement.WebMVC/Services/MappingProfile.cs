using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagement.Dto;
using TicketManagement.WebMVC.ViewModels;

namespace TicketManagement.WebMVC.Services
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
