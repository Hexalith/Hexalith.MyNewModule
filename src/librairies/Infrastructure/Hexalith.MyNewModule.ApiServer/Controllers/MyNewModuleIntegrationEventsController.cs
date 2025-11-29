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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// Controller responsible for handling document-related integration events and managing document processing workflows.
/// Implements the <see cref="EventIntegrationController" /> to process various document events and update corresponding projections.
/// </summary>
/// <seealso cref="EventIntegrationController" />
/// <param name="eventProcessor">The integration event processor responsible for handling incoming events.</param>
/// <param name="projectionProcessor">The projection processor that updates read models based on processed events.</param>
/// <param name="hostEnvironment">The host environment providing runtime environment information.</param>
/// <param name="logger">The logger instance for recording diagnostic information.</param>
[ApiController]
[Route("/api/MyNewModule/events")]
[SwaggerTag("Document Integration Events Receiver")]
public class MyNewModuleIntegrationEventsController(
    IIntegrationEventProcessor eventProcessor,
    IProjectionUpdateProcessor projectionProcessor,
    IHostEnvironment hostEnvironment,
    ILogger<MyNewModuleIntegrationEventsController> logger)
    : EventIntegrationController(eventProcessor, projectionProcessor, hostEnvironment, logger)
{
    /// <summary>
    /// Processes data management events asynchronously, handling operations related to document data management and maintenance.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>
    /// A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.
    /// </returns>
    [EventBusTopic(DocumentDomainHelper.DataManagementAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("datamanagement")]
    [SwaggerOperation(Summary = "Handles data management events", Description = "Processes data management events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleDataManagementEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.DataManagementAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes document container events asynchronously, managing operations related to document organization and grouping.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>
    /// A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.
    /// </returns>
    [EventBusTopic(DocumentDomainHelper.DocumentContainerAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("documentcontainer")]
    [SwaggerOperation(Summary = "Handles document container events", Description = "Processes document container events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleDocumentContainerEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.DocumentContainerAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes core document events asynchronously, handling basic document lifecycle operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(DocumentDomainHelper.DocumentAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("document")]
    [SwaggerOperation(Summary = "Handles document events", Description = "Processes document events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleDocumentEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.DocumentAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes document information extraction events asynchronously, managing content analysis and data extraction operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(DocumentDomainHelper.DocumentInformationExtractionAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("documentinformationextraction")]
    [SwaggerOperation(Summary = "Handles document information extraction events", Description = "Processes document information extraction events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleDocumentInformationExtractionEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.DocumentInformationExtractionAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes document type events asynchronously, managing document classification and type-specific operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(DocumentDomainHelper.DocumentTypeAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("documenttype")]
    [SwaggerOperation(Summary = "Handles document type events", Description = "Processes document type events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleDocumentTypeEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.DocumentTypeAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes file type events asynchronously, managing file format definitions and format-specific operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(DocumentDomainHelper.FileTypeAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("filetype")]
    [SwaggerOperation(Summary = "Handles file type events", Description = "Processes file type events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleFileTypeEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.FileTypeAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);

    /// <summary>
    /// Processes document storage events asynchronously, managing physical storage and retrieval operations.
    /// </summary>
    /// <param name="eventState">The event state containing the message payload and metadata for processing.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation result:
    /// - 200 OK if the event was processed successfully
    /// - 400 Bad Request if the event data is invalid
    /// - 500 Internal Server Error if processing fails.</returns>
    [EventBusTopic(DocumentDomainHelper.MyNewModuletorageAggregateName)]
    [TopicMetadata("requireSessions", "true")]
    [TopicMetadata("sessionIdleTimeoutInSec ", "15")]
    [TopicMetadata("maxConcurrentSessions", "32")]
    [HttpPost("MyNewModuletorage")]
    [SwaggerOperation(Summary = "Handles document storage events", Description = "Processes document storage events and updates projections accordingly.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed successfully.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid event data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while processing the event.")]
    public static async Task<ActionResult> HandleMyNewModuletorageEventsAsync(MessageState eventState)
         => await HandleEventAsync(
                eventState,
                DocumentDomainHelper.MyNewModuletorageAggregateName,
                CancellationToken.None)
             .ConfigureAwait(false);
}