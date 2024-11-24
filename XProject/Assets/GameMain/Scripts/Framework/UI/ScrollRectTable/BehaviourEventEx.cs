using UnityEngine;

public class BehaviourEventEx : BehaviourEvent
{
    public float moveEpsilon = 0.01f;
    bool isMoving = false;
    Vector3 lastPos;

    protected override void Start()
    {
        base.Start();
        lastPos = transform.localPosition;
    }

    protected virtual void Update()
    {
        Notify("Update");

        Vector3 pos = transform.localPosition;

        if (Vector3.Distance(pos, lastPos) > moveEpsilon)
        {
            if (!isMoving)
            {
                Notify("OnMoveBegin");
                isMoving = true;
            }

            Notify("OnMove");
            lastPos = pos;
        }
        else
        {
            if (isMoving)
            {
                Notify("OnMoveEnd");
                isMoving = false;
            }
        }
    }
}
