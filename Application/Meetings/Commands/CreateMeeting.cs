using System;
using Application.Core;
using Application.Interfaces;
using Application.Meetings.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class CreateMeeting
{
    public class Command : IRequest<Result<string>>
    {
        public required CreateMeetingDto MeetingDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
     : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserAsync();

            var meeting = mapper.Map<Meeting>(request.MeetingDto);

            context.Meetings.Add(meeting);

            var attendee = new MeetingAttendee
            {
                MeetingId = meeting.Id,
                UserId = user.Id,
                IsHost = true
            };

            meeting.Attendees.Add(attendee);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<string>.Failure("Failed to create meeting", 400);

            return Result<string>.Success(meeting.Id);
        }
    }
}

