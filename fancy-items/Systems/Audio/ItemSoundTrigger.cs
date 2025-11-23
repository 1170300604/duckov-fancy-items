using Duckov.UI;
using FancyItems.Core;
using ItemStatsSystem;
using System.Collections;
using UnityEngine;

namespace FancyItems.Systems.Audio
{
    /// <summary>
    ///     独立的物品音效触发器
    ///     不依赖于视觉效果，独立处理音效播放
    /// </summary>
    public class ItemSoundTrigger : MonoBehaviour
    {
        private Item currentItem;
        private ItemDisplay itemDisplay;
        private bool lastInspected;
        private Item lastItem;
        private bool soundPlayed;

        private void Awake() => itemDisplay = GetComponent<ItemDisplay>();

        private void Start() =>
            // 延迟初始化，确保所有组件都已加载
            StartCoroutine(DelayedInitialize());

        private void Update()
        {
            if (!ModSetting.EnableSoundEffects) return;

            if (itemDisplay)
            {
                currentItem = itemDisplay.Target;

                if (currentItem != lastItem)
                {
                    lastItem = currentItem;
                    // 保持与旧版本一致的逻辑：初始化时记录物品当前的检查状态
                    lastInspected = currentItem && currentItem.Inspected;
                    soundPlayed = false;
                }

                if (currentItem && currentItem.Inspected && !lastInspected && !soundPlayed)
                {
                    soundPlayed = true;
                    // 播放品质音效
                    QualitySoundManager.PlayQualitySound(currentItem.Quality);
                }

                lastInspected = currentItem?.Inspected ?? false;
            }
        }

        private void OnDestroy()
        {
            // 清理资源
        }

        private IEnumerator DelayedInitialize()
        {
            yield return null;
            // 确保组件已正确初始化
        }
    }
}