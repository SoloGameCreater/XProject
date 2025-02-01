using UnityEngine;

public class SceneRoot : Manager<SceneRoot>
{
    public Camera mSceneCamera;
    private void Awake()
    {
        mSceneCamera.useOcclusionCulling = false;
    }
}
