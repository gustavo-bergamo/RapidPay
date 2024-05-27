using RapidPay.Domain.Infrastructure.Attributes;

namespace RapidPay.Domain.Authentication.Services;

[AddDependency]
public class UserSession : IUserSession
{
    public int UserId { get; private set; }
    public string IPAddress { get; private set; }

    public void SetIPAddress(string ipAddress)
    {
        IPAddress = ipAddress;
    }

    public void SetUserId(int userId)
    {
        UserId = userId;
    }
}
