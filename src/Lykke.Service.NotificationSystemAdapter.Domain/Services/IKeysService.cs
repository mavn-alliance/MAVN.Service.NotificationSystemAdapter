using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.NotificationSystemAdapter.Domain.Services
{
    public interface IKeysService
    {
        /// <summary>
        /// Get keys
        /// </summary>
        /// <param name="namespace">Namespace to get keys for</param>
        /// <param name="customerId">Customer Id</param>
        /// <returns>Found keys for provided namespace</returns>
        Task<Dictionary<string, string>> GetKeysAsync(string @namespace, string customerId);
    }
}
