using System;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Queries;

public class GetMeetingDetails
{
    public class Query() : IRequest<Meeting>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, Meeting>
    {
        public async Task<Meeting> Handle(Query request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.Id], cancellationToken);

            if (meeting == null) throw new Exception("Meeting not found");

            return meeting;
        }
    }
}
