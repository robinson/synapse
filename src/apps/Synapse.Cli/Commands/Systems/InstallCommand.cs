﻿/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using Microsoft.Extensions.DependencyInjection;
using Synapse.Cli.Commands.Systems.Installs;

namespace Synapse.Cli.Commands.Systems
{

    /// <summary>
    /// Represents the <see cref="Command"/> used to cancel the execution of a <see cref="V1WorkflowInstance"/>
    /// </summary>
    internal class InstallCommand
        : Command
    {

        /// <summary>
        /// Gets the <see cref="InstallCommand"/>'s name
        /// </summary>
        public const string CommandName = "install";
        /// <summary>
        /// Gets the <see cref="InstallCommand"/>'s description
        /// </summary>
        public const string CommandDescription = "Installs Synapse";

        /// <inheritdoc/>
        public InstallCommand(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, ISynapseManagementApi synapseManagementApi) 
            : base(serviceProvider, loggerFactory, synapseManagementApi, CommandName, CommandDescription)
        {
            this.AddCommand(ActivatorUtilities.CreateInstance<NativeInstallCommand>(this.ServiceProvider));
            this.AddCommand(ActivatorUtilities.CreateInstance<DockerInstallCommand>(this.ServiceProvider));
        }

    }

}
