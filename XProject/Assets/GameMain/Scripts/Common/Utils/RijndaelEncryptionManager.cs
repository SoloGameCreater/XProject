using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Rijndael
{
    /// <summary>
    /// AES 加密解密管理器
    /// </summary>
    public class RijndaelEncryptionManager : Manager<RijndaelEncryptionManager>
    {
        #region 私有字段
        // 注意：这里的密钥和IV应从安全存储中获取，而不是硬编码
        // 在实际项目中，建议使用密钥管理系统或至少从配置文件读取
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

        // 使用UTF-8编码确保跨平台一致性
        private static readonly Encoding _encoding = Encoding.UTF8;
        #endregion

        #region 公共方法
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <returns>加密后的字节数组</returns>
        /// <exception cref="ArgumentNullException">明文为空时抛出</exception>
        /// <exception cref="CryptographicException">加密过程出错时抛出</exception>
        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            try
            {
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt, _encoding))
                {
                    swEncrypt.Write(plainText);
                }

                return msEncrypt.ToArray();
            }
            catch (Exception ex) when (ex is CryptographicException || ex is ArgumentException)
            {
                throw new CryptographicException("加密过程中发生错误", ex);
            }
        }

        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="cipherText">要解密的密文字节数组</param>
        /// <returns>解密后的明文字符串</returns>
        /// <exception cref="ArgumentNullException">密文为空时抛出</exception>
        /// <exception cref="CryptographicException">解密过程出错或密文被篡改时抛出</exception>
        public string Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            try
            {
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using var msDecrypt = new MemoryStream(cipherText);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt, _encoding);
                
                return srDecrypt.ReadToEnd();
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("解密失败，密文可能已被篡改", ex);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("解密过程中发生错误", ex);
            }
        }
        #endregion
    }

    /// <summary>
    /// 第三方文本加解密适配层，避免业务侧直接依赖具体 SDK 命名。
    /// </summary>
    public static class ExternalTextCryptoAdapter
    {
        public static string DecryptText(string text)
        {
#if ENCRY_IOS && !UNITY_EDITOR
            return DragonU3DSDK.Asset.EncryptDecrypt.Decrypt(text);
#else
            return text;
#endif
        }

        public static string EncryptText(string text)
        {
#if ENCRY_IOS && !UNITY_EDITOR
            return DragonU3DSDK.Asset.EncryptDecrypt.Encrypt(text);
#else
            return text;
#endif
        }
    }
}
