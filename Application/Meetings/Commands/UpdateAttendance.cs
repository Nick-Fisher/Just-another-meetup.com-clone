using System;
using Application.Core;
using Application.Interfaces;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Meetings.Commands;

public class UpdateAttendance
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string Id { get; set; }
    }

    public class Handler(IUserAccessor userAccessor, AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings
                .Include(x => x.Attendees)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (meeting == null) return Result<Unit>.Failure("Meeting not found", 404);

            var user = await userAccessor.GetUserAsync();

            var attendance = meeting.Attendees.FirstOrDefault(x => x.UserId == user.Id);

            var isHost = meeting.Attendees.Any(x => x.IsHost && x.UserId == user.Id);

            if (attendance != null)
            {
                if (isHost)
                {
                    meeting.IsCancelled = !meeting.IsCancelled;
                }
                else
                {
                    meeting.Attendees.Remove(attendance);
                }
            }
            else
            {
                meeting.Attendees.Add(new MeetingAttendee
                {
                    UserId = user.Id,
                    MeetingId = meeting.Id,
                    IsHost = false
                });
            }

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            return result ?
                    Result<Unit>.Success(Unit.Value) :
                    Result<Unit>.Failure("Failed to update attendance", 400);

        }
    }
}
