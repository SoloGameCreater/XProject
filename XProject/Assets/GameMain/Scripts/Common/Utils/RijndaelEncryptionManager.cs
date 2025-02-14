using System;
using System.IO;
using System.Security.Cryptography;

namespace Rijndael
{
    public class RijndaelEncryptionManager : Manager<RijndaelEncryptionManager>
    {
        private readonly byte[] _key = new byte[]
        {
            0x99, 0x90, 0xFA, 0x30, 0x26, 0x7C, 0x2C, 0xB3,
            0xD9, 0x9A, 0x50, 0x66, 0xE2, 0xBB, 0x14, 0x99,
            0x72, 0x04, 0xA9, 0x35, 0x35, 0xDB, 0x4D, 0x40,
            0xAB, 0x94, 0x29, 0x5A, 0x75, 0xA0, 0x0B, 0xCF,
        };

        private readonly byte[] _iv = new byte[]
        {
            0x18, 0xBA, 0x3F, 0x25, 0xF5, 0x83, 0x38, 0xE5,
            0x1A, 0x7D, 0xDE, 0xD0, 0x92, 0xC5, 0x2F, 0xD2
        };

        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            using RijndaelManaged rijAlg = new RijndaelManaged();
            rijAlg.Key = _key;
            rijAlg.IV = _iv;

            var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return msEncrypt.ToArray();
        }

        public string Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            using var rijAlg = new RijndaelManaged();
            rijAlg.Key = _key;
            rijAlg.IV = _iv;

            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
    }
}