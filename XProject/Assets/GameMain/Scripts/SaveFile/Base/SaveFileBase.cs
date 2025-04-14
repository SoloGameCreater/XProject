using System;
using System.Reflection;
using Framework;

namespace SaveFile
{
    [Serializable]
    public abstract class SaveFileBase
    {
        public void Clear()
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var type = pi.PropertyType;
                if (pi.PropertyType == typeof(string))
                {
                    pi.SetValue(this, string.Empty, null);
                }
                else if (pi.PropertyType == typeof(int))
                {
                    pi.SetValue(this, (int)0, null);
                }
                else if (pi.PropertyType == typeof(long))
                {
                    pi.SetValue(this, (long)0, null);
                }
                else if (pi.PropertyType == typeof(uint))
                {
                    pi.SetValue(this, (uint)0, null);
                }
                else if (pi.PropertyType == typeof(ulong))
                {
                    pi.SetValue(this, (ulong)0, null);
                }
                else if (pi.PropertyType == typeof(float))
                {
                    pi.SetValue(this, (float)0, null);
                }
                else if (pi.PropertyType == typeof(double))
                {
                    pi.SetValue(this, (double)0, null);
                }
                else if (pi.PropertyType == typeof(bool))
                {
                    pi.SetValue(this, (bool)false, null);
                }
                else if (type.IsSubclassOf(typeof(SaveFileBase)))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SaveFileList<>))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SaveFileDictionary<,>))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
                else
                {
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    DebugUtil.LogError($"存档数据格式错误 类型 {type.GetGenericTypeDefinition()}");
                    #endif
                }
            }
        }
    }
}
