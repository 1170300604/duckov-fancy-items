using Duckov.UI;
using Duckov.Utilities;
using FancyItems.Constants;
using FancyItems.Core;
using FancyItems.Systems.Audio;
using FancyItems.Systems.Optimization;
using FancyItems.Systems.UI;
using HarmonyLib;
using ItemStatsSystem;
using UnityEngine;

namespace FancyItems.Patches
{
    /// <summary>
    ///     ItemDisplay 相关的 Harmony 补丁
    /// </summary>
    public static class ItemDisplayPatches
    {

        public static int ProcessExistingItemDisplays()
        {
            ItemDisplay[]? displays = Object.FindObjectsOfType<ItemDisplay>();
            var processed = 0;

            foreach (var display in displays)
            {
                if (display == null) continue;

                // 添加视觉效果组件
                if (ModSetting.EnableQualityVisuals)
                {
                    if (display.GetComponent<ItemQualityVisualizer>() == null)
                    {
                        display.gameObject.AddComponent<ItemQualityVisualizer>();
                        processed++;
                    }
                }

                // 注释掉重复的音效触发器 - 音效功能已整合到 ItemQualityVisualizer 中
                // 移除 ItemSoundTrigger 以避免音效重复播放
                /*
                // 添加独立的音效触发器组件
                if (Core.ModConfiguration.EnableSoundEffects)
                {
                    if (display.GetComponent<Systems.Audio.ItemSoundTrigger>() == null)
                    {
                        display.gameObject.AddComponent<Systems.Audio.ItemSoundTrigger>();
                        processed++;
                    }
                }
                */
            }
            return processed;
        }

        public static int CleanupAllQualityVisualizers()
        {
            var cleanedCount = 0;

            // 清理视觉效果组件
            ItemQualityVisualizer[]? visualizers = Object.FindObjectsOfType<ItemQualityVisualizer>();
            foreach (var visualizer in visualizers)
            {
                if (visualizer != null) Object.Destroy(visualizer);
                cleanedCount++;
            }

            // 清理音效触发器组件（清理已废弃的组件）
            ItemSoundTrigger[] soundTriggers = Object.FindObjectsOfType<ItemSoundTrigger>();
            foreach (var soundTrigger in soundTriggers)
            {
                if (soundTrigger != null) Object.Destroy(soundTrigger);
                cleanedCount++;
            }

            return cleanedCount;
        }

        [HarmonyPatch(typeof(ItemDisplay), "OnEnable")]
        public static class ItemDisplayOnEnablePatch
        {
            private static void Postfix(ItemDisplay __instance)
            {
                // 添加视觉效果组件
                if (ModSetting.EnableQualityVisuals)
                {
                    if (__instance != null && __instance.gameObject != null &&
                        __instance.GetComponent<ItemQualityVisualizer>() == null)
                    {
                        __instance.gameObject.AddComponent<ItemQualityVisualizer>();
                    }
                }

                // 注释掉重复的音效触发器 - 音效功能已整合到 ItemQualityVisualizer 中
                // 移除 ItemSoundTrigger 以避免音效重复播放
                /*
                // 添加独立的音效触发器组件
                if (Core.ModConfiguration.EnableSoundEffects)
                {
                    if (__instance != null && __instance.gameObject != null &&
                        __instance.GetComponent<Systems.Audio.ItemSoundTrigger>() == null)
                    {
                        __instance.gameObject.AddComponent<Systems.Audio.ItemSoundTrigger>();
                    }
                }
                */
            }
        }
    }

    /// <summary>
    ///     搜索时间优化相关的 Harmony 补丁
    /// </summary>
    public static class LootTimePatches
    {
        [HarmonyPatch(typeof(GameplayDataSettings.LootingData), "MGetInspectingTime")]
        public static class LootingDataMGetInspectingTimePatch
        {
            private static void Postfix(Item item, ref float __result)
            {
                if (!ModSetting.EnableSearchOptimization) return;

                // 在原始方法执行后，获取原始结果
                var originalTime = __result;

                // 使用我们的优化时间倍率计算
                var optimizedTimeScale = SearchTimeOptimizer.GetOptimizedInspectingTimeScale(item);

#if DEBUG
                // 记录对比信息 - 只记录被优化的品质(0、1、2)
                if (item != null && !Mathf.Approximately(optimizedTimeScale, 1f))
                {
                    var itemName = item.DisplayName ?? "Unknown";
                    var reduction = originalTime > 0 ? (1 - optimizedTimeScale) * originalTime : 0f;

                    Debug.Log($"{FancyItemsConstants.LogPrefix} 时间优化: {itemName} (品质{item.Quality}) " +
                              $"{originalTime:F1}s → {optimizedTimeScale * __result:F1}s " +
                              $"(减少{reduction:F0}s)");
                }
#endif
                // 替换原始结果
                __result = optimizedTimeScale * __result;
            }
        }
    }
}