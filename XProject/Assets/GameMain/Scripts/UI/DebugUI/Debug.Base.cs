using System.ComponentModel;
using Framework;
using SaveFile;
using SaveFile.TripleMerge;
using UnityEngine;

public partial class DebugOptions
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    // Category ：大类型名称
    // DisplayName ：小类型名称
    [Category("通用")]
    [DisplayName("清空存档")]
    public void ClearSaveFile()
    {
        SaveFileManager.Instance.GetSaveFile<SaveFileTripleMerge>().Clear();
        SaveFileManager.Instance.TryAutoSave("Debug清空三消存档", true);
        DebugUtil.LogWarning("清除三消数据");
        QuitApp();
    }

    [Category("通用")]
    [DisplayName("手动存档")]
    public void ManualSaveFile()
    {
        var result = SaveFileManager.Instance.ManualSave("Debug面板");
        DebugUtil.LogWarning(result ? "手动存档成功" : "手动存档失败");
    }

    [Category("其他")]
    [DisplayName("日志测试")]
    public void LogTest()
    {
        DebugUtil.LogWarning("这是一条日志");
    }
#endif
}
