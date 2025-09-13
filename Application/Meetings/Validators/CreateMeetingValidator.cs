using System;
using Application.Meetings.Commands;
using Application.Meetings.DTOs;
using FluentValidation;

namespace Application.Meetings.Validators;

public class CreateMeetingValidator : BaseMeetingValidator<CreateMeeting.Command, CreateMeetingDto>
{
    public CreateMeetingValidator() : base(x => x.MeetingDto)
    {
    }
}
