using System;
using Application.Core;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class DeleteMeeting
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.Id], cancellationToken);

            if (meeting == null) return Result<Unit>.Failure("Meeting not found", 404);

            context.Remove(meeting);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete meeting", 400);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
