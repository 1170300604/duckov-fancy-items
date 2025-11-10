using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace FancyItems.Core
{
    /// <summary>
    /// FancyItems Mod 主入口类
    /// </summary>
    public class FancyItemsMod : Duckov.Modding.ModBehaviour
    {
        private bool initialized = false;
        private Harmony harmony;
       

        private void OnEnable()
        {
#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} Mod已启用 - ModSetting设置界面版本");
#endif
            if (!initialized)
            {
                StartCoroutine(InitializeMod());
            }
        }

        private IEnumerator InitializeMod()
        {
            if (initialized) yield break;

            // 初始化Harmony
            harmony = new Harmony(Constants.FancyItemsConstants.HarmonyId);
            harmony.PatchAll();
#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} Harmony补丁应用成功");
#endif
            yield return null;

            // 处理现有对象
            var processed = Patches.ItemDisplayPatches.ProcessExistingItemDisplays();
#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} 处理了 {processed} 个现有ItemDisplay对象");
#endif
            yield return null;

            initialized = true;
#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} 初始化完成！");
#endif
        }

        
        private void OnDisable()
        {
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} Mod已禁用");
            
            CleanupMod();
        }

        private void OnDestroy()
        {
            CleanupMod();
        }

        public void CleanupMod()
        {
            StopAllCoroutines();

            harmony?.UnpatchAll(Constants.FancyItemsConstants.HarmonyId);

            int cleaned = Patches.ItemDisplayPatches.CleanupAllQualityVisualizers();
            if (cleaned > 0)
            {
                Debug.Log($"{Constants.FancyItemsConstants.LogPrefix} 清理了 {cleaned} 个品质可视化组件");
            }
        }

        
    }
}