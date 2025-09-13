using System;
using Application.Core;
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

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {

            var meeting = mapper.Map<Meeting>(request.MeetingDto);

            context.Meetings.Add(meeting);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<string>.Failure("Failed to create meeting", 400);

            return Result<string>.Success(meeting.Id);
        }
    }
}

