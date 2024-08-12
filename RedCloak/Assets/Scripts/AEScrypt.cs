using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AEScrypt
{
    private static readonly string KEY = "DreamWeaverProject".Substring(0, 128 / 8);
    public static string Encrypt(string text)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);

        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(textBytes, 0, textBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encrypted = memoryStream.ToArray();
        string result = Convert.ToBase64String(encrypted);
        
        cryptoStream.Close();
        memoryStream.Close();

        return result;
    }

    public static string Decrypt(string text)
    {
        byte[] textBytes = Convert.FromBase64String(text);

        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(textBytes);
        ICryptoTransform decryptor = rijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] decrypted = new byte[textBytes.Length];
        int count = cryptoStream.Read(decrypted, 0, decrypted.Length);
        string result = Encoding.UTF8.GetString(decrypted, 0, count);
        
        cryptoStream.Close();
        memoryStream.Close();

        return result;
    }
}
