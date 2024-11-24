
using Framework;
using UnityEngine;


public class MyMain : Main
{
    public static MyGame myGame { get; private set; }
    public static bool InitFinished = false;

    protected override Game createGame()
    {
        initComponents();
        myGame = new MyGame();
        return myGame;
    }

    private void initComponents()
    {
        // gameObject.AddComponent<QualityMgr>();
        // gameObject.AddComponent<SDKEventsHandler>();
        // gameObject.AddComponent<OrientationHandler>();
        // gameObject.AddComponent<ApplicationEventHandler>();
        // gameObject.AddComponent<UnityMessageHandler>();
        if (Debug.isDebugBuild)
        {
            //gameObject.AddComponent<CanvasRebuiltMonitor>();
        }
    }
}
