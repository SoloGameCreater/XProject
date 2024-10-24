
using StarForce;
using UnityEngine;

/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameModule : MonoBehaviour
{
    public static BuiltinDataComponent BuiltinData
    {
        get;
        private set;
    }

    public static HPBarComponent HPBar
    {
        get;
        private set;
    }

    private static void InitCustomComponents()
    {
        BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
        HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
    }
}
