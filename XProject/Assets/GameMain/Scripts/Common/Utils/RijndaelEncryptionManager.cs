using System;
using System.Text;

namespace Rijndael
{
    /// <summary>
    /// 存档加解密管理器（当前为直通实现，仅保留接口）
    /// </summary>
    public class RijndaelEncryptionManager : Manager<RijndaelEncryptionManager>
    {
        // 保留 UTF-8 编码，保证存档字符串在各平台可逆。
        private static readonly Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 加密接口（当前为直通实现）
        /// </summary>
        /// <param name="plainText">输入明文</param>
        /// <returns>UTF-8 字节数据</returns>
        public byte[] Encrypt(string plainText)
        {
            if (plainText == null)
                throw new ArgumentNullException(nameof(plainText));

            return _encoding.GetBytes(plainText);
        }

        /// <summary>
        /// 解密接口（当前为直通实现）
        /// </summary>
        /// <param name="cipherText">输入字节数据</param>
        /// <returns>UTF-8 明文字符串</returns>
        public string Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            return _encoding.GetString(cipherText);
        }
    }

    /// <summary>
    /// 文本加解密适配层（当前为直通实现，仅保留接口）。
    /// </summary>
    public static class ExternalTextCryptoAdapter
    {
        public static string DecryptText(string text)
        {
            return text;
        }

        public static string EncryptText(string text)
        {
            return text;
        }
    }
}
