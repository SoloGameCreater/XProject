
using UnityEngine;

/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameModule : MonoBehaviour
{
    private void Start()
    {
        InitBuiltinComponents();
        InitCustomComponents();
    }
}
