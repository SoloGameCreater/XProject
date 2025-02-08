using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static SomeWhere.TableView;

namespace SomeWhere
{
    public partial class Internal_TableView
    {
        enum Status
        {
            None,
            AutoMoving,
        }

        public TableView _proxy;

        public const string DEFAULT_IDENTIFIER = "default.identifier";

        /// <summary>
        /// 返回行数
        /// </summary>
        public delegate int DelegateNumberOfCells(Internal_TableView tbView);

        /// <summary>
        /// 返回第i行cell的高度（垂直），或宽度（水平）
        /// </summary>
        public delegate float DelegateSizeOfIndex(Internal_TableView tbView, int index);

        /// <summary>
        /// 返回第i行cell的RectTransform
        /// </summary>
        public delegate TableViewCell DelegateTransformOfIndex(Internal_TableView tbView, int index);

        /// <summary>
        /// 返回第i行cell的标识符
        /// </summary>
        public delegate string DelegateIdentifierOfIndex(Internal_TableView tbView, int index);

        /// <summary>
        /// 初始化cell的数据
        /// </summary>
        public delegate void DelegateOnInitCell(TableViewCell cell, int index);

        /// <summary>
        /// 滚动时回调
        /// </summary>
        public delegate void DelegateOnCellMove(TableViewCell cell, int index);

        private DelegateNumberOfCells _delegateNumberOfCells = null;
        private DelegateSizeOfIndex _delegateSizeOfIndex = null;
        private DelegateTransformOfIndex _delegateTransformOfIndex = null;
        private DelegateIdentifierOfIndex _delegateIdentifierOfIndex = null;
        private DelegateOnInitCell _delegateOnInitCell = null;
        private DelegateOnCellMove _delegateOnCellMove = null;

        private int _extraBuffer = 2;
        private Rect[] _rects;
        private Dictionary<string, TableViewGroup> _groupDict = new Dictionary<string, TableViewGroup>();
        private int _numberOfCells = 0;
        private int _minVisibleIndex = 0;
        private int _maxVisibleIndex = 0;
        private bool _centralized = false;
        private TableViewCell[] _cellsList;

        private Status _status;

        public Internal_TableView(TableView monoProxy, DelegateNumberOfCells delegateNumberOfCells, DelegateSizeOfIndex delegateSizeOfIndex, DelegateTransformOfIndex delegateTransformOfIndex, DelegateOnInitCell delegateOnInitCell, DelegateIdentifierOfIndex delegateIdentifierOfIndex, DelegateOnCellMove delegateOnCellMove, bool centralized)
        {
            _proxy = monoProxy;
            _proxy.OnDestroyDelegate = OnRelease;
            _proxy.onValueChanged.AddListener(OnMove);
            _proxy.OnBeginDragDelegate = OnBeginDrag;
            _proxy.OnEndDragDelegate = OnEndDrag;

            _delegateNumberOfCells = delegateNumberOfCells;
            _delegateSizeOfIndex = delegateSizeOfIndex;
            _delegateTransformOfIndex = delegateTransformOfIndex;
            _delegateIdentifierOfIndex = delegateIdentifierOfIndex;
            _delegateOnInitCell = delegateOnInitCell;
            _delegateOnCellMove = delegateOnCellMove;

            _status = Status.None;
            _centralized = centralized;
            _cellsList = new TableViewCell[delegateNumberOfCells(this)];
        }

        protected void OnRelease()
        {
            _delegateNumberOfCells = null;
            _delegateOnInitCell = null;
            _delegateSizeOfIndex = null;
            _delegateTransformOfIndex = null;
            _delegateIdentifierOfIndex = null;
        }

        bool IsIdentifierExist(string identifier)
        {
            return _groupDict.ContainsKey(identifier + "_v");
        }

        void OnMove(Vector2 value)
        {
            if (_numberOfCells == 0) return;

            UpdateMinMaxVisibleIndex();
            updateCellArc();
            updateCellIndex();

            onCellMove();
        }

        private void onCellMove()
        {
            for (int i = _minVisibleIndex; i <= _maxVisibleIndex; i++)
            {
                var cell = GetCell(i);
                if (cell != null)
                {
                    _delegateOnCellMove?.Invoke(cell, i);
                }
            }
        }

        private void addCellAtIndex(int index)
        {
            if (index >= _numberOfCells || index < 0) return;
            var identifier = getIdentifierForIndex(index);

            if (_groupDict[identifier]._visibilityCellQueueDic.ContainsKey(index) == false)
            {
                var cell = _delegateTransformOfIndex(this, index);
                _cellsList[index] = cell;
                initCellTransformSettings(cell, index);
                _delegateOnInitCell(cell, index);

                _groupDict[identifier]._visibilityCellQueueDic.Add(index, cell);

                cell.RectTrans.gameObject.SetActive(true);
            }
        }

        private void updateCellArc()
        {
            if (_proxy.ArcCell)
            {
                for (int index = _minVisibleIndex; index <= _maxVisibleIndex; index++)
                {
                    var cell = _cellsList[index];
                    if (cell == null) continue;

                    var rect = _rects[index];

                    var newX = rect.x + rect.width / 2f;
                    var newY = rect.y;
                    var cellWorldPos = _proxy.content.TransformPoint(new Vector3(newX, newY, 0));
                    var radius = (_proxy.transform as RectTransform).rect.width * _proxy.ArcRadiusScale;
                    var viewCenterWorldPos = _proxy.transform.position;
                    var deltaX = viewCenterWorldPos.x - cellWorldPos.x;
                    deltaX = deltaX * UIRoot.Instance.mRootCanvas.referencePixelsPerUnit;
                    var sign = deltaX > 0 ? 1 : -1;
                    deltaX = Mathf.Abs(deltaX);
                    if (deltaX < radius)
                    {
                        var deltaY = radius - Mathf.Sqrt(radius * radius - deltaX * deltaX);
                        newY = rect.y - deltaY;
                    }
                    else
                    {
                        newY = rect.y - radius;
                    }

                    var angle = Mathf.Asin(deltaX / radius) / Mathf.PI * 180 * sign;
                    cell.RectTrans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    cell.RectTrans.anchoredPosition = new Vector2(newX, newY);
                }
            }
        }

        private void updateCellIndex()
        {
            return;

            for (var index = _cellsList.Length - 1; index >= 0; index--)
            {
                var cell = _cellsList[index];
                if (cell != null)
                {
                    var siblingIndex = _numberOfCells - 1 - index;
                    cell.RectTrans.SetSiblingIndex(siblingIndex);
                }
            }
        }

        private void initCellTransformSettings(TableViewCell cell, int index)
        {
            var rect = _rects[index];

            //trans.anchorMin = new Vector2(0.5f, 0.5f);
            //trans.anchorMax = new Vector2(0.5f, 0.5f);
            cell.RectTrans.SetParent(_proxy.content);
            //trans.anchoredPosition = new Vector2(x, y);
            cell.RectTrans.localScale = Vector3.one;
            cell.RectTrans.SetLocalPositionZ(0);

            if (_proxy.vertical)
            {
                cell.RectTrans.anchoredPosition = new Vector2(rect.x, rect.y - rect.height / 2f);
                cell.RectTrans.anchorMin = new Vector2(0f, 1f);
                cell.RectTrans.anchorMax = new Vector2(1f, 1f);
                cell.RectTrans.pivot = new Vector2(0.5f, 0.5f);
                var v = cell.RectTrans.offsetMin;
                v.x = 0;
                cell.RectTrans.offsetMin = v;
                v = cell.RectTrans.offsetMax;
                v.x = 0;
                cell.RectTrans.offsetMax = v;
            }
            else
            {
                var newX = rect.x + rect.width / 2f;
                var newY = rect.y;
                cell.RectTrans.anchoredPosition = new Vector2(newX, newY);
                cell.RectTrans.anchorMin = new Vector2(0f, 0.5f);
                cell.RectTrans.anchorMax = new Vector2(0f, 0.5f);
                cell.RectTrans.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        private void RemoveCellAtIndex(int index)
        {
            var identifier = getIdentifierForIndex(index);
            if (_groupDict.ContainsKey(identifier))
            {
                if (_groupDict[identifier]._visibilityCellQueueDic.ContainsKey(index) == true)
                {
                    var cell = _groupDict[identifier]._visibilityCellQueueDic[index];
                    _groupDict[identifier]._invisibilityCellQueue.Add(cell);
                    _groupDict[identifier]._visibilityCellQueueDic.Remove(index);
                    cell.RectTrans.gameObject.SetActive(false);
                }
            }
        }

        private int IndexAtOffset(float offset)
        {
            var minIndex = 0;
            var maxIndex = _numberOfCells - 1;
            maxIndex = maxIndex < 0 ? 0 : maxIndex;
            var index = (maxIndex + minIndex) / 2;

            if (_proxy.vertical)
            {
                while (minIndex < maxIndex)
                {
                    var indexY = _rects[index].y;
                    var nextY = _rects[index + 1].y;

                    if (indexY >= offset && nextY < offset) break;
                    else if (nextY >= offset) minIndex = index + 1;
                    else maxIndex = index;

                    index = (maxIndex + minIndex) / 2;
                }
            }
            else
            {
                while (minIndex < maxIndex)
                {
                    var indexX = -_rects[index].x;
                    var nextX = -_rects[index + 1].x;

                    if (indexX >= offset && nextX < offset) break;
                    else if (nextX >= offset) minIndex = index + 1;
                    else maxIndex = index;

                    index = (maxIndex + minIndex) / 2;
                }
            }

            return index;
        }

        private Vector2 GetMinMaxVisibleIndex()
        {
            var trans = _proxy.transform as RectTransform;
            var offset = _proxy.content.anchoredPosition;

            var viewWidth = trans.rect.width;
            var viewHeight = trans.rect.height;

            var minIndex = _proxy.vertical ? IndexAtOffset(-offset.y) : IndexAtOffset(offset.x);
            var maxIndex = _proxy.vertical ? IndexAtOffset(-offset.y - viewHeight) : IndexAtOffset(offset.x - viewWidth);

            var boundMinIndex = 0;
            var boundMaxIndex = _numberOfCells - 1; // Mathf.Max(0, numberOfCells - 1);
            minIndex = minIndex - _extraBuffer / 2;
            minIndex = minIndex < boundMinIndex ? boundMinIndex : minIndex;
            maxIndex = maxIndex + _extraBuffer / 2;
            maxIndex = maxIndex > boundMaxIndex ? boundMaxIndex : maxIndex;

            return new Vector2(minIndex, maxIndex);
        }

        public void ReAllocateCellList()
        {
            _numberOfCells = _delegateNumberOfCells(this);
            _cellsList = new TableViewCell[_numberOfCells];
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="startIndex">重新加载后跳转到第startIndex行</param>
        public void ReloadData(ReloadType reloadType = ReloadType.Keep, int startIndex = 0, bool indexAtCenter = true, bool clearCell = false)
        {
            if (_delegateNumberOfCells == null) return;

            //清理
            if (clearCell)
            {
                _groupDict.Clear();
                // _proxy.content.Clear();
                CommonUtils.DestroyAllChildren(_proxy.content);
            }
            else
            {
                foreach (string identifier in _groupDict.Keys)
                {
                    _groupDict[identifier].QueueInvisible();
                }
            }

            //创建cell分组
            System.Action<int> createIdentifierGroup = null;
            if (_delegateIdentifierOfIndex == null)
            {
                if (_groupDict.ContainsKey(DEFAULT_IDENTIFIER) == false) _groupDict[DEFAULT_IDENTIFIER] = new TableViewGroup();
            }
            else
            {
                createIdentifierGroup = (i) =>
                {
                    string identifier = _delegateIdentifierOfIndex(this, i);
                    if (_groupDict.ContainsKey(identifier) == false) _groupDict[identifier] = new TableViewGroup();
                };
            }

            //计算content的大小
            _numberOfCells = _delegateNumberOfCells(this);
            _rects = new Rect[_numberOfCells];
            var width = 0f;
            var x = 0f;
            var height = 0f;
            var y = 0f;
            var contentPos = _proxy.content.anchoredPosition;
            var contentSize = _proxy.content.sizeDelta;
            var trans = _proxy.transform as RectTransform;
            var scrollViewWidth = trans.rect.width;
            var scrollViewHeight = trans.rect.height;

            if (_proxy.vertical)
            {
                for (var i = 0; i < _numberOfCells; i++)
                {
                    height = _delegateSizeOfIndex(this, i);
                    _rects[i] = new Rect(0, -y, scrollViewWidth, height);
                    y += height;
                    if (createIdentifierGroup != null) createIdentifierGroup(i);
                }

                contentSize.y = y;

                if (_centralized) //make content centralize when few cells
                {
                    var scrollHeight = (_proxy.ScrollRect.transform as RectTransform).rect.height;
                    if (contentSize.y < scrollHeight)
                    {
                        var delta = (scrollHeight - _rects.Length * height) / 2;
                        var newY = 0f;
                        for (var j = 0; j < _rects.Length; j++)
                        {
                            _rects[j] = new Rect(0, -(delta + newY), scrollViewWidth, height);
                            newY += height;
                        }

                        contentSize.y = scrollHeight;
                    }
                }
            }
            else
            {
                x += _proxy.AppendWidthAdStart;

                for (var i = 0; i < _numberOfCells; i++)
                {
                    width = _delegateSizeOfIndex(this, i);
                    _rects[i] = new Rect(x, 0, width, scrollViewHeight);
                    x += width;

                    if (createIdentifierGroup != null) createIdentifierGroup(i);
                }

                if (x < scrollViewWidth)
                {
                    x = 0;
                }
                else
                {
                    x = x - scrollViewWidth;
                }

                x += _proxy.AppendWidthAdEnd;

                contentSize.x = x;
            }

            _proxy.content.sizeDelta = contentSize;

            //设置list的偏移位置
            if (reloadType == ReloadType.ScrollTo)
            {
                var minMax = GetMinMaxVisibleIndex();
                _minVisibleIndex = (int) minMax.x;
                _maxVisibleIndex = (int) minMax.y;
                scrollToIndex(startIndex, indexAtCenter);
            }
            else if (reloadType == ReloadType.ScrollToCorrection)
            {
                scrollToIndexCorrection(startIndex);
            }
            else if (reloadType == ReloadType.Start)
            {
                if (_proxy.vertical) contentPos.y = -scrollViewHeight;
                else contentPos.x = scrollViewWidth;
                _proxy.content.anchoredPosition = contentPos;
            }
            else if (reloadType == ReloadType.Keep)
            {
                //do nothing
            }

            var indexes = GetMinMaxVisibleIndex();
            _minVisibleIndex = (int) indexes.x;
            _maxVisibleIndex = (int) indexes.y;

            //add cells
            if (_numberOfCells > 0)
            {
                for (var i = _minVisibleIndex; i <= _maxVisibleIndex; i++)
                {
                    addCellAtIndex(i);
                }
            }

            //初始化按页滑动配置
            if (_proxy.PageScroll) InitForPageView();

            //初始化arc cell
            updateCellArc();

            //初始化cell的siblingIndex
            updateCellIndex();
        }

        private void scrollToIndex(int targetIndex, bool indexAtCenter)
        {
            var contentPos = _proxy.content.anchoredPosition;

            if (indexAtCenter)
            {
                targetIndex = _convertIndexToCenter(targetIndex);
            }

            var width = 0f;
            var height = 0f;
            var x = 0f;
            var y = 0f;
            var offsetX = 0f;
            var offsetY = 0f;
            var trans = _proxy.transform as RectTransform;
            var viewWidth = trans.rect.width;
            var viewHeight = trans.rect.height;

            if (_proxy.vertical)
            {
                for (var i = 0; i < targetIndex; i++)
                {
                    height = _delegateSizeOfIndex(this, i);
                    offsetY += height;
                }
            }
            else
            {
                for (var i = 0; i <= targetIndex; i++)
                {
                    width = _delegateSizeOfIndex(this, i);
                    if (i == targetIndex)
                    {
                        offsetX -= width / 2f;
                    }
                    else
                    {
                        offsetX -= width;
                    }
                }

                offsetX = Mathf.Min(0f, offsetX);
                offsetX = Mathf.Max(-_proxy.content.sizeDelta.x, offsetX);
            }

            if (_proxy.vertical) contentPos.y = offsetY;
            else contentPos.x = offsetX;
            _proxy.content.anchoredPosition = contentPos;
        }

        private string getIdentifierForIndex(int index)
        {
            return _delegateIdentifierOfIndex == null ? DEFAULT_IDENTIFIER : _delegateIdentifierOfIndex(this, index);
        }

        public TableViewCell DequeueReusabelCell(int index)
        {
            var identifier = getIdentifierForIndex(index);
            var invisibilityCellQueue = _groupDict[identifier]._invisibilityCellQueue;
            var count = invisibilityCellQueue.Count;
            if (count > 0)
            {
                var cell = invisibilityCellQueue[count - 1];
                invisibilityCellQueue.RemoveAt(count - 1);
                return cell;
            }

            return null;
        }

        public TableViewCell GetCell(int index)
        {
            var identifier = getIdentifierForIndex(index);
            if (_groupDict.ContainsKey(identifier))
            {
                var group = _groupDict[identifier];
                group._visibilityCellQueueDic.TryGetValue(index, out var cell);
                return cell;
            }

            return null;
        }

        public List<TableViewCell> GetVisibleCells()
        {
            var result = new List<TableViewCell>();
            for (var i = _minVisibleIndex; i <= _maxVisibleIndex; i++)
            {
                result.Add(GetCell(i));
            }

            return result;
        }

        private int _convertIndexToCenter(int index)
        {
            // Debug.LogError("startIndex:" + startIndex);
            index -= Mathf.Abs(_maxVisibleIndex - _minVisibleIndex) / 2;
            // Debug.LogError("startIndex:" + startIndex);
            index = Mathf.Clamp(index, 0, _delegateNumberOfCells(this) - 1);
            // Debug.LogError("startIndex:" + startIndex);
            return index;
        }

#region 修正版的定位到某Index,目前只支持竖向且居中

        private void scrollToIndexCorrection(int targetIndex)
        {
            var tran_scroll = _proxy.ScrollRect.GetComponent<RectTransform>();

            float viewH = tran_scroll.rect.height;
            float viewCenter = viewH * -0.5f;
            float offset = -0.5f * _delegateSizeOfIndex(this, targetIndex);

            float posY = viewCenter - (_rects[targetIndex].y + offset);
            if (posY > _proxy.content.rect.height - viewH) posY = _proxy.content.rect.height - viewH;
            if (posY < 0) posY = 0;
            _proxy.content.anchoredPosition = new Vector2(_proxy.content.anchoredPosition.x, posY);
        }

        public void ScrollToTargetIndexCorrection(int currentIndex, int targetIndex, System.Action onScrollFinish)
        {
            _proxy.ScrollEnable = false;
            _status = Status.AutoMoving;

            var tran_scroll = _proxy.ScrollRect.GetComponent<RectTransform>();

            float viewH = tran_scroll.rect.height;
            float viewCenter = viewH * -0.5f;
            float offset = -0.5f * _delegateSizeOfIndex(this, targetIndex);

            float posY = viewCenter - (_rects[targetIndex].y + offset);
            if (posY > _proxy.content.rect.height - viewH) posY = _proxy.content.rect.height - viewH;
            if (posY < 0) posY = 0;

            var deltaIndex = Mathf.Abs(targetIndex - currentIndex);
            var duration = 0.1f;
            if (deltaIndex <= 3) duration = deltaIndex * 0.3f;
            else if (deltaIndex <= 6) duration = deltaIndex * 0.2f;
            else duration = deltaIndex * 0.05f;

            var scaleDuration = 0.1f;
            var easyType = Ease.InSine;

            //移动列表
            var sequence2 = DOTween.Sequence();
            sequence2.AppendInterval(scaleDuration);
            sequence2.Append(_proxy.content.DOAnchorPosY(posY, duration).SetEase(easyType)).onComplete = () => onScrollFinish?.Invoke();
        }

#endregion

        public void ScrollToTargetIndex(int currentIndex, int targetIndex, System.Action onScrollFinish)
        {
            _proxy.ScrollEnable = false;
            _status = Status.AutoMoving;

            var centerPosIndex = _convertIndexToCenter(targetIndex);

            var deltaIndex = Mathf.Abs(targetIndex - currentIndex);
            var duration = 0.1f;
            if (deltaIndex <= 3) duration = deltaIndex * 0.3f;
            else if (deltaIndex <= 6) duration = deltaIndex * 0.2f;
            else duration = deltaIndex * 0.05f;

            var scaleDuration = 0.1f;
            var easyType = Ease.InSine;

            //移动列表
            var sequence2 = DOTween.Sequence();
            sequence2.AppendInterval(scaleDuration);
            sequence2.Append(_proxy.content.DOAnchorPosY(-_rects[centerPosIndex].y, duration).SetEase(easyType)).onComplete = () => onScrollFinish?.Invoke();
        }

        public void MoveCellToTargetIndex(int moveIndex, int targetIndex, System.Action onMoveFinish)
        {
            if (moveIndex == targetIndex)
            {
                onMoveFinish?.Invoke();
                return;
            }

            _proxy.ScrollEnable = false;
            _status = Status.AutoMoving;

            var centerPosIndex = _convertIndexToCenter(targetIndex);

            var deltaIndex = Mathf.Abs(targetIndex - moveIndex);
            var duration = 0.1f;
            if (deltaIndex <= 3) duration = deltaIndex * 0.3f;
            else if (deltaIndex <= 6) duration = deltaIndex * 0.2f;
            else duration = deltaIndex * 0.1f;

            var scaleDuration = 0.1f;
            var moveCell = GetCell(moveIndex);
            var floatingCell = GameObject.Instantiate(moveCell.RectTrans, moveCell.RectTrans.parent);
            var easyType = Ease.InSine;

            //移动自己cell
            moveCell.RectTrans.gameObject.SetActive(false);
            var sequence = DOTween.Sequence();
            sequence.Append(floatingCell.DOScale(Vector3.one * 1.03f, scaleDuration));
            sequence.Append(floatingCell.DOAnchorPosY(_rects[targetIndex].y - _rects[targetIndex].size.y / 2f, duration).SetEase(easyType));
            sequence.Append(floatingCell.DOScale(Vector3.one, scaleDuration));
            sequence.onUpdate = () => floatingCell.SetAsLastSibling();
            sequence.onComplete = () =>
            {
                updateFloatingData();
                ReloadData();
                GameObject.Destroy(floatingCell.gameObject);

                _status = Status.None;
                onMoveFinish?.Invoke();
                _proxy.ScrollEnable = true;
            };

            //移动中间所有cell

            //移动列表
            var sequence2 = DOTween.Sequence();
            sequence2.AppendInterval(scaleDuration);
            sequence2.Append(_proxy.content.DOAnchorPosY(-_rects[centerPosIndex].y, duration).SetEase(easyType));
        }

        private void Update()
        {
            updatePageScroll();
        }
    }
}