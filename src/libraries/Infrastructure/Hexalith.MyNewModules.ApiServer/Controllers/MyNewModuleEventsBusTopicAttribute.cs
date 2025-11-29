// <copyright file="MyNewModuleEventsBusTopicAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModules.ApiServer.Controllers;

using Hexalith.Infrastructure.WebApis.Buses;
using Hexalith.MyNewModules.Aggregates;

/// <summary>
/// Class CustomerEventsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="EventBusTopicAttribute" />.
/// </summary>
/// <seealso cref="EventBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class MyNewModuleEventsBusTopicAttribute : EventBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModuleEventsBusTopicAttribute"/> class.
    /// </summary>
    public MyNewModuleEventsBusTopicAttribute()
        : base(MyNewModuleDomainHelper.MyNewModuleAggregateName)
    {
    }
}