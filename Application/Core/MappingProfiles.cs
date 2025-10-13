using System;
using Application.Meetings.DTOs;
using Application.Profiles.DTOs;
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
        CreateMap<Meeting, MeetingDto>()
            .ForMember(d => d.HostDisplayName, o => o.MapFrom(s =>
                s.Attendees.FirstOrDefault(a => a.IsHost)!.User.DisplayName))
            .ForMember(d => d.HostId, o => o.MapFrom(s =>
                s.Attendees.FirstOrDefault(a => a.IsHost)!.User.Id));
        CreateMap<MeetingAttendee, UserProfile>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));

    }
}
