using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Vector2 velocity;
    public float angularVelocity;
    public int hp;
    public int animationHash;
    public float animationTime;

    public TimeFrame(Transform t, Rigidbody2D rb, Animator anim, int currentHP)
    {
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;

        if (rb != null)
        {
            velocity = rb.linearVelocity;
            angularVelocity = rb.angularVelocity;
        }
        else
        {
            velocity = Vector2.zero;
            angularVelocity = 0f;
        }

        hp = currentHP;

        if (anim != null)
        {
            var state = anim.GetCurrentAnimatorStateInfo(0);
            animationHash = state.shortNameHash;
            animationTime = state.normalizedTime;
        }
        else
        {
            animationHash = 0;
            animationTime = 0f;
        }
    }
}

public class TimeRecorder : MonoBehaviour
{
    private const int MAX_FRAMES = 120 * 50;
    private readonly List<TimeFrame> history = new List<TimeFrame>(MAX_FRAMES);

    private Rigidbody2D rb;

    public int FramesAvailableToRewind => Mathf.Max(0, history.Count - 1);

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void RecordFrame()
    {
        if (history.Count >= MAX_FRAMES)
        {
            history.RemoveAt(0);
        }

        history.Add(new TimeFrame(transform, rb, null, 0));
    }

    public void JumpBack(float secondsAgo)
    {
        if (history.Count == 0)
        {
            Debug.LogWarning("没有历史记录，无法回溯！");
            return;
        }

        int framesToGoBack = Mathf.RoundToInt(secondsAgo / Time.fixedDeltaTime);
        int targetIndex = history.Count - 1 - framesToGoBack;

        if (targetIndex < 0)
        {
            targetIndex = 0;
        }

        if (targetIndex < history.Count)
        {
            ApplyFrame(history[targetIndex]);

            int removeCount = history.Count - 1 - targetIndex;
            if (removeCount > 0)
            {
                history.RemoveRange(targetIndex + 1, removeCount);
            }
        }
    }

    public bool StepBackOneFrame()
    {
        if (history.Count == 0)
        {
            return false;
        }

        if (history.Count == 1)
        {
            ApplyFrame(history[0]);
            return false;
        }

        history.RemoveAt(history.Count - 1);
        ApplyFrame(history[history.Count - 1]);
        return true;
    }

    public void NotifyResume()
    {
        if (rb != null)
        {
            rb.WakeUp();
        }
    }

    private void ApplyFrame(TimeFrame frame)
    {
        transform.position = frame.position;
        transform.rotation = frame.rotation;
        transform.localScale = frame.scale;

        if (rb != null)
        {
            rb.linearVelocity = frame.velocity;
            rb.angularVelocity = frame.angularVelocity;
            rb.Sleep();
        }
    }
}
