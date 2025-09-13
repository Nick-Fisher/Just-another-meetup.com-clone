using System;
using Application.Meetings.Commands;
using Application.Meetings.DTOs;
using FluentValidation;

namespace Application.Meetings.Validators;

public class EditMeetingValidator : BaseMeetingValidator<EditMeeting.Command, EditMeetingDto>
{
    public EditMeetingValidator() : base(x => x.MeetingDto)
    {
        RuleFor(x => x.MeetingDto.Id).NotEmpty().WithMessage("Meeting ID is required.");
    }
}
