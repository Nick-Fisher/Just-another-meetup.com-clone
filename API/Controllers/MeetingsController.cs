using System;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class MeetingsController(AppDbContext context) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Meeting>>> GetMeetings()
    {
        return await context.Meetings.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meeting>> GetMeetingDetails(string id)
    {
        var meeting = await context.Meetings.FindAsync(id);

        if (meeting == null)
        {
            return NotFound();
        }

        return meeting;
    }
}
