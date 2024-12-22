
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.UI;
using Framework;
using Cosmos;

public partial class UIRoot : MonoSingleton<UIRoot>
{
    // 记录当前界面最高 到了多少层
    private int _canvasOrder = 10;

    // 尝试修复花屏的问题
    private bool _isDirty = false;
    private Font _dirtyFont = null;
    private Canvas _canvas;

    public Canvas mRootCanvas;

    // 所有UI的根节点
    public GameObject mRoot;
    public GameObject mWorldUIRoot;
    public GameObject mGuideRoot;

    public Camera mUICamera;

    public GameObject worldRoot;

    // 管理DEBUG信息的参数
    public bool CloseDebug;
    public bool CloseFPS;

    // 缓存UI摄像机的6个裁剪面
    public Plane[] CameraPlanes { private set; get; }
    public int CanvasOrder => _canvasOrder;

    public float RootWidth;
    public float RootHeight;

    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        if (mUICamera != null)
        {
            CameraPlanes = GeometryUtility.CalculateFrustumPlanes(mUICamera);
        }

        Font.textureRebuilt += delegate(Font font1)
        {
            _isDirty = true;
            _dirtyFont = font1;
        };

        _canvas = mRoot.GetComponent<Canvas>();
        _touchBlock = transform.Find("Canvas/TouchBlock").gameObject;
    }


    private void Start()
    {
        SpriteAtlasManager.atlasRequested += OnLoadAtlas;

        var rootRect = this.mRoot.GetComponent<RectTransform>();
        RootWidth = rootRect.rect.width;
        RootHeight = rootRect.rect.height;
        
        CommonUtils.NotchAdapt(mRootCanvas.transform.Find("EnvGroup").GetComponent<RectTransform>());
    }

    private void LateUpdate()
    {
        if (_isDirty)
        {
            _isDirty = false;
            foreach (Text text in FindObjectsOfType<Text>())
            {
                if (text.font == _dirtyFont)
                {
                    text.FontTextureChanged();
                }
            }

            _dirtyFont = null;
        }
    }

    private void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= OnLoadAtlas;
    }

    private void OnLoadAtlas(string atlasName, Action<SpriteAtlas> onAtlasLoad)
    {
        //过滤掉所有的sd
        //sd没有采用variant模式，那么图集回调就有2次，忽略掉sd的回调请求。在LoadSpriteAtlas中会区分到底是hd还是sd
        {
            string tempStr = atlasName.ToLower();
            int lastIndex = tempStr.LastIndexOf(".");
            if (lastIndex > 0)
            {
                tempStr = tempStr.Substring(lastIndex);
                if (tempStr.Equals(".sd"))
                {
                    Debug.Log(string.Format("忽略图集 ： {0}", atlasName));
                    return;
                }
            }
        }

        try
        {
            var sa = ResourcesManager.Instance.LoadSpriteAtlasVariant(atlasName);
            onAtlasLoad(sa);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void EnableTouch(bool b)
    {
        var cg = mRootCanvas.GetComponent<CanvasGroup>();
        cg.interactable = b;
        cg.blocksRaycasts = b;
    }

    #region mask from ck3

    // Loading spine动画
    public GameObject mUILoadingRoot;
    public GameObject mDebugConsole;
    private RectTransform m_Canvas;
    private GameObject _touchBlock;

    // Mask Loading动画
    private GameObject _maskLoading;
    private Animator _maskLoadingAnimator;
    private GameObject _mask;
    private Image _maskImage;

    public void AddTouchBlock()
    {
        CancelInvoke(nameof(RemoveTouchBlock));
        _touchBlock.SetActive(true);
    }

    public void RemoveTouchBlock()
    {
        _touchBlock.SetActive(false);
    }

    #endregion

    public void BlockTouchWithTime(float time)
    {
        AddTouchBlock();
        Invoke(nameof(RemoveTouchBlock), time);
    }
}