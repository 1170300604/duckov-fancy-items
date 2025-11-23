using Duckov.UI;
using FancyItems.Constants;
using FancyItems.Systems.Audio;
using ItemStatsSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

namespace FancyItems.Systems.UI
{
    /// <summary>
    ///     物品品质可视化组件
    /// </summary>
    public class ItemQualityVisualizer : MonoBehaviour
    {
        private ProceduralImage background;
        private bool initialized;
        private bool isDirty = true;
        private ItemDisplay itemDisplay;
        private bool lastInspected;
        private Item lastItem;
        private int lastQuality = -1;
        private float nextUpdateTime;
        private UniformModifier roundedModifier;
        private bool soundPlayed;

        private void LateUpdate()
        {
            if (!initialized || itemDisplay == null || background == null) return;

            if (!isDirty && Time.time < nextUpdateTime) return;

            nextUpdateTime = Time.time + FancyItemsConstants.PerformanceUpdateInterval;
            isDirty = false;

            UpdateItemDisplay();
        }

        private void OnEnable()
        {
            if (!initialized)
            {
                StartCoroutine(DelayedInitialize());
            }
            else
            {
                isDirty = true;
            }
        }

        private void OnDestroy()
        {
            if (background != null) Destroy(background.gameObject);
        }

        private IEnumerator DelayedInitialize()
        {
            yield return null;
            if (!initialized)
            {
                itemDisplay = GetComponent<ItemDisplay>();
                if (itemDisplay != null)
                {
                    CreateBackground();
                    initialized = true;
                    isDirty = true;
                }
            }
        }

        private void CreateBackground()
        {
            if (background != null) return;

            // 创建背景GameObject
            GameObject bgObject = new GameObject("FancyItems_Background");
            background = bgObject.AddComponent<ProceduralImage>();

            // 添加UniformModifier用于圆角
            roundedModifier = bgObject.AddComponent<UniformModifier>();
            roundedModifier.Radius = FancyItemsConstants.BackgroundCornerRadius;

            // 添加LayoutElement并设置ignoreLayout，防止LayoutGroup干扰
            LayoutElement? layoutElement = bgObject.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;

            // 设置为当前ItemDisplay的子对象（使用false保留本地坐标）
            bgObject.transform.SetParent(transform, false);

            // 获取RectTransform并完全重置
            RectTransform? rect = bgObject.GetComponent<RectTransform>();

            // 重置所有transform属性
            rect.localPosition = Vector3.zero;
            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;

            // 设置锚点和pivot到左下角
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);

            // 清空所有offset
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

            // 将背景移到最下层（最先渲染）
            bgObject.transform.SetAsFirstSibling();

            background.color = Color.clear;

            // 禁用raycast，避免阻挡点击事件
            background.raycastTarget = false;
        }

        private void UpdateItemDisplay()
        {
            Item currentItem = itemDisplay.Target;
            if (currentItem != lastItem)
            {
                lastItem = currentItem;
                lastQuality = -1;
                lastInspected = currentItem != null ? currentItem.Inspected : false;
                soundPlayed = false;
            }

            if (currentItem == null)
            {
                if (background.gameObject.activeSelf) background.gameObject.SetActive(false);
                return;
            }

            if (currentItem.Inspected && !lastInspected && !soundPlayed)
            {
                soundPlayed = true;
                // 播放品质音效（独立于视觉效果开关）
                QualitySoundManager.PlayQualitySound(currentItem.Quality);
            }
            lastInspected = currentItem.Inspected;

            var isShopItem = itemDisplay.IsStockshopSample;
            if (!currentItem.Inspected && !isShopItem)
            {
                if (background.gameObject.activeSelf) background.gameObject.SetActive(false);
                return;
            }

            var quality = currentItem.Quality;
            if (quality != lastQuality)
            {
                lastQuality = quality;
                UpdateBackgroundColor(quality);
            }
        }

        private void UpdateBackgroundColor(int quality)
        {
            if (background == null) return;
            /*
            if (!QualityColorConfig.ShouldShowBackground(quality))
            {
                background.gameObject.SetActive(false);
                return;
            }*/
            Color? tmpColor = QualityColorConfig.GetQualityColor(quality);
            if (tmpColor.HasValue)
            {
                background.gameObject.SetActive(true);
                background.color = tmpColor.Value;
            }
            else
            {
                background.gameObject.SetActive(false);
            }

        }

        public void MarkDirty() => isDirty = true;
    }
}