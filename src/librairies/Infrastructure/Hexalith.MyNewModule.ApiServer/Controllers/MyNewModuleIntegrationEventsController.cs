// <copyright file="MyNewModuleIntegrationEventsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Controllers;

using Dapr;

using Hexalith.Application.Events;
using Hexalith.Application.Projections;
using Hexalith.Application.States;
using Hexalith.Infrastructure.WebApis.Buses;
using Hexalith.Infrastructure.WebApis.Controllers;
using Hexalith.MyNewModule.Aggregates;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Controller responsible for handling MyNewModule-related integration events and managing MyNewModule processing workflows.
/// Implements the <see cref="EventIntegrationController" /> to process various MyNewModule events and update corresponding projections.
/// </summary>
/// <seealso cref="EventIntegrationController" />
/// <param name="eventProcessor">The integration event processor responsible for handling incoming events.</param>
/// <param name="projectionProcessor">The projection processor that updates read models based on processed events.</param>
/// <param name="hostEnvironment">The host environment providing runtime environment information.</param>
/// <param name="logger">The logger instance for recording diagnostic information.</param>
[ApiController]
[Route("/api/MyNewModule/events")]
[SwaggerTag("MyNewModule Integration Events Receiver")]
public class MyNewModuleIntegrationEventsController(
    IIntegrationEventProcessor eventProcessor,
    IProjectionUpdateProcessor projectionProcessor,
    IHostEnvironment hostEnvironment,
    ILogger<MyNewModuleIntegrationEventsController> logger)
    : EventIntegrationController(eventProcessor, projectionProcessor, hostEnvironment, logger)
{
    /// <summary>
    /// Processes file type events asynchronously, managing file format definitions and format-specific operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(MyNewModuleDomainHelper.MyNewModuleAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("MyNewModule")]
    [SwaggerOperation(Summary = "Handles file type events", Description = "Processes file type events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public async Task<ActionResult> HandleMyNewModuleEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                MyNewModuleDomainHelper.MyNewModuleAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);
}