namespace RapidPay.Domain.Authentication.Services
{
    public interface IUserSession
    {
        int UserId { get; }
        string IPAddress { get; }

        void SetUserId(int userId);
        void SetIPAddress(string ip);
    }
}