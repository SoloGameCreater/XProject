using System;
using System.Collections;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance;

    [Header("回溯设置")]
    [Tooltip("每次按键回溯的时间长度（秒）")]
    public float rewindSeconds = 10f;

    [Tooltip("回溯动画持续时间（秒），设为 0 表示瞬间完成")]
    public float visualRewindDuration = 1.0f;

    [Tooltip("最大回溯次数")]
    public int maxRewinds = 6;

    [Header("调试 UI")]
    public bool showDebugUI = true;

    public enum GameState
    {
        Playing,
        Rewinding,
        Paused
    }

    public GameState CurrentState { get; private set; } = GameState.Playing;
    public int RewindsLeft { get; private set; }

    private Coroutine rewindRoutine;
    private TimeRecorder[] lastRewindRecorders = Array.Empty<TimeRecorder>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        RewindsLeft = maxRewinds;
    }

    private void FixedUpdate()
    {
        if (CurrentState == GameState.Playing)
        {
            foreach (var recorder in FindObjectsOfType<TimeRecorder>())
            {
                recorder.RecordFrame();
            }
        }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    TryStartRewind();
                }
                break;

            case GameState.Rewinding:
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    ResumeGame();
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    TryStartRewind();
                }
                break;
        }
    }

    private void TryStartRewind()
    {
        if (rewindRoutine != null)
        {
            return;
        }

        if (RewindsLeft <= 0)
        {
            Debug.Log("回溯次数已用尽！");
            return;
        }

        rewindRoutine = StartCoroutine(ProcessRewind());
    }

    private IEnumerator ProcessRewind()
    {
        CurrentState = GameState.Rewinding;

        var recorders = FindObjectsOfType<TimeRecorder>();
        if (recorders.Length == 0)
        {
            Debug.LogWarning("场景中没有 TimeRecorder，无法回溯。");
            CurrentState = GameState.Playing;
            rewindRoutine = null;
            lastRewindRecorders = Array.Empty<TimeRecorder>();
            yield break;
        }

        int requestedFrames = Mathf.RoundToInt(rewindSeconds / Time.fixedDeltaTime);
        int framesToRewind = requestedFrames;
        foreach (var recorder in recorders)
        {
            framesToRewind = Mathf.Min(framesToRewind, recorder.FramesAvailableToRewind);
        }

        if (framesToRewind <= 0)
        {
            Debug.LogWarning("历史记录不足，尚无法回溯");
            CurrentState = GameState.Playing;
            rewindRoutine = null;
            lastRewindRecorders = Array.Empty<TimeRecorder>();
            yield break;
        }

        RewindsLeft--;
        Time.timeScale = 0f;

        float stepDelay = visualRewindDuration <= 0f
            ? 0f
            : visualRewindDuration / framesToRewind;

        for (int i = 0; i < framesToRewind; i++)
        {
            foreach (var recorder in recorders)
            {
                recorder.StepBackOneFrame();
            }

            if (stepDelay > 0f)
            {
                yield return new WaitForSecondsRealtime(stepDelay);
            }
            else
            {
                yield return null;
            }
        }

        lastRewindRecorders = recorders;
        CurrentState = GameState.Paused;
        Debug.Log("回溯完成，按 Enter 继续，或按 R 继续倒放");
        rewindRoutine = null;
    }

    private void ResumeGame()
    {
        foreach (var recorder in lastRewindRecorders)
        {
            if (recorder != null)
            {
                recorder.NotifyResume();
            }
        }

        lastRewindRecorders = Array.Empty<TimeRecorder>();
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        Debug.Log(">>> 游戏继续");
    }

    private void OnGUI()
    {
        if (!showDebugUI) return;

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        GUILayout.BeginArea(new Rect(20, 20, 420, 220));

        GUILayout.Label($"剩余回溯次数: {RewindsLeft} / {maxRewinds}", style);

        if (CurrentState == GameState.Rewinding)
        {
            style.normal.textColor = Color.yellow;
            GUILayout.Label("<< 正在倒放 <<", style);
        }
        else if (CurrentState == GameState.Paused)
        {
            style.normal.textColor = Color.cyan;
            GUILayout.Label("已暂停", style);
            GUILayout.Label("[Enter] 继续游戏", style);
            GUILayout.Label("[R] 继续回溯", style);
        }

        GUILayout.EndArea();
    }
}
