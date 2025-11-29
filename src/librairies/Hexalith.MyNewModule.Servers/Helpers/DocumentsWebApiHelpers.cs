// <copyright file="DocumentsWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Helpers;

using Hexalith.Application.Services;
using Hexalith.Documents.Application;
using Hexalith.Documents.Application.Services;
using Hexalith.Documents.DataManagements;
using Hexalith.Documents.DocumentContainers;
using Hexalith.Documents.DocumentInformationExtractions;
using Hexalith.Documents.Documents;
using Hexalith.Documents.DocumentStorages;
using Hexalith.Documents.DocumentTypes;
using Hexalith.Documents.FileTypes;
using Hexalith.Documents.Projections.Helpers;
using Hexalith.Documents.Requests.DataManagements;
using Hexalith.Documents.Requests.DocumentContainers;
using Hexalith.Documents.Requests.DocumentInformationExtractions;
using Hexalith.Documents.Requests.Documents;
using Hexalith.Documents.Requests.DocumentStorages;
using Hexalith.Documents.Requests.DocumentTypes;
using Hexalith.Documents.Requests.FileTypes;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Services;
using Hexalith.MyNewModule.Servers.Services;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class DocumentsWebApiHelpers.
/// </summary>
public static class DocumentsWebApiHelpers
{
    /// <summary>
    /// Adds the document projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddDocumentsProjectionActorFactories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddTransient<IWritableFileProvider, WritableFileProvider>();
        _ = services.AddTransient<IReadableFileProvider, WritableFileProvider>();
        _ = services.AddScoped<IOneToManyAggregateRelationService<DocumentContainer, Document>, OneToManyAggregateRelationService<DocumentContainer, Document>>();
        _ = services.AddDocumentProjections();
        _ = services.AddActorRelationFactory<DocumentContainer, Document>();
        _ = services.AddActorProjectionFactory<DataManagement>();
        _ = services.AddActorProjectionFactory<DataManagementSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DataManagementDetailsViewModel>();
        _ = services.AddActorProjectionFactory<FileType>();
        _ = services.AddActorProjectionFactory<FileTypeSummaryViewModel>();
        _ = services.AddActorProjectionFactory<FileTypeDetailsViewModel>();
        _ = services.AddActorProjectionFactory<DocumentType>();
        _ = services.AddActorProjectionFactory<DocumentTypeSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentTypeDetailsViewModel>();
        _ = services.AddActorProjectionFactory<Document>();
        _ = services.AddActorProjectionFactory<DocumentSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentDetailsViewModel>();
        _ = services.AddActorProjectionFactory<DocumentContainer>();
        _ = services.AddActorProjectionFactory<DocumentContainerSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentContainerDetailsViewModel>();
        _ = services.AddActorProjectionFactory<DocumentStorage>();
        _ = services.AddActorProjectionFactory<DocumentStorageSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentStorageDetailsViewModel>();
        _ = services.AddActorProjectionFactory<DocumentInformationExtraction>();
        _ = services.AddActorProjectionFactory<DocumentInformationExtractionSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentInformationExtractionDetailsViewModel>();
        return services;
    }

    /// <summary>
    /// Adds the document storage services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddDocumentStorage(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.AddTransient<AzureContainerStorage>();
        _ = services.AddTransient<OneDriveStorage>();
        _ = services.AddTransient<FileSystemStorage>();
        _ = services.AddTransient<DropboxStorage>();
        _ = services.AddTransient<GoogleDriveStorage>();
        _ = services.AddTransient<AwsS3BucketStorage>();
        _ = services.AddTransient<SharepointStorage>();
        _ = services.AddScoped<IDocumentUploadService, DocumentUploadService>();
        return services;
    }
}