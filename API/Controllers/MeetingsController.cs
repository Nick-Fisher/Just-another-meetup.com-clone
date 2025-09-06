using System;
using Application.Meetings.Commands;
using Application.Meetings.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MeetingsController() : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<Meeting>>> GetMeetings(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetMeetingList.Query(), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meeting>> GetMeetingDetails(string id)
    {
        return await Mediator.Send(new GetMeetingDetails.Query { Id = id });
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateMeeting(Meeting meeting)
    {
        return await Mediator.Send(new CreateMeeting.Command { Meeting = meeting });
    }

    [HttpPut]
    public async Task<ActionResult> EditMeeting(Meeting meeting)
    {
        await Mediator.Send(new EditMeeting.Command { Meeting = meeting });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeeting(string id)
    {
        await Mediator.Send(new DeleteMeeting.Command { Id = id });

        return Ok();
    }
}
