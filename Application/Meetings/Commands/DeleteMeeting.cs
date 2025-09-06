using System;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class DeleteMeeting
{
    public class Command : IRequest
    {
        public string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.Id], cancellationToken);

            if (meeting == null) throw new Exception("No meetings found");

            context.Remove(meeting);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
