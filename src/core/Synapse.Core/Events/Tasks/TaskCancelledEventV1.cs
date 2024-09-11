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
/// Represents the data carried by the cloud event that notifies that the execution of a task has been cancelled
/// </summary>
[CloudEvent(SynapseDefaults.CloudEvents.Task.Cancelled.v1)]
public record TaskCancelledEventV1
{

    /// <summary>
    /// Gets/sets the qualified name of the workflow instance the task that has been cancelled belongs to
    /// </summary>
    [DataMember(Name = "workflow", Order = 1), JsonPropertyName("workflow"), JsonPropertyOrder(1), YamlMember(Alias = "workflow", Order = 1)]
    public required string Workflow { get; set; }

    /// <summary>
    /// Gets/sets the reference of the task that has been cancelled
    /// </summary>
    [DataMember(Name = "task", Order = 2), JsonPropertyName("task"), JsonPropertyOrder(2), YamlMember(Alias = "task", Order = 2)]
    public required Uri Task { get; set; }

    /// <summary>
    /// Gets/sets the date and time at which the workflow instance has been cancelled
    /// </summary>
    [DataMember(Name = "cancelledAt", Order = 2), JsonPropertyName("cancelledAt"), JsonPropertyOrder(2), YamlMember(Alias = "cancelledAt", Order = 2)]
    public DateTimeOffset CancelledAt { get; set; } = DateTimeOffset.Now;

}