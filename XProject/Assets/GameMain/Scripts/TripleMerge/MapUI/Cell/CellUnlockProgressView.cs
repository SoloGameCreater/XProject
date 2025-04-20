using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TripleMerge
{
    public class CellUnlockProgressView : MonoBehaviour
    {
        private const int POOL_LIMIT_SINGLE_ELEMENT_TYPE = 50;

        private const float SPEED = 1.35f;

        private static readonly Stack<CellUnlockProgressView> _pool = new();

        private static Transform _poolRoot;

        private Slider _progressBar;

        private TextMeshProUGUI _textProgress;

        public int DisplayProgress { private set; get; }
        private int _realProgress;
        private int _maxProgress;

        private Coroutine _progressUpdateTask;

        private void Initialize(int maxProgress)
        {
            _progressBar = transform.Find("Progress").GetComponent<Slider>();

            _textProgress = transform.Find("Progress/TextProgress").GetComponent<TextMeshProUGUI>();

            _maxProgress = maxProgress;
        }

        public void SetProgress(int progress)
        {
            DisplayProgress = _realProgress = progress;

            _textProgress.text = $"{DisplayProgress}/{_maxProgress}";
        }

        public void UpdateProgress(int progress, Action onUpdated = null)
        {
            _realProgress = progress;
            DebugUtil.Log($"UpdateProgress: {progress}/{_maxProgress}");
            gameObject.SetActive(true);

            if (_progressUpdateTask != null)
            {
                StopCoroutine(_progressUpdateTask);
            }

            _progressUpdateTask = StartCoroutine(Update());

            IEnumerator Update()
            {
                while (true)
                {
                    yield return null;

                    var nextProgress = _progressBar.value + SPEED * Time.deltaTime;

                    var targetProgress = _realProgress * 1.0f / _maxProgress;

                    if (nextProgress > targetProgress)
                    {
                        nextProgress = targetProgress;
                    }

                    DisplayProgress = Mathf.RoundToInt(_maxProgress * nextProgress);

                    _progressBar.value = nextProgress;
                    _textProgress.text = $"{DisplayProgress}/{_maxProgress}";

                    if (DisplayProgress == _realProgress)
                    {
                        yield return new WaitForSeconds(0.5f);

                        gameObject.SetActive(false);

                        onUpdated?.Invoke();

                        break;
                    }
                }
            }
        }

        public static CellUnlockProgressView Get(int currentProgress, int maxProgress)
        {
            if (_poolRoot == null)
            {
                var root = new GameObject("@CellUnlockProgressViewPool")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };

                _poolRoot = root.transform;

                DontDestroyOnLoad(root);
            }


            if (_pool.TryPop(out var item))
            {
                item.transform.SetParent(TripleMergeSystem.Instance.Gameplay.MapUIManager.GetOrCreateTransform("CellUnlockProgressViews"));
                item.DisplayProgress = currentProgress;
                item._realProgress = currentProgress;
                item._maxProgress = maxProgress;
                item._progressBar.value = currentProgress * 1.0f / maxProgress;
                return item;
            }

            var insObj = Utils.InstantiateUI("UIMapCellUnlockProgressBar", TripleMergeSystem.Instance.Gameplay.MapUIManager.GetOrCreateTransform("CellUnlockProgressViews"));
            var progressBar = insObj.GetOrAddComponent<CellUnlockProgressView>();
            progressBar.Initialize(maxProgress);
            progressBar.DisplayProgress = currentProgress;
            progressBar._realProgress = currentProgress;
            progressBar._maxProgress = maxProgress;
            progressBar._progressBar.value = currentProgress * 1.0f / maxProgress;
            return progressBar;
        }

        public static void Recycle(CellUnlockProgressView item)
        {
            if (_poolRoot == null)
            {
                return;
            }

            if (item == null || item.gameObject == null)
            {
                return;
            }

            if (_pool.Contains(item))
            {
                Debug.Log($"回收了重复的三合地块解锁进度条,已终止本次回收操作!!");
                return;
            }

            if (_pool.Count >= POOL_LIMIT_SINGLE_ELEMENT_TYPE)
            {
                Destroy(item.gameObject);

                return;
            }

            _pool.Push(item);

            item.transform.SetParent(_poolRoot, false);
            item.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_progressUpdateTask != null) StopCoroutine(_progressUpdateTask);
            _progressUpdateTask = null;
        }
    }
}