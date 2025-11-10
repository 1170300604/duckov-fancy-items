using UnityEngine;

namespace FancyItems.Constants
{
    /// <summary>
    /// FancyItems Mod 全局常量定义
    /// </summary>
    public static class FancyItemsConstants
    {
        /// <summary>
        /// Harmony ID，用于补丁管理
        /// </summary>
        public const string HarmonyId = "com.fancyitems.mod";

        /// <summary>
        /// Mod日志前缀
        /// </summary>
        public const string LogPrefix = "[FancyItems]";

        /// <summary>
        /// 品质背景圆角半径（像素）
        /// </summary>
        public const float BackgroundCornerRadius = 15f;

        /// <summary>
        /// 性能优化更新间隔（秒）
        /// </summary>
        public const float PerformanceUpdateInterval = 0.1f;

        /// <summary>
        /// 品质颜色配置（RGBA）
        /// </summary>
        public static readonly Color[] QualityColors = new Color[]
        {
            new Color(1.0f, 0.75f, 0.2f, 1f),       // c0: 不使用
            new Color(0f, 0f, 0f, 0f),              // c1: 透明灰色
            new Color(0.6f, 0.9f, 0.6f, 0.24f),     // c2: 柔和浅绿
            new Color(0.6f, 0.8f, 1.0f, 0.30f),     // c3: 天蓝浅色
            new Color(1.0f, 0.50f, 1.0f, 0.40f),    // c4: 亮浅紫（提亮，略粉）
            new Color(1.0f, 0.75f, 0.2f, 0.60f),    // c5: 柔亮橙（更偏橙、更暖）
            new Color(1.0f, 0.3f, 0.3f, 0.4f),      // c6: 明亮红（亮度提升、透明度降低）
            new Color(1f, 1f, 1f, 0.3f),            // c7: 透明浅白
        };
    }
}