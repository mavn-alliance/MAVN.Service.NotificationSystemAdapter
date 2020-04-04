using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Responses;
using Lykke.Service.Dictionaries.Client;
using Lykke.Service.Dictionaries.Client.Models.Notifications;
using MAVN.Service.NotificationSystemAdapter.Domain.Services;
using MAVN.Service.NotificationSystemAdapter.DomainServices;
using Lykke.Service.PushNotifications.Client;
using Lykke.Service.PushNotifications.Client.Models.Responses;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace MAVN.Service.NotificationSystemAdapter.Tests
{
    public class KeysServiceTests
    {
        private readonly IKeysService _keysService;
        private readonly Mock<ICustomerProfileClient> _personalDataServiceMock = new Mock<ICustomerProfileClient>();

        private readonly Mock<IPushNotificationsClient> _pushNotificationsClientMock =
            new Mock<IPushNotificationsClient>();

        private readonly Mock<IDictionariesClient> _dictionaryClientMock = new Mock<IDictionariesClient>();
        private readonly Mock<ISettingsService> _settingsServiceMock = new Mock<ISettingsService>();
        private const string ImagesBaseUrl = "dummy blobs url";


        public KeysServiceTests()
        {
            _keysService = new KeysService(_personalDataServiceMock.Object, _pushNotificationsClientMock.Object,
                _dictionaryClientMock.Object, _settingsServiceMock.Object, ImagesBaseUrl);
        }

        [Fact]
        public async Task
            When_GetKeys_Is_Executed_And_Existing_Namespace_Is_Provided_Expect_That_Personal_Data_GetAsync_Is_Called()
        {
            var @namespace = KeysService.PersonalDataNamespace;
            var customerId = "test";

            _personalDataServiceMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), true, It.IsAny<bool>()))
                .ReturnsAsync(
                    new CustomerProfileResponse
                    {
                        Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
                        {
                            FirstName = "test", LastName = "test", Email = "test", PhoneNumber = "test"
                        }
                    });

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            _personalDataServiceMock.Verify(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), true, It.IsAny<bool>()),
                Times.Once);

            Assert.True(result.ContainsKey("Email"));
            Assert.Equal("test", result.GetValueOrDefault("Email"));

            Assert.True(result.ContainsKey("PhoneNumber"));
            Assert.Equal("test", result.GetValueOrDefault("PhoneNumber"));

            Assert.True(result.ContainsKey("FirstName"));
            Assert.Equal("test", result.GetValueOrDefault("FirstName"));

            Assert.True(result.ContainsKey("LastName"));
            Assert.Equal("test", result.GetValueOrDefault("LastName"));
        }

        [Fact]
        public async Task
            When_GetKeysIsExecuted_And_CustomerProfileNull_Expect_NoResults()
        {
            var @namespace = KeysService.PersonalDataNamespace;
            var customerId = "test";

            _personalDataServiceMock.Setup(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), true, It.IsAny<bool>()))
                .ReturnsAsync(
                    new CustomerProfileResponse {Profile = null});

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            _personalDataServiceMock.Verify(x => x.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), true, It.IsAny<bool>()),
                Times.Once);

            Assert.True(result.IsNullOrEmpty());
        }

        [Fact]
        public async Task
            When_GetKeysIsExecuted_And_ExistingNamespaceIsProvided_Expect_That_Personal_Data_GetAsync_Is_Called()
        {
            var @namespace = KeysService.PushNotificationsNamespace;
            var customerId = "test";

            var infobipToken = "toast";

            _pushNotificationsClientMock
                .Setup(x => x.PushRegistrationsApi.GetAllPushNotificationRegistrationsForCustomerAsync(customerId))
                .ReturnsAsync(
                    new List<GetPushRegistrationResponseModel>
                    {
                        new GetPushRegistrationResponseModel {PushRegistrationToken = infobipToken}
                    });

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            var expectedResult = JsonConvert.SerializeObject(new List<string> {infobipToken});

            _pushNotificationsClientMock
                .Verify(x => x.PushRegistrationsApi.GetAllPushNotificationRegistrationsForCustomerAsync(customerId), Times.Once);
            Assert.True(result.ContainsKey("PushRegistrationIds"));
            Assert.Equal(expectedResult, result.GetValueOrDefault("PushRegistrationIds"));
        }

        [Fact]
        public async Task
            When_GetKeys_Is_Executed_And_EmailCommonSpace_Namespace_Is_Provided_Expect_That_DictionaryClient_GetEmailNotificationPropertiesAsync_Is_Called()
        {
            var @namespace = KeysService.CommonInfoNamespace;
            var customerId = "test";
            var commonData = new CommonInformationPropertiesModel()
            {
                SupportPhoneNumber = "phone",
                FacebookLink = "facebook",
                TwitterLink = "twitter",
                InstagramLink = "instagram",
                LinkedInLink = "linkedin",
                YouTubeLink = "youtube",
                DownloadAppLink = "download",
                TermsAndConditionLink = "terms",
                PrivacyPolicyLink = "policy",
                UnsubscribeLink = "unsubscribe",
                DownloadAndroidAppLink = "androidLink",
                DownloadIsoAppLink = "isoLink",
                SupportEmailAddress = "email"
            };

            _dictionaryClientMock.Setup(x => x.CommonInformation.GetCommonInformationAsync())
                .ReturnsAsync(commonData);

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            _dictionaryClientMock.Verify(x => x.CommonInformation.GetCommonInformationAsync(), Times.Once);

            Assert.True(result.ContainsKey(nameof(commonData.FacebookLink)));
            Assert.Equal(commonData.FacebookLink, result.GetValueOrDefault(nameof(commonData.FacebookLink)));

            Assert.True(result.ContainsKey(nameof(commonData.SupportPhoneNumber)));
            Assert.Equal(commonData.SupportPhoneNumber,
                result.GetValueOrDefault(nameof(commonData.SupportPhoneNumber)));

            Assert.True(result.ContainsKey(nameof(commonData.InstagramLink)));
            Assert.Equal(commonData.InstagramLink,
                result.GetValueOrDefault(nameof(commonData.InstagramLink)));

            Assert.True(result.ContainsKey(nameof(commonData.LinkedInLink)));
            Assert.Equal(commonData.LinkedInLink, result.GetValueOrDefault(nameof(commonData.LinkedInLink)));

            Assert.True(result.ContainsKey(nameof(commonData.YouTubeLink)));
            Assert.Equal(commonData.YouTubeLink, result.GetValueOrDefault(nameof(commonData.YouTubeLink)));

            Assert.True(result.ContainsKey(nameof(commonData.DownloadAppLink)));
            Assert.Equal(commonData.DownloadAppLink,
                result.GetValueOrDefault(nameof(commonData.DownloadAppLink)));

            Assert.True(result.ContainsKey(nameof(commonData.TermsAndConditionLink)));
            Assert.Equal(commonData.TermsAndConditionLink,
                result.GetValueOrDefault(nameof(commonData.TermsAndConditionLink)));

            Assert.True(result.ContainsKey(nameof(commonData.PrivacyPolicyLink)));
            Assert.Equal(commonData.PrivacyPolicyLink,
                result.GetValueOrDefault(nameof(commonData.PrivacyPolicyLink)));

            Assert.True(result.ContainsKey(nameof(commonData.UnsubscribeLink)));
            Assert.Equal(commonData.UnsubscribeLink,
                result.GetValueOrDefault(nameof(commonData.UnsubscribeLink)));

            Assert.True(result.ContainsKey(nameof(commonData.SupportEmailAddress)));
            Assert.Equal(commonData.SupportEmailAddress,
                result.GetValueOrDefault(nameof(commonData.SupportEmailAddress)));

            Assert.True(result.ContainsKey(nameof(commonData.DownloadIsoAppLink)));
            Assert.Equal(commonData.DownloadIsoAppLink,
                result.GetValueOrDefault(nameof(commonData.DownloadIsoAppLink)));

            Assert.True(result.ContainsKey(nameof(commonData.DownloadAndroidAppLink)));
            Assert.Equal(commonData.DownloadAndroidAppLink,
                result.GetValueOrDefault(nameof(commonData.DownloadAndroidAppLink)));
        }

        [Fact]
        public async Task
            When_GetKeys_Is_Executed_And_Settings_Namespace_Is_Provided_Expect_That_Settings_Values_Are_Added()
        {
            var @namespace = KeysService.SettingsNamespace;
            var customerId = "test";

            var result = await _keysService.GetKeysAsync(@namespace, customerId);

            Assert.True(result.ContainsKey("ImagesBaseUrl"));
            Assert.Equal(ImagesBaseUrl, result.GetValueOrDefault("ImagesBaseUrl"));
        }
    }
}
