using System;
using System.Security.Claims;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Common.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
    public class IsHostRequirementHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<IsHostRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return;

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext?.GetRouteValue("id") is not string meetingId) return;

            var attendee = await dbContext.MeetingAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.MeetingId == meetingId);

            if (attendee == null) return;

            if (attendee.IsHost) context.Succeed(requirement);
        }
    }
}
