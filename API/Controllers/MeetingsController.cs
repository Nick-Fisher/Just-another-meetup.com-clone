using System;
using Application.Meetings.Commands;
using Application.Meetings.DTOs;
using Application.Meetings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MeetingsController() : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<MeetingDto>>> GetMeetings(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetMeetingList.Query(), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeetingDto>> GetMeetingDetails(string id)
    {
        return HandleResult(await Mediator.Send(new GetMeetingDetails.Query { Id = id }));
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateMeeting(CreateMeetingDto meetingDto)
    {
        return HandleResult(await Mediator.Send(new CreateMeeting.Command { MeetingDto = meetingDto }));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "IsMeetingHost")]
    public async Task<ActionResult> EditMeeting(string id, EditMeetingDto meeting)
    {
        meeting.Id = id;

        return HandleResult(await Mediator.Send(new EditMeeting.Command { MeetingDto = meeting }));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "IsMeetingHost")]
    public async Task<ActionResult> DeleteMeeting(string id)
    {
        return HandleResult(await Mediator.Send(new DeleteMeeting.Command { Id = id }));
    }

    [HttpPost("{id}/attend")]
    public async Task<ActionResult> Attend(string id)
    {
        return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
    }
}
