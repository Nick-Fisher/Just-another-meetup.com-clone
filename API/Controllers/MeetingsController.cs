using System;
using Application.Meetings.Commands;
using Application.Meetings.DTOs;
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
        return HandleResult(await Mediator.Send(new GetMeetingDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateMeeting(CreateMeetingDto meetingDto)
    {
        return HandleResult(await Mediator.Send(new CreateMeeting.Command { MeetingDto = meetingDto }));
    }

    [HttpPut]
    public async Task<ActionResult> EditMeeting(EditMeetingDto meeting)
    {
        return HandleResult(await Mediator.Send(new EditMeeting.Command { MeetingDto = meeting }));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMeeting(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteMeeting.Command { Id = id }));
    }
}
