using System;
using Application.Core;
using Application.Meetings.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Meetings.Queries;

public class GetMeetingDetails
{
    public class Query() : IRequest<Result<MeetingDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<MeetingDto>>
    {
        public async Task<Result<MeetingDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings
                            .ProjectTo<MeetingDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (meeting == null) return Result<MeetingDto>.Failure("Meeting not found", 404);

            return Result<MeetingDto>.Success(meeting);
        }
    }
}
