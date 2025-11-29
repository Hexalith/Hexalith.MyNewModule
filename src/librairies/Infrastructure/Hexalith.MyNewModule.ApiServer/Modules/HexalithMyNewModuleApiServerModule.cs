// <copyright file="HexalithMyNewModuleApiServerModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule.ApiServer.Controllers;

using Hexalith.MyNewModule.Commands.Extensions;
using Hexalith.MyNewModule.Requests.Extensions;
using Hexalith.MyNewModule.Servers.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The document construction site client module.
/// </summary>
public sealed class HexalithMyNewModuleApiServerModule : IApiServerApplicationModule, IDocumentModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => DocumentModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Document API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModule.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith Document API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(MyNewModule);

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

        HexalithMyNewModuleAbstractionsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IDocumentModule, HexalithMyNewModuleApiServerModule>();

        // Add command handlers
        _ = services
            .AddDocumentManagement()
            .AddMyNewModuletorage()
            .AddMyNewModuleProjectionActorFactories();

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(MyNewModuleIntegrationEventsController).Assembly);
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
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.MyNewModuletorageAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.DocumentTypeAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterActor<DomainAggregateActor>(DocumentDomainHelper.FileTypeAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterAggregateRelationActor<DocumentContainer, Document>();
        actorRegistrations.RegisterProjectionActor<DataManagement>();
        actorRegistrations.RegisterProjectionActor<Document>();
        actorRegistrations.RegisterProjectionActor<DocumentContainer>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtraction>();
        actorRegistrations.RegisterProjectionActor<MyNewModuletorage>();
        actorRegistrations.RegisterProjectionActor<DocumentType>();
        actorRegistrations.RegisterProjectionActor<FileType>();
        actorRegistrations.RegisterProjectionActor<DataManagementSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DataManagementDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<MyNewModuleummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentContainerSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentContainerDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtractionSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentInformationExtractionDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<MyNewModuletorageSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<MyNewModuletorageDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentTypeSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<DocumentTypeDetailsViewModel>();
        actorRegistrations.RegisterProjectionActor<FileTypeSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<FileTypeDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DataManagementAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentContainerAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentInformationExtractionAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.MyNewModuletorageAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.DocumentTypeAggregateName));
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(DocumentDomainHelper.FileTypeAggregateName));
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}