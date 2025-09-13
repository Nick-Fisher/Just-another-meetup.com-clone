using System;
using Application.Core;
using Domain;
using Infrastructure;
using MediatR;

namespace Application.Meetings.Queries;

public class GetMeetingDetails
{
    public class Query() : IRequest<Result<Meeting>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context) : IRequestHandler<Query, Result<Meeting>>
    {
        public async Task<Result<Meeting>> Handle(Query request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings.FindAsync([request.Id], cancellationToken);

            if (meeting == null) return Result<Meeting>.Failure("Meeting not found", 404);

            return Result<Meeting>.Success(meeting);
        }
    }
}
