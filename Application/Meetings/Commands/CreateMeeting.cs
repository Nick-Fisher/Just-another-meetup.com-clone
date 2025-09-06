using System;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class CreateMeeting
{
    public class Command : IRequest<string>
    {
        public required Meeting Meeting { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Command, string>
    {
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            context.Meetings.Add(request.Meeting);

            await context.SaveChangesAsync(cancellationToken);

            return request.Meeting.Id;
        }
    }
}
