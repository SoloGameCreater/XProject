using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;
using System.Collections;
using Framework;

namespace TripleMerge
{
    /// <summary>
    /// 单元格解锁进度视图
    /// </summary>
    public class CellUnlockProgressView : MonoBehaviour
    {
        private const float SPEED = 1.25f;
        private static ObjectPool<CellUnlockProgressView> _pool;
        private const int DEFAULT_POOL_SIZE = 8;
        private const int MAX_POOL_SIZE = 20;

        private TextMeshProUGUI _textProgress;
        private Slider _progressBar;

        public int DisplayProgress { private set; get; }
        private int _realProgress;
        private int _maxProgress;
        // 对象释放调用
        public Action<CellUnlockProgressView> ReleaseAction { private get; set; }
        private Coroutine _progressUpdateTask;
        private static void InitializePool()
        {
            if (_pool == null)
            {
                _pool = new ObjectPool<CellUnlockProgressView>(
                    CreatePooledItem,        // 创建新对象的方法
                    OnTakeFromPool,          // 从池中获取对象时的操作
                    OnReturnedToPool,        // 将对象返回池中时的操作
                    OnDestroyPoolObject,     // 当池已满时销毁对象的操作
                    true,                    // 是否进行集合检查 (防止重复释放)
                    DEFAULT_POOL_SIZE,       // 默认池容量
                    MAX_POOL_SIZE            // 最大池容量
                );
            }
        }
        private static CellUnlockProgressView CreatePooledItem()
        {
            // 注意: 确保 "UICellUnPurifiedProgressBar" Prefab 存在且可访问
            // 如果 Utils.InstantiateUI 有特殊逻辑 (例如异步加载), 可能需要调整
            var insObj = Utils.InstantiateUI("UIMapCellUnlockProgressBar", TripleMergeSystem.Instance.Gameplay.MapUIManager.GetOrCreateTransform("CellUnlockProgressViews"));
            var progressBar = insObj.GetOrAddComponent<CellUnlockProgressView>();
            progressBar.InitializeComponents(); // 首次创建时获取组件
            return progressBar;
        }
        
        private void InitializeComponents()
        {
            _progressBar = transform.Find("Progress").GetComponent<Slider>();
            _textProgress = transform.Find("Progress/TextProgress").GetComponent<TextMeshProUGUI>();
        }

        private static void OnTakeFromPool(CellUnlockProgressView item)
        {
            item.transform.SetParent(TripleMergeSystem.Instance.Gameplay.MapUIManager.GetOrCreateTransform("CellUnlockProgressViews"));
            item.gameObject.SetActive(true);
            // 将 Release Action 关联到 Pool 的 Release 方法
            item.ReleaseAction = (progressBar) => _pool.Release(progressBar);
        }
        private static void OnReturnedToPool(CellUnlockProgressView item)
        {
            // 停止可能正在运行的协程
            if (item._progressUpdateTask != null)
            {
                item.StopCoroutine(item._progressUpdateTask);
                item._progressUpdateTask = null;
            }
            item.gameObject.SetActive(false);
        }

        private static void OnDestroyPoolObject(CellUnlockProgressView item)
        {
            Destroy(item.gameObject);
        }
        public void SetProgress(int currentProgress)
        {
            // 确保 _progressBar 和 _textProgress 已初始化
            if (_progressBar == null || _textProgress == null)
            {
                InitializeComponents(); // 添加一层保护，尽管 Create 时会调用
            }

            DisplayProgress = _realProgress = currentProgress;
            _progressBar.value = (_maxProgress > 0) ? (currentProgress * 1.0f / _maxProgress) : 0f;
            _textProgress.text = $"{DisplayProgress}/{_maxProgress}";
            DebugUtil.Log($"SetProgress: {currentProgress}/{_maxProgress}");
            // 如果初始进度已经是最大进度，可能不需要显示动画，直接隐藏或回收
            if(currentProgress >= _maxProgress)
            {
                 // gameObject.SetActive(false); // 或者直接回收
                 ReleaseAction?.Invoke(this);
            }
        }
        public void UpdateProgress(int progress, Action onUpdated = null)
        {
            _realProgress = progress;

            if (_progressUpdateTask != null)
            {
                StopCoroutine(_progressUpdateTask);
            }

            _progressUpdateTask = StartCoroutine(UpdateCoroutine(onUpdated)); // 传递 onUpdated 回调
        }
        // 将 Update 逻辑移到单独的 IEnumerator 方法中
        private IEnumerator UpdateCoroutine(Action onUpdated)
        {
            while (true)
            {
                yield return null;

                // 确保 maxProgress > 0 避免除零错误
                 if (_maxProgress <= 0) {
                     Debug.LogWarning("MaxProgress is zero or negative, cannot update progress bar.");
                     break; // 退出协程
                 }


                var targetProgress = _realProgress * 1.0f / _maxProgress;
                // 使用 Mathf.MoveTowards 平滑过渡
                var nextValue = Mathf.MoveTowards(_progressBar.value, targetProgress, SPEED * Time.deltaTime); // Removed division by _maxProgress as it might make speed inconsistent depending on max value

                _progressBar.value = nextValue;
                DisplayProgress = Mathf.RoundToInt(_maxProgress * nextValue);
                _textProgress.text = $"{DisplayProgress}/{_maxProgress}";

                // 使用 Mathf.Approximately 比较浮点数
                if (Mathf.Approximately(_progressBar.value, targetProgress))
                {
                    // 确保最终值精确
                    _progressBar.value = targetProgress;
                    DisplayProgress = _realProgress;
                    _textProgress.text = $"{DisplayProgress}/{_maxProgress}";

                    yield return new WaitForSeconds(0.5f);

                    // gameObject.SetActive(false); // 不再手动设置 Inactive
                    onUpdated?.Invoke();
                    ReleaseAction?.Invoke(this); // 使用 Release Action 回收

                    _progressUpdateTask = null; // 清理 Coroutine 引用
                    yield break; // 退出协程
                }
            }
             _progressUpdateTask = null; // 确保在异常退出时也清理
        }
        public static CellUnlockProgressView Get(int currentProgress, int maxProgress)
        {
            InitializePool(); // 确保 Pool 已初始化

            var item = _pool.Get();
            item._maxProgress = maxProgress;
            item.SetProgress(currentProgress); // 设置初始状态
            return item;
        }
        public static void Recycle(CellUnlockProgressView item)
        {
             if (item != null && item.ReleaseAction != null)
             {
                 item.ReleaseAction(item);
             }
             else if (item != null) // 如果 ReleaseAction 未设置 (理论上不应发生)
             {
                  Debug.LogWarning("Attempting to recycle an item without a ReleaseAction. Destroying instead.");
                  Destroy(item.gameObject);
             }
        }
    }
}
