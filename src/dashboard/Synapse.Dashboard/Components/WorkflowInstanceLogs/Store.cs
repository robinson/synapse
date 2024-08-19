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

using Neuroglia.Data.Infrastructure;
using Synapse.Api.Client.Services;
using System.Text.RegularExpressions;

namespace Synapse.Dashboard.Components.WorkflowInstanceLogsStateManagement;

/// <summary>
/// Represents the <see cref="ComponentStore{TState}" /> of a <see cref="WorkflowInstanceLogs"/> component
/// </summary>
/// <param name="apiClient">The service used interact with Synapse API</param>
public class WorkflowInstanceLogsStore(
    ISynapseApiClient apiClient
)
    : ComponentStore<WorkflowInstanceLogsState>(new())
{

    CancellationTokenSource _watchCancellationTokenSource = new CancellationTokenSource();

    /// <summary>
    /// Gets the service used to interact with the Synapse API
    /// </summary>
    protected ISynapseApiClient ApiClient { get; } = apiClient;

    #region Selectors
    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstanceLogsState.Name"/> changes
    /// </summary>
    public IObservable<string> Name => this.Select(state => state.Name)
        .Where(name => !string.IsNullOrWhiteSpace(name))
        .DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstanceLogsState.Namespace"/> changes
    /// </summary>
    public IObservable<string> Namespace => this.Select(state => state.Namespace)
        .Where(@namespace => !string.IsNullOrWhiteSpace(@namespace))
        .DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstanceLogsState.Loaded"/> changes
    /// </summary>
    public IObservable<bool> Loaded => this.Select(state => state.Loaded).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstanceLogsState.Logs"/> changes
    /// </summary>
    public IObservable<IEnumerable<string>> Logs => this.Select(state => state.Logs).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstanceLogsState.Name"/> and <see cref="WorkflowInstanceLogsState.Namespace"/> combined changes
    /// </summary>
    public IObservable<(string, string)> FullName => Observable.CombineLatest(
        this.Name,
        this.Namespace,
        (name, @namespace) => (name, @namespace)
    ).DistinctUntilChanged();
    #endregion

    #region Setters
    /// <summary>
    /// Sets the state's <see cref="WorkflowInstanceLogsState.Name"/>
    /// </summary>
    /// <param name="name">The new name</param>
    public void SetName(string name)
    {
        this.Reduce(state => state with
        {
            Name = name
        });
    }

    /// <summary>
    /// Sets the state's <see cref="WorkflowInstanceLogsState.Namespace"/>
    /// </summary>
    /// <param name="namespace">The new namespace</param>
    public void SetNamespace(string @namespace)
    {
        this.Reduce(state => state with {
            Namespace = @namespace
        });
    }
    #endregion

    #region Actions
    /// <summary>
    /// Reads and watches the logs
    /// </summary>
    public async Task LoadLogsAsync()
    {
        this.Reduce(state => state with
        {
            Loaded = false
        });
        await this.ReadLogsAsync();
        await this.WatchLogsAsync(); //fire and forget, otherwise the subscription is blocked
    }

    /// <summary>
    /// Reads the logs form the API
    /// </summary>
    protected async Task ReadLogsAsync()
    {
        var name = this.Get(state => state.Name);
        var @namespace = this.Get(state => state.Namespace);
        var logs = await this.ApiClient.WorkflowInstances.ReadLogsAsync(name, @namespace, this.CancellationTokenSource.Token).ConfigureAwait(false);
        this.Reduce(state => state with
        {
            Logs = this.SplitLogs(logs),
            Loaded = true
        });
    }

    /// <summary>
    /// Watches the logs
    /// </summary>
    protected async Task WatchLogsAsync()
    {
        this._watchCancellationTokenSource.Cancel();
        this._watchCancellationTokenSource.Dispose();
        this._watchCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.CancellationTokenSource.Token);
        var name = this.Get(state => state.Name);
        var @namespace = this.Get(state => state.Namespace);
        await foreach (ITextDocumentWatchEvent evt in await this.ApiClient.WorkflowInstances.WatchLogsAsync(name, @namespace, this._watchCancellationTokenSource.Token).ConfigureAwait(false))
        {
            Console.WriteLine(evt?.Content);
            if (evt != null && !string.IsNullOrEmpty(evt.Content))
            {
                if (evt.Type == TextDocumentWatchEventType.Appended) 
                {
                    var logs = this.Get(state => state.Logs) ?? [];
                    this.Reduce(state => state with
                    {
                        Logs = [.. logs, .. this.SplitLogs(evt.Content)]
                    });
                }
                else if (evt.Type == TextDocumentWatchEventType.Replaced)
                {
                    this.Reduce(state => state with
                    {
                        Logs = this.SplitLogs(evt.Content)
                    });
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Splits the provided logs
    /// </summary>
    /// <param name="logs">A blob of text containing the logs</param>
    /// <returns>A <see cref="IEnumerable{T}"/> of log lines</returns>
    protected IEnumerable<string> SplitLogs(string logs)
    {
        return logs.Replace("\r\n", "\n")
            .Split('\n');
    }

    private bool disposed;
    /// <summary>
    /// Disposes of the store
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the dispose of the store</param>
    protected override void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                if (this._watchCancellationTokenSource != null)
                {
                    this._watchCancellationTokenSource.Cancel();
                    this._watchCancellationTokenSource.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}