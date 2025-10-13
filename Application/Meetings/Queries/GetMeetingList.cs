using System;
using Application.Meetings.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Meetings.Queries;

public class GetMeetingList
{
    public class Query : IRequest<List<MeetingDto>> { }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, List<MeetingDto>>
    {
        public async Task<List<MeetingDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await context.Meetings.ProjectTo<MeetingDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
    }
}
