using System;
using AutoMapper;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Commands;

public class EditMeeting
{
    public class Command : IRequest
    {
        public required Meeting Meeting { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.Meeting.Id], cancellationToken);

            if (meeting == null) throw new Exception("Meeting not found");

            mapper.Map(request.Meeting, meeting);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
