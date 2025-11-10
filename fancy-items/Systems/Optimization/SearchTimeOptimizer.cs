using ItemStatsSystem;
using UnityEngine;

namespace FancyItems.Systems.Optimization
{
    /// <summary>
    /// 搜索时间优化器
    /// 负责优化低级物品的搜索时间
    /// </summary>
    public static class SearchTimeOptimizer
    {

        /// <summary>
        /// 获取优化后的搜索时间
        /// </summary>
        /// <param name="item">物品</param>
        /// <returns>优化后的时间（秒），-1表示保持原时间</returns>
        public static float GetOptimizedInspectingTimeScale(Item item)
        {
            if (item == null) return 1f;

            var quality = item.Quality;
            if (quality < 0) quality = 0;
            if (quality > 6) quality = 6;
            
            var optimizedTimeScale = Core.ModSetting.SearchTimeOptimization[quality];
            
            return optimizedTimeScale;
        }

        /// <summary>
        /// 处理搜索时间优化
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="originalTime">原始时间</param>
        /// <returns>优化后的时间</returns>
        public static float ProcessSearchTimeOptimization(Item item, float originalTime)
        {
            if (!Core.ModSetting.EnableSearchOptimization) return originalTime;
            if (item == null) return originalTime;

            var optimizedTime = GetOptimizedInspectingTimeScale(item);

            // 如果返回-1，表示保持原始时间，不需要修改
            if (optimizedTime < 0)
            {
                return originalTime;
            }

            // 记录对比信息并应用优化 - 只记录被优化的品质(0、1、2)
            if (item.Quality <= 2)
            {
                LogOptimizationInfo(item, originalTime, optimizedTime);
            }

            return optimizedTime;
        }

        /// <summary>
        /// 记录优化信息
        /// </summary>
        private static void LogOptimizationInfo(Item item, float originalTime, float optimizedTime)
        {
            var itemName = item.DisplayName ?? "Unknown";
            var reductionPercent = (originalTime > 0) ?
                ((originalTime - optimizedTime) / originalTime * 100f) : 0f;

            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} 时间优化: {itemName} (品质{item.Quality}) " +
                     $"{originalTime:F1}s → {optimizedTime:F1}s " +
                     $"(减少{reductionPercent:F0}%)");
        }
    }
}