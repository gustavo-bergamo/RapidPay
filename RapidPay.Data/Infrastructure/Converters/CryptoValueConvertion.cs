using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using RapidPay.CrossProject.Tools;
using RapidPay.Data.Infrastructure.Tools;

namespace RapidPay.Data.Infrastructure.Converters
{
    internal static class CryptoValueConvertion
    {
        public static PropertyBuilder HasCryptoConversion(this PropertyBuilder<string> propertyBuilder, IConfiguration configuration, bool mask)
        {
            var crypto = new Crypto(configuration);

            var converter = new ValueConverter<string, string>
            (
                v => crypto.Encrypt(v),
                v => mask ? CreditCardMask.Mask(crypto.Decrypt(v)) : crypto.Decrypt(v)
            );

            var comparer = new ValueComparer<string>
            (
                (l, r) => crypto.Encrypt(l) == crypto.Encrypt(r),
                v => v == null ? 0 : crypto.Encrypt(v).GetHashCode(),
                v => crypto.Decrypt(crypto.Encrypt(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("nvarchar(max)");

            return propertyBuilder;
        }
    }
}
