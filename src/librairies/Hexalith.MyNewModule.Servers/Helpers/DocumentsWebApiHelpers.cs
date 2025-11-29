// <copyright file="DocumentsWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Helpers;

using Hexalith.Application.Services;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Services;
using Hexalith.MyNewModule.Servers.Services;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class MyNewModuleWebApiHelpers.
/// </summary>
public static class MyNewModuleWebApiHelpers
{
    /// <summary>
    /// Adds the document projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddMyNewModuleProjectionActorFactories(this IServiceCollection services)
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
        _ = services.AddActorProjectionFactory<MyNewModuleummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentDetailsViewModel>();
        _ = services.AddActorProjectionFactory<DocumentContainer>();
        _ = services.AddActorProjectionFactory<DocumentContainerSummaryViewModel>();
        _ = services.AddActorProjectionFactory<DocumentContainerDetailsViewModel>();
        _ = services.AddActorProjectionFactory<MyNewModuletorage>();
        _ = services.AddActorProjectionFactory<MyNewModuletorageSummaryViewModel>();
        _ = services.AddActorProjectionFactory<MyNewModuletorageDetailsViewModel>();
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
    public static IServiceCollection AddMyNewModuletorage(this IServiceCollection services)
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