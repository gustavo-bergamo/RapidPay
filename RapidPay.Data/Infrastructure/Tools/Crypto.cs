using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace RapidPay.Data.Infrastructure.Tools;

public class Crypto
{
    private readonly IConfiguration _configuration;

    public Crypto(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Encrypt(string value)
    {
        byte[] encrypted;
        using (AesManaged aes = new AesManaged())
        {
            var key = _configuration["Cryptokey"]!;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(value);
                    encrypted = ms.ToArray();
                }
            }
        }

        // Return encrypted data
        return Convert.ToBase64String(encrypted);


        //byte[] iv = new byte[16];
        //byte[] array;

        //using (Aes aes = Aes.Create())
        //{
        //    var key = _configuration["Cryptokey"]!;
        //    aes.Key = Encoding.UTF8.GetBytes(key);
        //    aes.IV = iv;

        //    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        //        {
        //            using (var streamWriter = new StreamWriter(cryptoStream))
        //            {
        //                streamWriter.Write(value);
        //                array = memoryStream.ToArray();
        //            }
        //        }
        //    }
        //}

        //return Convert.ToBase64String(array);
    }

    public string Decrypt(string value)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(value);

        using (Aes aes = Aes.Create())
        {
            var key = _configuration["Cryptokey"]!;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
    }


}
