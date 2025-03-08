using System;
using DG.Tweening;
using Framework;
using UnityEngine;

namespace TripleMerge
{
    public partial class OnCellObject
    {
        protected enum EInputOrderStage
        {
            None,
            MouseDown,
            LongPressListen,
            LongPressed,
            Dragging,
        }
        
        private const float LongPressListenTriggerTime = 0.2f;
        private const float LongPressListenEnterTime = 0.5f;
        public bool IsDragging { private set; get; }

        protected enum EInputStage
        {
            None,
            MouseDown,
            LongPressListen,
            LongPressed,
            Dragging,
        }

        private EInputStage _currentInputStage;

        protected bool IsLongPressedTriggered { get; set; }
        private float _mouseDownTime = float.NegativeInfinity;

        private Vector2? _mouseDownPosition;

        private void Update()
        {
            try
            {
                if (_currentInputStage == EInputStage.None)
                {
                    return;
                }

                CheckTouchCount();
                HandleInputStages();
            }
            catch
            {
                Debug.Log($"touch override");
                OnPointerUp();
            }
        }

        /// <summary>
        /// 检查触摸点数量，在移动平台上确保触摸点有效
        /// </summary>
        private void CheckTouchCount()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount <= 0 || TouchIndex > Input.touchCount)
            {
                OnPointerUp();
            }
#endif
        }

        /// <summary>
        /// 根据当前输入阶段分发处理逻辑
        /// </summary>
        private void HandleInputStages()
        {
            switch (_currentInputStage)
            {
                case EInputStage.MouseDown:
                    HandleMouseDownStage();
                    break;
                case EInputStage.LongPressListen:
                    HandleLongPressListenStage();
                    break;
                case EInputStage.Dragging:
                    HandleDraggingStage();
                    break;
            }
        }

        /// <summary>
        /// 处理鼠标按下阶段的逻辑
        /// 检测是否需要进入拖拽状态或长按监听状态
        /// </summary>
        private void HandleMouseDownStage()
        {
            Vector2 mousePosition = GetCurrentInputPosition();
            
            if (IsValidMoveTrigger(mousePosition))
            {
                ModifyInputStage(EInputStage.Dragging);
            }
            else if (Time.time - _mouseDownTime >= LongPressListenTriggerTime)
            {
                ModifyInputStage(EInputStage.LongPressListen);
            }
        }

        /// <summary>
        /// 处理长按监听阶段的逻辑
        /// 检测是否需要进入拖拽状态或触发长按事件
        /// </summary>
        private void HandleLongPressListenStage()
        {
            Vector2 mousePosition = GetCurrentInputPosition();
            
            if (IsValidMoveTrigger(mousePosition))
            {
                ModifyInputStage(EInputStage.Dragging);
            }
            else
            {
                var progress = Mathf.Min(1, (Time.time - _mouseDownTime) / (LongPressListenTriggerTime + LongPressListenEnterTime));
                
                if (progress >= 1)
                {
                    ModifyInputStage(EInputStage.LongPressed);
                }
            }
        }

        /// <summary>
        /// 处理拖拽阶段的逻辑
        /// 更新物体位置和最近的触发单元格
        /// </summary>
        private void HandleDraggingStage()
        {
            UpdateNearestTriggeredCell();

            var originZ = transform.position.z;
            var touchPosition = GetCurrentInputPosition();
            var worldPosition = SceneRoot.Instance.mSceneCamera.ScreenToWorldPoint(touchPosition);
            worldPosition.z = originZ;

            transform.position = worldPosition;
        }

        /// <summary>
        /// 获取当前平台下的输入位置
        /// 在编辑器和PC平台返回鼠标位置，在移动平台返回触摸位置
        /// </summary>
        /// <returns>输入位置的二维坐标</returns>
        private Vector2 GetCurrentInputPosition()
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            return Input.mousePosition;
#else
            return Input.GetTouch(TouchIndex).position;
#endif
        }

        /// <summary>
        /// 判断是否触发了有效的移动
        /// 在PC平台检查位置是否改变，在移动平台检查移动距离是否超过阈值
        /// </summary>
        /// <param name="currentPosition">当前输入位置</param>
        /// <returns>是否触发了有效的移动</returns>
        private bool IsValidMoveTrigger(Vector2 currentPosition)
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            return currentPosition != _mouseDownPosition;
#else
            return Input.GetTouch(TouchIndex).deltaPosition.magnitude > 1.75f;
#endif
        }

        private void UpdateNearestTriggeredCell()
        {
            if (_triggeredCells == null || _triggeredCells.Count == 0)
            {
                return;
            }

            MergeableCell nearestCell = null;

            var minDistance = float.MaxValue;

            foreach (var triggeredCell in _triggeredCells.Values)
            {
                var distance = Vector2.Distance(TriggerPivot.position, triggeredCell.InteractiveTrigger.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCell = triggeredCell;
                }
            }

            SetTouchedCell(nearestCell);
        }

        private void SetTouchedCell(MergeableCell cell)
        {
            if (TouchedCell == cell)
            {
                return;
            }

            OnTouchCellUpdateBefore();
            
            ResetPreviousTouchedCell();
            
            TouchedCell = cell;
            
            HandleTouchedCellItem();
        }

        /// <summary>
        /// 重置之前触摸的单元格中的物品位置
        /// </summary>
        private void ResetPreviousTouchedCell()
        {
            if (TouchedCell == null || TouchedCell.PlacedItem == null || TouchedCell.PlacedItem == this)
            {
                return;
            }
            
            var cellTransform = TouchedCell.PlacedItem.transform;
            if (cellTransform == null)
            {
                return;
            }
            
            cellTransform.DOKill();
            cellTransform.DOLocalMove(new Vector3(0, 0, cellTransform.position.z), 0.15f);
        }

        /// <summary>
        /// 处理当前触摸单元格中的物品位置
        /// </summary>
        private void HandleTouchedCellItem()
        {
            if (TouchedCell == null || TouchedCell.PlacedItem == null || TouchedCell.PlacedItem == this)
            {
                return;
            }
            
            var placedItem = TouchedCell.PlacedItem;
            if (placedItem.transform == null)
            {
                return;
            }
            
            var selfPosition = transform.position;
            var touchItemPosition = placedItem.transform.position;
            var xDistance = selfPosition.x - touchItemPosition.x;
            var yDistance = selfPosition.y - touchItemPosition.y;
            
            var safeItem = placedItem;
            
            if (Mathf.Abs(xDistance) >= Mathf.Abs(yDistance))
            {
                MoveItemHorizontally(safeItem, xDistance);
            }
            else
            {
                MoveItemVertically(safeItem, yDistance);
            }
        }

        /// <summary>
        /// 水平方向移动物品
        /// </summary>
        private void MoveItemHorizontally(OnCellObject item, float xDistance)
        {   
            item.transform.DOKill();
            
            float offsetX = xDistance >= 0 ? -0.25f : 0.25f;
            Vector3 targetPosition = new(
                item.transform.localPosition.x + offsetX,//x
                0,//y
                item.transform.localPosition.z//z
            );
            
            item.transform.DOLocalMove(targetPosition, 0.15f)
                .OnComplete(() => {
                    if (item != null && item.transform != null)
                    {
                        OnTouchCellUpdated();
                    }
                });
        }

        /// <summary>
        /// 垂直方向移动物品
        /// </summary>
        private void MoveItemVertically(OnCellObject item, float yDistance)
        {   
            item.transform.DOKill();
            
            float offsetY = yDistance >= 0 ? -0.25f : 0.25f;
            Vector3 targetPosition = new(
                0,//x
                item.transform.localPosition.y + offsetY,//y
                item.transform.localPosition.z//z
            );
            
            item.transform.DOLocalMove(targetPosition, 0.15f)
                .OnComplete(() => {
                    if (item != null && item.transform != null)
                    {
                        OnTouchCellUpdated();
                    }
                });
        }

        protected virtual void OnTouchCellUpdateBefore()
        {
        }

        protected virtual void OnTouchCellUpdated()
        {
        }

        private void ModifyInputStage(EInputStage stage)
        {
            BeforeInputOrderStageUpdate(stage);

            _currentInputStage = stage;

            AfterInputOrderStageUpdate();
        }

        private void BeforeInputOrderStageUpdate(EInputStage stage)
        {
            switch (_currentInputStage)
            {
                case EInputStage.None:
                    break;

                case EInputStage.MouseDown:
                    if (stage == EInputStage.None)
                    {
                        // 点击音效
                        OnClicked();
                    }

                    break;

                case EInputStage.LongPressListen:
                    //ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedUIHandler.gameObject.SetActive(false);
                    break;

                case EInputStage.LongPressed:
                    break;

                case EInputStage.Dragging:
                    EndDragging();
                    break;
            }
        }

        private void EndDragging()
        {
            IsDragging = false;

            _triggeredCells.Clear();

            Deselect();

            OnDragEnd();

            #region 拖动结束逻辑

            TripleMergeSystem.Instance.Gameplay.CameraInputManager.DraggingItem = null;
            
            if (TouchedCell == null || TouchedCell == BelongCell || !TouchedCell.CanPlaceTargetSizeItem())
            {
                transform.SetParent(BelongCell.PlaceItemRoot);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                // 万能卡
                if (TouchedCell.PlacedItem != null && TouchedCell.PlacedItem != this 
                && IsUniversalCard && this is MergeableObject mergeableObject)
                {
                    var continuousItems = TouchedCell.GetContinuousSameItems(mergeableObject);
                    if (continuousItems.Count >= 3)
                    {
                        TouchedCell.PlacedItem.transform.DOKill();
                        TouchedCell.PlacedItem.transform.localPosition = Vector3.zero;
                        // todo 弹出万能卡合成界面
                        //TripleMergeSystem.Instance.Gameplay.GameplayBridge.OpenOmnipotentCardMergePopUp(this, continuousItems);
                        return;
                    }
                }
                
                TouchedCell.PlaceItem(this, null, () => { 
                    EventDispatcher.Instance.DispatchEvent(EventEnum.TripleMergeOnMapContentChanged); 
                });
            }

            #endregion
        }

        private void AfterInputOrderStageUpdate()
        {
            switch (_currentInputStage)
            {
                case EInputStage.None:
                    break;

                case EInputStage.MouseDown:
                    break;

                case EInputStage.LongPressListen:
                    // var uiHandler = ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedUIHandler;
                    // uiHandler.transform.position = BelongCell.transform.position;
                    // uiHandler.UpdateProgress(0);
                    // uiHandler.gameObject.SetActive(true);
                    break;

                case EInputStage.LongPressed:
                    //ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedUIHandler.gameObject.SetActive(false);
                    Select();
                    IsLongPressedTriggered = true;
                    OnLongPressTrigger();
                    break;

                case EInputStage.Dragging:
                    BeginDragging();
                    break;
            }
        }

        private void BeginDragging()
        {
            TripleMergeSystem.Instance.Gameplay.CameraInputManager.DraggingItem = transform;

            IsDragging = true;

            Select();

            OnDragBegin();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("TripleMergeCell"))
            {
                OnCellTriggered();
            }

            void OnCellTriggered()
            {
                if (!_triggeredCells.TryGetValue(other, out var triggeredCell))
                {
                    triggeredCell = other.gameObject.GetComponentInParent<MergeableCell>();
                }

                if (triggeredCell == null || triggeredCell.CellStatus != MergeableCell.ECellStatus.Mergeable)
                {
                    return;
                }

                if (!triggeredCell.CanPlaceTargetSizeItem())
                {
                    return;
                }

                _triggeredCells.TryAdd(other, triggeredCell);
            }
        }

        #region input listener

        public void OnMouseDown()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            OnPointerDown();
#endif
        }

        public override void OnPointerDown(int touchIndex = 0)
        {
            base.OnPointerDown(touchIndex);

            if (BelongCell == null || BelongCell.CellStatus != MergeableCell.ECellStatus.Mergeable)
            {
                if (BelongCell == null)
                {
                    Debug.Log($"{name} : BelongCell is null,cant click");
                }
                else
                {
                    Debug.Log($"{name} : BelongCell status is not mergeable,cant click");
                }

                return;
            }

            //if (BelongCell.ActiveGuidanceTask == null)
            {
                if (TripleMergeSystem.Instance.Gameplay.CameraInputManager.IsInputDisabled)
                {
                    Debug.Log($"{name} : InputDisabled,cant click");
                    return;
                }

                if (BelongCell == null)
                {
                    return;
                }

                if (BelongCell.CellStatus != MergeableCell.ECellStatus.Mergeable)
                {
                    BelongCell.OnMouseDown();
                    return;
                }

                if (CommonUtils.IsTouchUGUI())
                {
                    Debug.Log($"{name} : click on ui,cant click");
                    return;
                }

                if (IsLongPressedTriggered)
                {
                    // var needBlockMouseEvent = OnMouseDownWithLongPressedMode();
                    // if (needBlockMouseEvent)
                    {
                        return;
                    }
                }

                if (TripleMergeSystem.Instance.Gameplay.MapManager.IsMerging)
                {
                    Debug.Log($"{name} : map is merging,cant click");
                    return;
                }
            }

            _mouseDownTime = Time.time;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            _mouseDownPosition = Input.mousePosition;
#else
            _mouseDownPosition = Input.GetTouch(TouchIndex).position;
#endif
            _currentInputStage = EInputStage.MouseDown;
        }

        private void OnMouseUp()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            OnPointerUp();
#endif
        }

        public override void OnPointerUp()
        {
            base.OnPointerUp();
            if (_currentInputStage == EInputStage.None)
            {
                return;
            }

            if (BelongCell == null)
            {
                return;
            }

            _mouseDownTime = float.NegativeInfinity;

            ModifyInputStage(EInputStage.None);
        }

        private void OnMouseUpAsButton()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            OnPointerClick();
#endif
        }

        public override void OnPointerClick()
        {
            base.OnPointerClick();

            if (IsDragging)
            {
                return;
            }

            if (BelongCell == null)
            {
                return;
            }

            if (BelongCell.CellStatus != MergeableCell.ECellStatus.Mergeable)
            {
                //BelongCell.ShowCellStatusTips();
            }
        }

        #endregion
    }
}