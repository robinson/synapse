﻿/*
 * Copyright © 2022-Present The Synapse Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace Synapse.Dashboard
{

    /// <summary>
    /// Represents a logical gateway <see cref="WorkflowNodeViewModel"/>
    /// </summary>
    public class GatewayNodeViewModel
        : WorkflowNodeViewModel
    {

        /// <summary>
        /// Initializes a new <see cref="GatewayNodeViewModel"/>
        /// </summary>
        /// <param name="type">The gateway's type</param>
        public GatewayNodeViewModel(GatewayNodeType type)
            : base("", "gateway-node", null, Neuroglia.Blazor.Dagre.Constants.NodeHeight * 1.5, Neuroglia.Blazor.Dagre.Constants.NodeHeight * 1.5)
        {
            this.Type = type;
            this.ComponentType = typeof(GatewayNodeTemplate);
        }

        /// <summary>
        /// Gets the gateway's ParallelCompletionType
        /// </summary>
        public GatewayNodeType Type { get; }

    }
    
}
