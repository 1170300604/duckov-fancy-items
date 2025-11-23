using FancyItems.Constants;
using FancyItems.Patches;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace FancyItems.Core
{
    /// <summary>
    ///     FancyItems Mod 主入口类
    /// </summary>
    public class FancyItemsMod : Duckov.Modding.ModBehaviour
    {
        private Harmony harmony;
        private bool initialized;


        private void OnEnable()
        {
#if DEBUG
            Debug.Log($"{FancyItemsConstants.LogPrefix} Mod已启用 - ModSetting设置界面版本");
#endif
            if (!initialized)
            {
                StartCoroutine(InitializeMod());
            }
        }


        private void OnDisable()
        {
            Debug.Log($"{FancyItemsConstants.LogPrefix} Mod已禁用");

            CleanupMod();
        }

        private void OnDestroy() => CleanupMod();

        private IEnumerator InitializeMod()
        {
            if (initialized) yield break;

            // 初始化Harmony
            harmony = new Harmony(FancyItemsConstants.HarmonyId);
            harmony.PatchAll();
#if DEBUG
            Debug.Log($"{FancyItemsConstants.LogPrefix} Harmony补丁应用成功");
#endif
            yield return null;

            // 处理现有对象
            var processed = ItemDisplayPatches.ProcessExistingItemDisplays();
#if DEBUG
            Debug.Log($"{FancyItemsConstants.LogPrefix} 处理了 {processed} 个现有ItemDisplay对象");
#endif
            yield return null;

            initialized = true;
#if DEBUG
            Debug.Log($"{FancyItemsConstants.LogPrefix} 初始化完成！");
#endif
        }

        public void CleanupMod()
        {
            StopAllCoroutines();

            harmony?.UnpatchAll(FancyItemsConstants.HarmonyId);

            var cleaned = ItemDisplayPatches.CleanupAllQualityVisualizers();
            if (cleaned > 0)
            {
                Debug.Log($"{FancyItemsConstants.LogPrefix} 清理了 {cleaned} 个品质可视化组件");
            }
        }
    }
}