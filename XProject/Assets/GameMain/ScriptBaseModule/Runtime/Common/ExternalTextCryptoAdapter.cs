namespace BaseModule
{
    /// <summary>
    /// 启动模块的文本加解密适配层，避免依赖其他程序集的实现。
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
