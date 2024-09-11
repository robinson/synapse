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

using Neuroglia.Security;

namespace Synapse.Api.Client.Services;

/// <summary>
/// Defines the fundamentals of a service used to manage users using the Synapse API
/// </summary>
public interface IUserApiClient
{

    /// <summary>
    /// Gets the current user's profile
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>An object that describes the current user</returns>
    Task<UserInfo> GetUserProfileAsync(CancellationToken cancellationToken = default);

}