using System;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Meetings.Queries;

public class GetMeetingList
{
    public class Query : IRequest<List<Meeting>> { }

    public class Handler(AppDbContext context) : IRequestHandler<Query, List<Meeting>>
    {
        public async Task<List<Meeting>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Meetings.ToListAsync(cancellationToken);
        }
    }
}
