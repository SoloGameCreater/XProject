using System.ComponentModel;
using Framework;
using SaveFile;
using SaveFile.TripleMerge;
using UnityEngine;

public partial class DebugOptions
{
    // Category ：大类型名称
    // DisplayName ：小类型名称
    [Category("通用")]
    [DisplayName("清空存档")]
    public void ClearSaveFile()
    {
        SaveFileManager.Instance.GetSaveFile<SaveFileTripleMerge>().Clear();
        DebugUtil.LogWarning("清除三消数据");
        QuitApp();
    }
    [Category("其他")]
    [DisplayName("日志测试")]
    public void LogTest()
    {
        DebugUtil.LogWarning("这是一条日志");
    }
}