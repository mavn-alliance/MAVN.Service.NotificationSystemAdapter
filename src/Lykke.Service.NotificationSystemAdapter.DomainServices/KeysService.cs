using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.NotificationSystemAdapter.Domain.Services;
using Lykke.Service.PushNotifications.Client;
using Newtonsoft.Json;

namespace Lykke.Service.NotificationSystemAdapter.DomainServices
{
    public class KeysService : IKeysService
    {
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly IPushNotificationsClient _pushNotificationsClient;
        private readonly IDictionariesClient _dictionariesClient;
        private readonly ISettingsService _settingsService;
        private readonly string _imagesBaseUrl;

        public const string PersonalDataNamespace = "personaldata";
        public const string CommonInfoNamespace = "commoninfo";
        public const string PushNotificationsNamespace = "pushnotifications";
        public const string SettingsNamespace = "settings";

        public KeysService(ICustomerProfileClient customerProfileClient,
            IPushNotificationsClient pushNotificationsClient,
            IDictionariesClient dictionariesClient,
            ISettingsService settingsService,
            string imagesBaseUrl)
        {
            _customerProfileClient = customerProfileClient;
            _pushNotificationsClient = pushNotificationsClient;
            _dictionariesClient = dictionariesClient ?? throw new ArgumentNullException(nameof(dictionariesClient));
            _settingsService = settingsService;
            _imagesBaseUrl = imagesBaseUrl;
        }

        public async Task<Dictionary<string, string>> GetKeysAsync(string @namespace, string customerId)
        {
            var result = new Dictionary<string, string>();

            if (string.Compare(@namespace, PersonalDataNamespace, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var personalData = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId, true);

                if (personalData?.Profile == null)
                    return result;

                result.Add("Email", personalData.Profile.Email);

                result.Add("PhoneNumber", personalData.Profile.PhoneNumber);

                result.Add("FirstName", personalData.Profile.FirstName);
                result.Add("LastName", personalData.Profile.LastName);
                //We will change this in future
                result.Add("Localization", "en");
            }
            else if (string.Compare(@namespace, CommonInfoNamespace, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var commonInfo = await _dictionariesClient.CommonInformation.GetCommonInformationAsync();

                if (commonInfo == null)
                    return result;

                result.Add("TokenSymbol", _settingsService.GetTokenSymbol());
                result.Add(nameof(commonInfo.FacebookLink), commonInfo.FacebookLink);
                result.Add(nameof(commonInfo.InstagramLink), commonInfo.InstagramLink);
                result.Add(nameof(commonInfo.SupportPhoneNumber), commonInfo.SupportPhoneNumber);
                result.Add(nameof(commonInfo.TwitterLink), commonInfo.TwitterLink);
                result.Add(nameof(commonInfo.LinkedInLink), commonInfo.LinkedInLink);
                result.Add(nameof(commonInfo.YouTubeLink), commonInfo.YouTubeLink);
                result.Add(nameof(commonInfo.DownloadAppLink), commonInfo.DownloadAppLink);
                result.Add(nameof(commonInfo.TermsAndConditionLink), commonInfo.TermsAndConditionLink);
                result.Add(nameof(commonInfo.PrivacyPolicyLink), commonInfo.PrivacyPolicyLink);
                result.Add(nameof(commonInfo.UnsubscribeLink), commonInfo.UnsubscribeLink);
                result.Add(nameof(commonInfo.DownloadAndroidAppLink), commonInfo.DownloadAndroidAppLink);
                result.Add(nameof(commonInfo.DownloadIsoAppLink), commonInfo.DownloadIsoAppLink);
                result.Add(nameof(commonInfo.SupportEmailAddress), commonInfo.SupportEmailAddress);
            }
            else if (string.Compare(@namespace, PushNotificationsNamespace, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                var pushNotifications =
                    await _pushNotificationsClient.PushRegistrationsApi.GetAllPushNotificationRegistrationsForCustomerAsync(
                        customerId);
                var pushRegistrationIds = pushNotifications.Select(pn => pn.PushRegistrationToken);

                result.Add("PushRegistrationIds", JsonConvert.SerializeObject(pushRegistrationIds));
            }
            else if (string.Compare(@namespace, SettingsNamespace, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                result.Add("ImagesBaseUrl", _imagesBaseUrl);
            }

            return result;
        }
    }
}
