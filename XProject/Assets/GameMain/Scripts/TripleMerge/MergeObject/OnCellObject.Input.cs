using System;
using DG.Tweening;
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

#if UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount <= 0 || TouchIndex > Input.touchCount)
                {
                    OnPointerUp();
                }
#endif

                if (_currentInputStage == EInputStage.MouseDown)
                {
                    // 获取鼠标位置
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                    Vector2 mousePosition = Input.mousePosition;
#else
                    Vector2 mousePosition = Input.GetTouch(TouchIndex).position;
#endif
                    if (IsValidMoveTrigger())
                    {
                        ModifyInputStage(EInputStage.Dragging);
                    }
                    else
                    {
                        //if (!ThreeMergeSystem.Instance.Gameplay.InputListener.IsInputDisabled && 
                        if (Time.time - _mouseDownTime >= LongPressListenTriggerTime)
                        {
                            ModifyInputStage(EInputStage.LongPressListen);
                        }
                    }

                    bool IsValidMoveTrigger()
                    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                        return mousePosition != _mouseDownPosition;
#endif
                        return Input.GetTouch(TouchIndex).deltaPosition.magnitude > 1.75f;
                    }
                }

                if (_currentInputStage == EInputStage.LongPressListen)
                {
                    // 获取鼠标位置
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                    Vector2 mousePosition = Input.mousePosition;
#else
                    Vector2 mousePosition = Input.GetTouch(TouchIndex).position;
#endif

                    if (IsValidMoveTrigger())
                    {
                        ModifyInputStage(EInputStage.Dragging);
                    }
                    else
                    {
                        //var uiHandler = ThreeMergeSystem.Instance.Gameplay.MapManager.CellLongPressedUIHandler;
                        var progress = Mathf.Min(1, (Time.time - _mouseDownTime) / (LongPressListenTriggerTime + LongPressListenEnterTime));
                        //uiHandler.UpdateProgress(progress);

                        if (progress >= 1)
                        {
                            ModifyInputStage(EInputStage.LongPressed);
                        }
                    }

                    bool IsValidMoveTrigger()
                    {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                        return mousePosition != _mouseDownPosition;
#endif
                        return Input.GetTouch(TouchIndex).deltaPosition.magnitude > 1.75f;
                    }
                }

                if (_currentInputStage == EInputStage.Dragging)
                {
                    UpdateNearestTriggeredCell();

                    var originZ = transform.position.z;
                    // 获取鼠标位置
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
                    var touchPosition = (Vector2)Input.mousePosition;
#else
                    var touchPosition = Input.GetTouch(TouchIndex).position;
#endif
                    // 将屏幕坐标转换为世界坐标
                    var worldPosition = SceneRoot.Instance.mSceneCamera.ScreenToWorldPoint(touchPosition);
                    worldPosition.z = originZ;

                    // 设置物体的位置为转换后的世界坐标
                    transform.position = worldPosition;
                }
            }
            catch (ArgumentException e)
            {
                Debug.Log($"touch override");
                OnPointerUp();
            }
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
            if (TouchedCell != null && TouchedCell.PlacedItem != null && TouchedCell.PlacedItem != this)
            {
                var cellTransform = TouchedCell.PlacedItem.transform;

                cellTransform.DOKill();

                TouchedCell.PlacedItem.transform.DOLocalMove(new Vector3(0, 0, cellTransform.position.z), 0.15f);
            }

            TouchedCell = cell;

            if (TouchedCell != null && TouchedCell.PlacedItem != null && TouchedCell.PlacedItem != this)
            {
                var selfPosition = transform.position;
                var touchItemPosition = TouchedCell.PlacedItem.transform.position;
                var xDistance = selfPosition.x - touchItemPosition.x;
                var yDistance = selfPosition.y - touchItemPosition.y;

                if (Mathf.Abs(xDistance) >= Mathf.Abs(yDistance))
                {
                    TouchedCell.PlacedItem.transform.DOKill();
                    TouchedCell.PlacedItem.transform.DOLocalMove(
                                    new Vector3(
                                        xDistance >= 0 ? TouchedCell.PlacedItem.transform.localPosition.x - 0.25f : TouchedCell.PlacedItem.transform.localPosition.x + 0.25f, 0,
                                        TouchedCell.PlacedItem.transform.localPosition.z), 0.15f)
                               .OnComplete(OnTouchCellUpdated);
                }
                else
                {
                    TouchedCell.PlacedItem.transform.DOKill();
                    TouchedCell.PlacedItem.transform.DOLocalMove(
                                    new Vector3(0,
                                        yDistance >= 0 ? TouchedCell.PlacedItem.transform.localPosition.y - 0.25f : TouchedCell.PlacedItem.transform.localPosition.y + 0.25f,
                                        TouchedCell.PlacedItem.transform.localPosition.z), 0.15f)
                               .OnComplete(OnTouchCellUpdated);
                }
            }
        }

        protected virtual void OnTouchCellUpdateBefore()
        {
            Debug.Log("关闭合成提示");
        }

        protected virtual void OnTouchCellUpdated()
        {
            Debug.Log("打开合成提示");
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

            // todo 拖动结束逻辑

            #region 拖动结束逻辑

            // ThreeMergeSystem.Instance.Gameplay.InputListener.DraggingItem = null;
            //
            // if (BelongCell != null && BelongCell.ActiveGuidanceTask != null)
            // {
            //     var hostCell = BelongCell;
            //     if (!BelongCell.ActiveGuidanceTask.TargetCell.Contains(TouchedCell))
            //     {
            //         transform.SetParent(BelongCell.PlaceItemRoot);
            //         transform.localPosition = Vector3.zero;
            //     }
            //     else
            //     {
            //         // 万能卡
            //         if (TouchedCell.PlacedItem != null && TouchedCell.PlacedItem != this && IsOmnipotentMergeable && this is MergeableObject mergeableObject)
            //         {
            //             var continuousItems = TouchedCell.GetContinuousSameItems(mergeableObject);
            //             if (continuousItems.Count >= 3)
            //             {
            //                 TouchedCell.PlacedItem.transform.DOKill();
            //                 TouchedCell.PlacedItem.transform.localPosition = Vector3.zero;
            //                 GuideSubSystem.Instance.FinishCurrent(GuideTargetType.ThreeMergeMergeItems);
            //                 ThreeMergeSystem.Instance.Gameplay.GameplayBridge.OpenOmnipotentCardMergePopUp(this, continuousItems);
            //                 BelongCell.OnActiveGuidanceCompleted();
            //                 return;
            //             }
            //         }
            //
            //         TouchedCell.PlaceItem(this, false, true, 0.05f, () => { hostCell.OnActiveGuidanceCompleted(); }, false, () => { EventDispatcher.Instance.DispatchEventImmediately(EventEnum.ThreeMergeOnMapContentChanged); });
            //     }
            //
            //     return;
            // }
            //
            // if (TouchedCell == null || TouchedCell == BelongCell || !TouchedCell.CanPlaceTargetSizeItem(CfgData.CellSizeObj, true))
            // {
            //     transform.SetParent(BelongCell.PlaceItemRoot);
            //     transform.localPosition = Vector3.zero;
            // }
            // else
            // {
            //     // 万能卡
            //     if (TouchedCell.PlacedItem != null && TouchedCell.PlacedItem != this && IsOmnipotentMergeable && this is MergeableObject mergeableObject)
            //     {
            //         var continuousItems = TouchedCell.GetContinuousSameItems(mergeableObject);
            //         if (continuousItems.Count >= 3)
            //         {
            //             TouchedCell.PlacedItem.transform.DOKill();
            //             TouchedCell.PlacedItem.transform.localPosition = Vector3.zero;
            //             ThreeMergeSystem.Instance.Gameplay.GameplayBridge.OpenOmnipotentCardMergePopUp(this, continuousItems);
            //             return;
            //         }
            //     }
            //
            //     TouchedCell.PlaceItem(this, false, true, 0.05f, null, false, () => { EventDispatcher.Instance.DispatchEventImmediately(EventEnum.ThreeMergeOnMapContentChanged); });
            // }

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
            //ThreeMergeSystem.Instance.Gameplay.InputListener.DraggingItem = transform;
            //ThreeMergeSystem.Instance.Gameplay.InputListener.TryClearGuidingItems();

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

                // if (!triggeredCell.CanPlaceTargetSizeItem(CfgData.CellSizeObj, true))
                // {
                //     return;
                // }

                _triggeredCells.TryAdd(other, triggeredCell);
            }
        }

        #region input listener

        public void OnMouseDown()
        {
#if UNITY_EDITOR
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
                // if (ThreeMergeSystem.Instance.Gameplay.InputListener.IsInputDisabled)
                // {
                //     Debug.Log($"{name} : InputDisabled,cant click");
                //     return;
                // }

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

#if UNITY_EDITOR
            _mouseDownPosition = Input.mousePosition;
#else
            _mouseDownPosition = Input.GetTouch(TouchIndex).position;
#endif
            _currentInputStage = EInputStage.MouseDown;
        }

        private void OnMouseUp()
        {
#if UNITY_EDITOR
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
#if UNITY_EDITOR
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