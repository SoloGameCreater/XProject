using UnityEngine;
using System;
using Framework;

namespace TripleMerge
{
    
    public class TripleMergeCameraComponent : TripleMergeComponent
    {
        private enum ECameraInputBehaviour
        {
            None,
            Prepare,
            CameraMove,
            CameraScale,
        }
        private Camera _sceneCamera;
        
        public Transform MinPosition;
        public Transform MaxPosition;
        public Transform InitPosition;
        private float _originCameraSize;
        private float _minCameraSize;
        private float _maxCameraSize;
        public float _minCameraScale = 0.3f;
        public float _maxCameraScale = 1f;

        private Vector2 _minCameraPosition;
        private Vector2 _maxCameraPosition;
        private Vector3 _screenMinPosition;
        private Vector3 _screenMaxPosition;

        private Touch _startTouch_1;
        private Touch _startTouch_2;

        private Vector2 _prevMousePos;
        private Vector2 _moveSpeed;
        private float _autoScale;
        private Action _completeCallback = null;

        private Vector3 _targetPosition;
        private float _targetScale;
        private Vector3 _originPosition;
        private float _originScale;
        private bool _focusEnable;
        public float _focusTime = 1.0f;
        private float _focusEscapeTime = 0.0f;

        private ECameraInputBehaviour _cameraInputBehaviour;
        //private Vector3 _lastMoveControlTouch;
        private Vector3 _lastScaleTouch1, _lastScaleTouch2;
        public Transform DraggingItem;
        public float CurrentCameraScale
        {
            get { return _sceneCamera.orthographicSize / _originCameraSize; }
        }
        public bool IsInCameraOperation => _cameraInputBehaviour != ECameraInputBehaviour.None;
        public TripleMergeGameplay GamePlayComponent { get; private set; }
        protected override void OnInitialize()
        {
            GamePlayComponent = TripleMergeSystem.Instance.Gameplay;
            _sceneCamera = SceneRoot.Instance.mSceneCamera;
            _cameraInputBehaviour = ECameraInputBehaviour.None;
            
            MinPosition = GamePlayComponent.MapManager.MapArea.CameraMinPoint;
            MaxPosition = GamePlayComponent.MapManager.MapArea.CameraMaxPoint;
            InitPosition = GamePlayComponent.MapManager.MapArea.CameraInitPoint;

            _originCameraSize = GamePlayComponent.MapManager.MapArea.CameraMaxScaler;
            
            _minCameraSize = GamePlayComponent.MapManager.MapArea.CameraMinScaler;
            _maxCameraSize = GamePlayComponent.MapManager.MapArea.CameraMaxScaler;

            _minCameraPosition = MinPosition.position;
            _maxCameraPosition = MaxPosition.position;
            
            _sceneCamera.transform.position = InitPosition.position;
            var uiRootTransform = UIRoot.Instance.mRoot.transform as RectTransform;
            var uiCamera = UIRoot.Instance.mUICamera;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRootTransform, new Vector2(0, 0), uiCamera, out _screenMinPosition);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(uiRootTransform, new Vector2(Screen.width, Screen.height), uiCamera, out _screenMaxPosition);

            if (CommonUtils.IsWideScreenDevice())
            {
                _minCameraScale /= 0.8f;

                _maxCameraScale *= 0.91f;
            }
        }
        private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        protected override void OnUpdate()
        {
            
    
            if (CommonUtils.IsTouchUGUI()) return;
            
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
            PCInputListener();
#elif UNITY_ANDROID || UNITY_IOS
            CellPhoneInputListener();
#endif
        }

        private bool _isMouseInputting;
        private void PCInputListener()
        {
            // 摄像机缩放
            HandlePCCameraZoom();

            // 当松开鼠标时，重置状态
            if (!Input.GetMouseButton(0))
            {
                _isMouseInputting = false;
                _cameraInputBehaviour = ECameraInputBehaviour.None;
                _prevMousePos = Vector2.zero;
                return;
            }

            // 处理鼠标按下的初始状态
            if (Input.GetMouseButtonDown(0))
            {
                if (!CommonUtils.IsTouchUGUI() && _cameraInputBehaviour == ECameraInputBehaviour.None)
                {
                    _isMouseInputting = true;
                    _prevMousePos = Input.mousePosition;
                    _cameraInputBehaviour = ECameraInputBehaviour.Prepare;
                }
                return;
            }

            // 只在已经开始输入的情况下检测移动
            if (_isMouseInputting && _cameraInputBehaviour == ECameraInputBehaviour.Prepare)
            {
                Vector2 mouseDelta = (Vector2)Input.mousePosition - _prevMousePos;
                if (mouseDelta.magnitude >= 1.5f)
                {
                    _cameraInputBehaviour = ECameraInputBehaviour.CameraMove;
                }
            }

            // 处理相机移动
            if (_cameraInputBehaviour == ECameraInputBehaviour.CameraMove)
            {
                HandlePCCameraMove();
            }
        }

        private void HandlePCCameraMove()
        {
            var currentMousePos = Input.mousePosition;
            var worldOffset = _sceneCamera.ScreenToWorldPoint(currentMousePos) - _sceneCamera.ScreenToWorldPoint(_prevMousePos);
            _moveSpeed = worldOffset / Time.deltaTime;

            Move(_moveSpeed);
            
            _prevMousePos = currentMousePos;
        }

        private void HandlePCCameraZoom()
        {
            var scrollWheelDelta = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Approximately(scrollWheelDelta, 0f))
            {
                return;
            }

            var curSize = _sceneCamera.orthographicSize;
            curSize /= 1 + scrollWheelDelta;
            TouchScale(curSize);
            //SaveCamera();
        }
        private void CellPhoneInputListener()
        {
            // Early return if no touches
            if (Input.touches.Length == 0)
            {
                return;
            }
            if (_cameraInputBehaviour == ECameraInputBehaviour.None)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began)
                {
                    return;
                }

                if (CommonUtils.IsTouchUGUI())
                {
                    return;
                }
                _startTouch_1 = touch;
                _cameraInputBehaviour = ECameraInputBehaviour.Prepare;
                return; 
            }

            switch (_cameraInputBehaviour)
            {
                case ECameraInputBehaviour.Prepare:
                    HandlePrepareStage();
                    break;
                case ECameraInputBehaviour.CameraMove:
                    HandleCameraMove();
                    break;
                case ECameraInputBehaviour.CameraScale:
                    HandleCameraScale();
                    break;
            }
        }

        private void HandlePrepareStage()
        {
            if (Input.touches.Length == 1)
            {
                var touch = Input.GetTouch(0);
                if (touch is { phase: TouchPhase.Moved, deltaPosition: { magnitude: >= 1.5f } })
                {
                    _startTouch_1 = touch;
                    _cameraInputBehaviour = ECameraInputBehaviour.CameraMove;
                    Debug.Log($"enter move stage");
                }
            }
            else if (Input.touches.Length >= 2)
            {
                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);

                if (touch2.phase == TouchPhase.Began)
                {
                    _cameraInputBehaviour = ECameraInputBehaviour.CameraScale;
                    _startTouch_1 = touch1;
                    _startTouch_2 = touch2;
                    DraggingItem = null;
                }
            }
        }
        private void HandleCameraMove()
        {
            if (Input.touches.Length == 1)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _cameraInputBehaviour = ECameraInputBehaviour.None;
                    //SaveCamera();
                    Debug.Log("将最新的相机信息写入存档!");
                    return;
                }

                var currentMousePos = _startTouch_1.position;
                var prePos = _startTouch_1.position - _startTouch_1.deltaPosition;
                var worldOffset = _sceneCamera.ScreenToWorldPoint(currentMousePos) - _sceneCamera.ScreenToWorldPoint(prePos);
                _moveSpeed = worldOffset / Time.deltaTime;
                
                Move(_moveSpeed);
                _startTouch_1 = touch;
            }
            else if (Input.touches.Length >= 2)
            {
                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);

                _startTouch_1 = touch1;
                _startTouch_2 = touch2;
                _cameraInputBehaviour = ECameraInputBehaviour.CameraScale;
            }
        }
        private void Move(Vector3 speed)
        {
            if (speed == Vector3.zero) return;

            var currentPos = _sceneCamera.transform.position;

            var newPos = currentPos - speed * Time.deltaTime;
            var nextPos = Vector3.Lerp(currentPos, newPos, Time.deltaTime * 50f);

            var curScale = _sceneCamera.orthographicSize / _originCameraSize;
            var halfScreenSize = (_screenMaxPosition - _screenMinPosition) / 2 * curScale;

            var xMin = _minCameraPosition.x + halfScreenSize.x;
            var xMax = _maxCameraPosition.x - halfScreenSize.x;
            var yMin = _minCameraPosition.y + halfScreenSize.y;
            var yMax = _maxCameraPosition.y - halfScreenSize.y;

            if (nextPos.x < xMin) nextPos.x = xMin;
            if (nextPos.x > xMax) nextPos.x = xMax;
            if (nextPos.y < yMin) nextPos.y = yMin;
            if (nextPos.y > yMax) nextPos.y = yMax;

            _sceneCamera.transform.position = nextPos;
        }
        private void HandleCameraScale()
        {
            if (Input.touches.Length >= 2)
            {
                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);
                if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                {
                    _startTouch_1 = touch0;
                    _startTouch_2 = touch1;
                }
                else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    var currentDist = Vector2.Distance(touch0.position, touch1.position);
                    var prevDist = Vector2.Distance(touch0.position - touch0.deltaPosition,
                        touch1.position - touch1.deltaPosition);

                    var curSize = CameraManager.MainCamera.orthographicSize;
                    curSize /= currentDist / prevDist;
                    TouchScale(curSize);
                }
            }
            else if (Input.touches.Length == 1)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _cameraInputBehaviour = ECameraInputBehaviour.None;
                    return;
                }

                _startTouch_1 = touch;
                _cameraInputBehaviour = ECameraInputBehaviour.CameraMove;
            }
        }
        private void TouchScale(float curSize)
        {
            if (Mathf.Approximately(_sceneCamera.orthographicSize, curSize)) return;

            curSize = Mathf.Clamp(curSize, _minCameraSize, _maxCameraSize);
            _sceneCamera.orthographicSize = curSize;

            BoundLimit();
        }
        
        private void BoundLimit()
        {
            var curScale = _sceneCamera.orthographicSize / _originCameraSize;

            var halfScreenSize = (_screenMaxPosition - _screenMinPosition) / 2 * curScale;
            var position = _sceneCamera.transform.position;

            position.x = Mathf.Clamp(position.x, _minCameraPosition.x + halfScreenSize.x, _maxCameraPosition.x - halfScreenSize.x);
            position.y = Mathf.Clamp(position.y, _minCameraPosition.y + halfScreenSize.y, _maxCameraPosition.y - halfScreenSize.y);
            _sceneCamera.transform.position = position;
        }
        
        private void Reset()
        {
            _prevMousePos = Vector2.zero;
            _moveSpeed = Vector2.zero;
            _targetPosition = Vector3.zero;
            _targetScale = 0f;
            _originPosition = Vector3.zero;
            _originScale = 0f;
            _focusEnable = false;
            _focusTime = 1.0f;
            _focusEscapeTime = 0.0f;

            _sceneCamera.transform.position = Vector3.zero;
            _sceneCamera.orthographicSize = _originCameraSize;
        }

        protected override void OnDispose()
        {
            Reset();
        }
    }
}