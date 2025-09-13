using System;
using Application.Meetings.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Meeting, Meeting>();
        CreateMap<CreateMeetingDto, Meeting>();
        CreateMap<EditMeetingDto, Meeting>();
    }
}
