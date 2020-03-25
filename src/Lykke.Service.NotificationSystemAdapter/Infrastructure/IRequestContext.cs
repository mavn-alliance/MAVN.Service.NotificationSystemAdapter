namespace Lykke.Service.NotificationSystemAdapter.Infrastructure
{
    public interface IRequestContext
    {
        void ResponseUnauthorized(string customMessage = null);
        string ApiKey { get; }
    }
}
