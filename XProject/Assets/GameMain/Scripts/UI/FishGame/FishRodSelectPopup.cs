using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Config.FishGame;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace FishGameRuntime
{
    public enum RodPurchaseResult
    {
        Success,
        NotEnoughCoin,
        AlreadyOwned,
        ConfigError
    }

    public class FishRodSelectParam : UIViewParam
    {
        public int CurrentRodId;
        public HashSet<int> OwnedRodIds;
        public Action<int> OnRodEquipped;
        public Func<int, RodPurchaseResult> OnRodPurchased;
        public Action OnPopupClosed;
    }

    [AssetAddress("UIFishGame/FishRodSelectPopup")]
    public class FishRodSelectPopup : UIPopup
    {
        private static readonly Color CardInnerColor = new(0.10f, 0.20f, 0.27f, 0.78f);
        private static readonly Color AccentColor = new(0.20f, 0.72f, 0.86f, 0.92f);
        private static readonly Color MutedTextColor = new(0.72f, 0.83f, 0.88f, 1f);
        private static readonly Color EquippedBtnColor = new(0.15f, 0.49f, 0.24f);
        private static readonly Color EquipBtnColor = new(0.18f, 0.56f, 0.67f);
        private static readonly Color BuyBtnColor = new(0.76f, 0.46f, 0.14f);
        private static readonly Color DisabledBtnColor = new(0.3f, 0.3f, 0.3f, 0.6f);
        private static readonly Color ErrorColor = new(1f, 0.45f, 0.45f);

        [ComponentBinder("DialogPanel/TitleBar/CoinText")] private Text _coinText;
        [ComponentBinder("DialogPanel/TitleBar/CloseButton")] private Button _closeButton;
        [ComponentBinder("DialogPanel/ScrollView/Viewport/Content")] private RectTransform _content;

        private FishRodSelectParam _param;
        private readonly List<RodRowRef> _rows = new();
        private Font _font;

        public override UIViewLayer ViewLayer => UIViewLayer.Tips;
        public override Action EmptyCloseAction => DoViewClose;

        public override void OnViewOpen(UIViewParam param)
        {
            base.OnViewOpen(param);
            _param = param as FishRodSelectParam;
            _closeButton.onClick.AddListener(DoViewClose);

            _font = _coinText != null ? _coinText.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            BuildRodList();
            RefreshCoinDisplay();
        }

        public override async Task OnViewClose()
        {
            _closeButton.onClick.RemoveListener(DoViewClose);
            var onClosed = _param?.OnPopupClosed;
            _param = null;
            _rows.Clear();
            await base.OnViewClose();
            onClosed?.Invoke();
        }

        private void BuildRodList()
        {
            var configs = FishGameConfigManager.Instance?.RodConfigs;
            if (configs == null || _content == null)
            {
                return;
            }

            for (var i = 0; i < configs.Count; i++)
            {
                var row = CreateRodRow(configs[i]);
                _rows.Add(row);
            }

            RefreshAllRows();
        }

        private void RefreshAllRows()
        {
            for (var i = 0; i < _rows.Count; i++)
            {
                RefreshRow(_rows[i]);
            }
        }

        private void RefreshRow(RodRowRef row)
        {
            var rod = row.Config;
            var isOwned = _param.OwnedRodIds.Contains(rod.RodId);
            var isEquipped = rod.RodId == _param.CurrentRodId;

            if (isEquipped)
            {
                SetButtonState(row.ActionButton, row.ActionLabel, "已装备", EquippedBtnColor, false);
                row.AccentBar.color = AccentColor;
            }
            else if (isOwned)
            {
                SetButtonState(row.ActionButton, row.ActionLabel, "装备", EquipBtnColor, true);
                row.AccentBar.color = new Color(AccentColor.r, AccentColor.g, AccentColor.b, 0.15f);
            }
            else
            {
                var coins = CurrencyModel.Instance.GetCurrencyAmount(CurrencyType.Coin);
                var canAfford = rod.BuyCostCoin <= 0 || coins >= rod.BuyCostCoin;
                var label = rod.BuyCostCoin <= 0 ? "免费领取" : $"购买 {rod.BuyCostCoin} Coin";
                SetButtonState(row.ActionButton, row.ActionLabel, label, canAfford ? BuyBtnColor : DisabledBtnColor, canAfford);
                row.AccentBar.color = new Color(AccentColor.r, AccentColor.g, AccentColor.b, 0.05f);
            }

            if (row.ErrorText != null)
            {
                row.ErrorText.text = string.Empty;
            }
        }

        private void RefreshCoinDisplay()
        {
            if (_coinText != null)
            {
                var coins = CurrencyModel.Instance.GetCurrencyAmount(CurrencyType.Coin);
                _coinText.text = $"金币: {coins}";
            }
        }

        private void OnActionClicked(RodRowRef row)
        {
            if (_param == null)
            {
                return;
            }

            var rod = row.Config;
            var isOwned = _param.OwnedRodIds.Contains(rod.RodId);
            var isEquipped = rod.RodId == _param.CurrentRodId;

            if (isEquipped)
            {
                return;
            }

            if (isOwned)
            {
                _param.CurrentRodId = rod.RodId;
                _param.OnRodEquipped?.Invoke(rod.RodId);
                RefreshAllRows();
                return;
            }

            var result = _param.OnRodPurchased?.Invoke(rod.RodId) ?? RodPurchaseResult.ConfigError;
            if (result == RodPurchaseResult.Success)
            {
                _param.OwnedRodIds.Add(rod.RodId);
                _param.CurrentRodId = rod.RodId;
                _param.OnRodEquipped?.Invoke(rod.RodId);
                RefreshCoinDisplay();
                RefreshAllRows();
            }
            else if (row.ErrorText != null)
            {
                row.ErrorText.text = result switch
                {
                    RodPurchaseResult.NotEnoughCoin => "金币不足",
                    RodPurchaseResult.AlreadyOwned => "已拥有该鱼竿",
                    _ => "购买失败"
                };
            }
        }

        private RodRowRef CreateRodRow(FishRodConfig rod)
        {
            var rowObj = new GameObject($"Rod_{rod.RodId}", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            rowObj.transform.SetParent(_content, false);
            rowObj.layer = gameObject.layer;

            var rowRect = rowObj.GetComponent<RectTransform>();
            rowRect.sizeDelta = new Vector2(0f, 110f);

            var rowImage = rowObj.GetComponent<Image>();
            rowImage.color = CardInnerColor;

            var layout = rowObj.GetComponent<LayoutElement>();
            layout.preferredHeight = 110f;
            layout.flexibleWidth = 1f;

            var accentBar = CreateImage("AccentBar", rowObj.transform, AccentColor);
            var accentRect = accentBar.GetComponent<RectTransform>();
            accentRect.anchorMin = new Vector2(0f, 0f);
            accentRect.anchorMax = new Vector2(0f, 1f);
            accentRect.pivot = new Vector2(0f, 0.5f);
            accentRect.anchoredPosition = Vector2.zero;
            accentRect.sizeDelta = new Vector2(4f, 0f);

            var nameText = CreateText("RodName", rowObj.transform, rod.Name, 22, FontStyle.Bold, Color.white);
            SetRect(nameText, new Vector2(16f, -10f), new Vector2(400f, 30f), new Vector2(0f, 1f));

            var statsStr = $"线强 {rod.LineStrength:F0}  |  收线 {rod.ReelSpeed:F1}  |  控制 {rod.ControlPower:F1}  |  压制 {rod.SuppressPower:F1}";
            var statsText = CreateText("Stats", rowObj.transform, statsStr, 16, FontStyle.Normal, MutedTextColor);
            SetRect(statsText, new Vector2(16f, -44f), new Vector2(500f, 22f), new Vector2(0f, 1f));

            var baitTypeStr = (rod.SupportedBaitTypeMask & FishBaitTypeMask.Lure) != 0 ? "消耗品+拟饵" : "消耗品";
            var baitInfoStr = $"鱼饵: {baitTypeStr}  |  品质上限 {rod.SupportedBaitQualityMax}  |  推荐鱼等级 ≤{rod.RecommendFishLevelMax}";
            var baitText = CreateText("BaitInfo", rowObj.transform, baitInfoStr, 14, FontStyle.Normal, AccentColor);
            SetRect(baitText, new Vector2(16f, -68f), new Vector2(500f, 20f), new Vector2(0f, 1f));

            var btnObj = new GameObject("ActionButton", typeof(RectTransform), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(rowObj.transform, false);
            btnObj.layer = gameObject.layer;
            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(1f, 0.5f);
            btnRect.anchorMax = new Vector2(1f, 0.5f);
            btnRect.pivot = new Vector2(1f, 0.5f);
            btnRect.anchoredPosition = new Vector2(-16f, 0f);
            btnRect.sizeDelta = new Vector2(160f, 44f);

            var btnImage = btnObj.GetComponent<Image>();
            btnImage.color = EquipBtnColor;

            var btnLabelObj = new GameObject("Label", typeof(RectTransform), typeof(Text));
            btnLabelObj.transform.SetParent(btnObj.transform, false);
            btnLabelObj.layer = gameObject.layer;
            var labelRect = btnLabelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            var btnLabel = btnLabelObj.GetComponent<Text>();
            btnLabel.font = _font;
            btnLabel.fontSize = 18;
            btnLabel.alignment = TextAnchor.MiddleCenter;
            btnLabel.fontStyle = FontStyle.Bold;
            btnLabel.color = Color.white;

            var actionButton = btnObj.GetComponent<Button>();

            var errorObj = CreateText("ErrorText", rowObj.transform, string.Empty, 14, FontStyle.Normal, ErrorColor);
            SetRect(errorObj, new Vector2(-16f, -90f), new Vector2(160f, 18f), new Vector2(1f, 1f));
            errorObj.alignment = TextAnchor.MiddleRight;

            var rowRef = new RodRowRef
            {
                Config = rod,
                AccentBar = accentBar,
                ActionButton = actionButton,
                ActionLabel = btnLabel,
                ErrorText = errorObj
            };

            actionButton.onClick.AddListener(() => OnActionClicked(rowRef));

            return rowRef;
        }

        private static void SetButtonState(Button button, Text label, string text, Color bgColor, bool interactable)
        {
            label.text = text;
            button.interactable = interactable;
            var img = button.GetComponent<Image>();
            if (img != null)
            {
                img.color = bgColor;
            }

            label.color = interactable ? Color.white : new Color(1f, 1f, 1f, 0.5f);
        }

        private Image CreateImage(string name, Transform parent, Color color)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            obj.layer = gameObject.layer;
            var img = obj.GetComponent<Image>();
            img.color = color;
            img.raycastTarget = false;
            return img;
        }

        private Text CreateText(string name, Transform parent, string value, int fontSize, FontStyle style, Color color)
        {
            var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(parent, false);
            obj.layer = gameObject.layer;
            var text = obj.GetComponent<Text>();
            text.font = _font;
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = color;
            text.alignment = TextAnchor.MiddleLeft;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            text.text = value;
            return text;
        }

        private static void SetRect(Text text, Vector2 anchoredPos, Vector2 size, Vector2 anchorPivot)
        {
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = anchorPivot;
            rect.anchorMax = anchorPivot;
            rect.pivot = anchorPivot;
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = size;
        }

        private sealed class RodRowRef
        {
            public FishRodConfig Config;
            public Image AccentBar;
            public Button ActionButton;
            public Text ActionLabel;
            public Text ErrorText;
        }
    }
}
