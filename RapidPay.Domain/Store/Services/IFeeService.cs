namespace RapidPay.Domain.Store.Services;

public interface IFeeService
{
    Task CalculateFeeAsync();
    decimal GetFee();
}