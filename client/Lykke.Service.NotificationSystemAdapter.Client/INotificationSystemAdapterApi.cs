using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Lykke.Service.NotificationSystemAdapter.Client
{
    // This is an example of service controller interfaces.
    // Actual interface methods must be placed here (not in INotificationSystemAdapterClient interface).

    /// <summary>
    /// NotificationSystemAdapter client API interface.
    /// </summary>
    [PublicAPI]
    public interface INotificationSystemAdapterApi
    {
        /// <summary>
        /// Get customer personal data
        /// </summary>
        /// <param name="namespace">Namespace in which we want to look for keys</param>
        /// <param name="customerId">Id of a customer that we want to get keys for</param>
        /// <returns>Found keys for provided namespace</returns>
        [Get("/api/keys/{namespace}")]
        Task<Dictionary<string, string>> GetKeysAsync([FromRoute] string @namespace, string customerId);
    }
}
