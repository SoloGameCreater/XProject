using UnityEngine;

namespace FishGameRuntime
{
    /// <summary>
    /// FishGame 全局 UI 颜色定义，集中管理以避免散落的 magic color。
    /// </summary>
    public static class FishGameColors
    {
        // ── 状态徽章 ──
        public static readonly Color StateCasting = new(1f, 0.84f, 0.35f);
        public static readonly Color StateWaiting = new(0.72f, 0.9f, 1f);
        public static readonly Color StateFighting = new(0.45f, 1f, 0.55f);
        public static readonly Color StateAutoRetrieve = new(1f, 0.55f, 0.35f);

        // ── 张力条 ──
        public static readonly Color TensionLow = new(0.31f, 0.78f, 0.31f);
        public static readonly Color TensionMid = new(0.95f, 0.72f, 0.12f);
        public static readonly Color TensionHigh = new(0.86f, 0.26f, 0.26f);
        public static readonly Color TensionBurstFlashA = new(1f, 0.78f, 0.1f);

        // ── 体力条 ──
        public static readonly Color StaminaBurst = new(1f, 0.55f, 0.2f);
        public static readonly Color StaminaFatigue = new(0.31f, 0.78f, 0.31f);
        public static readonly Color StaminaSteady = new(0.95f, 0.72f, 0.12f);

        // ── 鱼线条 ──
        public static readonly Color LineNormal = new(0.31f, 0.63f, 1f);

        // ── 稀有度 ──
        public static readonly Color RarityLegendary = new(0.76f, 0.47f, 1f);
        public static readonly Color RarityRare = new(1f, 0.8f, 0.25f);
        public static readonly Color RarityUncommon = new(0.53f, 0.92f, 0.63f);
        public static readonly Color RarityCommon = Color.white;

        // ── 投掷区域 ──
        public static readonly Color CastZoneFill = new(0.20f, 0.72f, 0.86f, 0.05f);
        public static readonly Color CastZoneOutline = new(0.42f, 0.84f, 0.98f, 0.22f);
        public static readonly Color CastZoneLabel = new(0.88f, 0.95f, 0.98f, 1f);
        public static readonly Color CastZoneLabelCharged = new(1f, 0.92f, 0.45f, 1f);

        // ── 鱼竿选择弹窗 ──
        public static readonly Color RodCardInner = new(0.10f, 0.20f, 0.27f, 0.78f);
        public static readonly Color RodAccent = new(0.20f, 0.72f, 0.86f, 0.92f);
        public static readonly Color RodMutedText = new(0.72f, 0.83f, 0.88f, 1f);
        public static readonly Color RodEquippedBtn = new(0.15f, 0.49f, 0.24f);
        public static readonly Color RodEquipBtn = new(0.18f, 0.56f, 0.67f);
        public static readonly Color RodBuyBtn = new(0.76f, 0.46f, 0.14f);
        public static readonly Color RodDisabledBtn = new(0.3f, 0.3f, 0.3f, 0.6f);
        public static readonly Color RodError = new(1f, 0.45f, 0.45f);

        public static Color GetRarityColor(string rarity)
        {
            return rarity switch
            {
                "legendary" => RarityLegendary,
                "rare" => RarityRare,
                "uncommon" => RarityUncommon,
                _ => RarityCommon
            };
        }
    }
}
