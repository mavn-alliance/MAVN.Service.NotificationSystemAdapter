using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.NotificationSystemAdapter.Client;
using Lykke.Service.NotificationSystemAdapter.Domain.Services;
using Lykke.Service.NotificationSystemAdapter.DomainServices;
using Lykke.Service.NotificationSystemAdapter.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.NotificationSystemAdapter.Controllers
{
    [Route("/api/keys/")]
    [ApiController]
    [Produces("application/json")]
    public class KeysController : Controller, INotificationSystemAdapterApi
    {
        private readonly ILog _log;
        private readonly IKeysService _keysService;

        public KeysController(IKeysService keysService, ILogFactory logFactory)
        {
            _keysService = keysService;
            _log = logFactory.CreateLog(this);
        }

        /// <inheritdoc/>
        /// <response code="200">Personal data</response>
        [ApiAuthorize]
        [HttpGet("{namespace}")]
        [SwaggerOperation("Get Keys From Namespace")]
        [ProducesResponseType(typeof(Dictionary<string, string>), (int)HttpStatusCode.OK)]
        public async Task<Dictionary<string, string>> GetKeysAsync([FromRoute] string @namespace,
            [Required] [FromQuery] string customerId)
        {
            if (string.IsNullOrEmpty(@namespace))
                throw new ValidationApiException(HttpStatusCode.BadRequest, $"{nameof(@namespace)} is not provided");

            if (string.IsNullOrEmpty(customerId))
                throw new ValidationApiException(HttpStatusCode.BadRequest, $"{nameof(customerId)} is not provided");

            var validNamespaces = new List<string>
            {
                KeysService.PersonalDataNamespace,
                KeysService.PushNotificationsNamespace,
                KeysService.CommonInfoNamespace,
                KeysService.SettingsNamespace
            };

            if (validNamespaces.All(vn => string.Compare(@namespace, vn, StringComparison.InvariantCultureIgnoreCase) != 0))
                throw new ValidationApiException(HttpStatusCode.BadRequest, $"Invalid value for {nameof(@namespace)}");

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            _log.Info("Got Keys For Customer", new { Namespace = @namespace, CustomerId = customerId});

            return result;
        }
    }
}
