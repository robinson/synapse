﻿// Copyright © 2024-Present The Synapse Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Synapse.Events.Tasks;

/// <summary>
/// Represents the data carried by the cloud event that notifies that a task has faulted
/// </summary>
[CloudEvent(SynapseDefaults.CloudEvents.Task.Faulted.v1)]
public record TaskFaultedEventV1
{

    /// <summary>
    /// Gets/sets the qualified name of the workflow instance the task that has faulted belongs to
    /// </summary>
    [DataMember(Name = "workflow", Order = 1), JsonPropertyName("workflow"), JsonPropertyOrder(1), YamlMember(Alias = "workflow", Order = 1)]
    public required string Workflow { get; set; }

    /// <summary>
    /// Gets/sets the reference of the task that has faulted
    /// </summary>
    [DataMember(Name = "task", Order = 2), JsonPropertyName("task"), JsonPropertyOrder(2), YamlMember(Alias = "task", Order = 2)]
    public required Uri Task { get; set; }

    /// <summary>
    /// Gets/sets the error that has cause the task to fault
    /// </summary>
    [DataMember(Name = "error", Order = 3), JsonPropertyName("error"), JsonPropertyOrder(3), YamlMember(Alias = "error", Order = 3)]
    public required Error Error { get; set; }

    /// <summary>
    /// Gets/sets the date and time at which the task has faulted
    /// </summary>
    [DataMember(Name = "faultedAt", Order = 4), JsonPropertyName("faultedAt"), JsonPropertyOrder(4), YamlMember(Alias = "faultedAt", Order = 4)]
    public DateTimeOffset FaultedAt { get; set; } = DateTimeOffset.Now;

}