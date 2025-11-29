// <copyright file="HexalithDocumentsApiServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Documents.Abstractions.Extensions;
using Hexalith.Documents.Application.Documents;
using Hexalith.Documents.Application.Helpers;
using Hexalith.Documents.Commands.Extensions;
using Hexalith.Documents.DataManagements;
using Hexalith.Documents.DocumentContainers;
using Hexalith.Documents.DocumentInformationExtractions;
using Hexalith.Documents.Documents;
using Hexalith.Documents.DocumentStorages;
using Hexalith.Documents.DocumentTypes;
using Hexalith.Documents.FileTypes;
using Hexalith.Documents.Requests.DataManagements;
using Hexalith.Documents.Requests.DocumentContainers;
using Hexalith.Documents.Requests.DocumentInformationExtractions;
using Hexalith.Documents.Requests.Documents;
using Hexalith.Documents.Requests.DocumentStorages;
using Hexalith.Documents.Requests.DocumentTypes;
using Hexalith.Documents.Requests.Extensions;
using Hexalith.Documents.Requests.FileTypes;
using Hexalith.Documents.Servers.Helpers;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule.ApiServer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The document construction site client module.
/// </summary>
public sealed class HexalithDocumentsApiServerModule : IApiServerApplicationModule, IDocumentModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => DocumentModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Document API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.Documents.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith Document API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(Documents);

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services
            .ConfigureSettings<CosmosDbSettings>(configuration);

        HexalithDocumentsAbstractionsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsCommandsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IDocumentModule, HexalithDocumentsApiServerModule>();

        // Add command handlers
        _ = services
            .AddDocumentManagement()
            .AddDocumentStorage()
            .AddDocumentsProjectionActorFactories();

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(DocumentsIntegrationEventsController).Assembly);
    }

    /// <summary>
    /// Registers the actors associated with the module.
    /// </summary>
    /// <param name="actorCollection">The actor collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorCollection is null.</exception>
    /// <exception cref="ArgumentException">Thrown when actorCollection is not an ActorRegistrationCollection.</exception>
    public static void RegisterActors(object actorCollection)
    {
        ArgumentNullException.ThrowIfNull(actorCollection);
        if (actorCollection is not ActorRegistrationCollection actorRegistrations)
        {
            throw new ArgumentException($"{nameof(RegisterActors)} parameter must be an {nameof(ActorRegistrationCollection)}. Actual type : {actorCollection.GetType().Name}.", nameof(actorCollection));
        }

        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DataManagementAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentContainerAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentInformationExtractionAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentStorageAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentTypeAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.FileTypeAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterAggregateRelationActor<DocumentContainer, Document>();
        actorRegistrations.RegisterProjectionActor<DataManagement>();
        actorRegistrations.RegisterProjectionActor<Document>();
        actorRegistrations.RegisterProjectionActor<DocumentContainer>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtraction>();
        actorRegistrations.RegisterProjectionActor<DocumentStorage>();
        actorRegistrations.RegisterProjectionActor<DocumentType>();
        actorRegistrations.RegisterProjectionActor<FileType>();
        actorRegistrations.RegisterProjectionActor<DataManagementSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DataManagementDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentContainerSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentContainerDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtractionSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtractionDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentStorageSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentStorageDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentTypeSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentTypeDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<FileTypeSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<FileTypeDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DataManagementAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentContainerAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentInformationExtractionAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentStorageAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentTypeAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.FileTypeAggregateName));
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}