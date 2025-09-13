using System;
using Application.Core;
using Application.Meetings.DTOs;
using AutoMapper;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class EditMeeting
{
    public class Command : IRequest<Result<Unit>>
    {
        public required EditMeetingDto MeetingDto { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.MeetingDto.Id], cancellationToken);

            if (meeting == null) return Result<Unit>.Failure("Meeting not found", 404);

            mapper.Map(request.MeetingDto, meeting);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update meeting", 400);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
