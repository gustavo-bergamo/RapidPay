namespace RapidPay.CrossProject.Tools;

public static class CreditCardMask
{
    public static string Mask(string creditCardNumber)
    {
        return creditCardNumber.Substring(creditCardNumber.Length - 4).PadLeft(creditCardNumber.Length, '*');
    }
}
