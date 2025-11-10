using Duckov.Modding;
using System.Collections.Generic;
using UnityEngine;

namespace FancyItems
{
    /// <summary>
    /// FancyItems Mod 入口类 - 游戏期望的 ModBehaviour 类
    /// </summary>
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private Core.FancyItemsMod fancyItemsMod;

        private void OnEnable()
        {
            Debug.Log("[FancyItems] ModBehaviour 已启用");
            ModManager.OnModActivated += ModManager_OnModActivated;
            // 创建并初始化核心Mod组件
            fancyItemsMod = gameObject.AddComponent<Core.FancyItemsMod>();
        }

        private void OnDisable()
        {
            Debug.Log("[FancyItems] ModBehaviour 已禁用");
            ModManager.OnModActivated -= ModManager_OnModActivated;

            if (fancyItemsMod != null)
            {
                // 调用清理方法
                fancyItemsMod.CleanupMod();
                Destroy(fancyItemsMod);
                fancyItemsMod = null;
            }
        }

        private void OnDestroy()
        {
            OnDisable();
        }
        
        private void ModManager_OnModActivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2)
        {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //(触发时机:此mod在ModSetting之前启用)检查启用的mod是否是ModSetting,是进行初始化
            //先从ModSetting中读取配置
            Core.ModSetting.Init();
            AddUI();
        }
        protected override void OnAfterSetup()
        {
            //(触发时机:ModSetting在此mod之前启用)此mod，Setup后,尝试进行初始化
            if (!ModSettingAPI.Init(info)) return;
            //先从ModSetting中读取配置
            Core.ModSetting.Init();
            AddUI();
        }

        private void AddUI()
        {
            ModSettingAPI.AddToggle("quality", "是否启用品质视觉效果", Core.ModSetting.EnableQualityVisuals, QualityUICallback);
            ModSettingAPI.AddToggle("time", "是否启用搜索时间乘数", Core.ModSetting.EnableSearchOptimization, TimeUICallback);
            ModSettingAPI.AddToggle("audio", "是否启用音效", Core.ModSetting.EnableSoundEffects, Core.ModSetting.SetAudio);
            
            if (Core.ModSetting.EnableQualityVisuals)
            {
                AddQualityUI();
            }
            if (Core.ModSetting.EnableSearchOptimization)
            {
                AddTimeUI();
            }
        }

        private void AddQualityUI()
        {
            var colors = new List<string> { "无（原版）","透明灰色", "柔和浅绿", "天蓝浅色", "亮浅紫", "柔亮橙", "明亮红","透明浅白" };
            ModSettingAPI.AddDropdownList("lv0color", "垃圾物品颜色", colors, colors[Core.ModSetting.QualityColor[0]],Core.ModSetting.SetLv0Color);
            ModSettingAPI.AddDropdownList("lv1color", "普通物品颜色", colors, colors[Core.ModSetting.QualityColor[1]],Core.ModSetting.SetLv1Color);
            ModSettingAPI.AddDropdownList("lv2color", "优良物品颜色", colors, colors[Core.ModSetting.QualityColor[2]],Core.ModSetting.SetLv2Color);
            ModSettingAPI.AddDropdownList("lv3color", "精良物品颜色", colors, colors[Core.ModSetting.QualityColor[3]],Core.ModSetting.SetLv3Color);
            ModSettingAPI.AddDropdownList("lv4color", "史诗物品颜色", colors, colors[Core.ModSetting.QualityColor[4]],Core.ModSetting.SetLv4Color);
            ModSettingAPI.AddDropdownList("lv5color", "传说物品颜色", colors, colors[Core.ModSetting.QualityColor[5]],Core.ModSetting.SetLv5Color);
            ModSettingAPI.AddDropdownList("lv6color", "神话物品颜色", colors, colors[Core.ModSetting.QualityColor[6]],Core.ModSetting.SetLv6Color);
            ModSettingAPI.AddGroup("ColorGroup", "物品颜色设置(修改后需要重启)",new List<string>{"lv0color","lv1color","lv2color","lv3color","lv4color","lv5color","lv6color"}, 
                0.7f, false, false);
        }
		private void QualityUICallback(bool value){

            if (value)
            {
                AddQualityUI();
                Core.ModSetting.SetQuality(value);
            }
            else
            {
                ModSettingAPI.RemoveUI("ColorGroup");
                Core.ModSetting.ResetQualityOptimization();
            }
		}

        private void AddTimeUI()
        {
            
#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix}before add:{Core.ModSetting.SearchTimeOptimization[0]},{Core.ModSetting.SearchTimeOptimization[1]}" +
                      $",{Core.ModSetting.SearchTimeOptimization[2]},{Core.ModSetting.SearchTimeOptimization[3]}" +
                      $",{Core.ModSetting.SearchTimeOptimization[4]},{Core.ModSetting.SearchTimeOptimization[5]}");      
#endif 
            ModSettingAPI.AddSlider("lv0time", "垃圾物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[0], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv0Time);
            ModSettingAPI.AddSlider("lv1time", "普通物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[1], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv1Time);
            ModSettingAPI.AddSlider("lv2time", "优良物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[2], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv2Time);
            ModSettingAPI.AddSlider("lv3time", "精良物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[3], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv3Time);
            ModSettingAPI.AddSlider("lv4time", "史诗物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[4], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv4Time);
            ModSettingAPI.AddSlider("lv5time", "传说物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[5], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv5Time);
            ModSettingAPI.AddSlider("lv6time", "神话物品搜索时间比例", Core.ModSetting.SearchTimeOptimization[6], new Vector2(0.1f, 3.0f),
                Core.ModSetting.SetLv6Time);
            ModSettingAPI.AddGroup("TimeGroup", "搜索时间设置",new List<string>{"lv0time","lv1time","lv2time","lv3time","lv4time","lv5time","lv6time"}, 
                0.7f, false, false);

#if DEBUG
            Debug.Log($"{Constants.FancyItemsConstants.LogPrefix}after add:{Core.ModSetting.SearchTimeOptimization[0]},{Core.ModSetting.SearchTimeOptimization[1]}" +
                      $",{Core.ModSetting.SearchTimeOptimization[2]},{Core.ModSetting.SearchTimeOptimization[3]}" +
                      $",{Core.ModSetting.SearchTimeOptimization[4]},{Core.ModSetting.SearchTimeOptimization[5]}");      
#endif 
        }
        private void TimeUICallback(bool value)
        {
            if (value)
            {
                AddTimeUI();
                Core.ModSetting.SetTime(value);

            }
            else
            {
#if DEBUG
                Debug.Log($"{Constants.FancyItemsConstants.LogPrefix}before remove:{Core.ModSetting.SearchTimeOptimization[0]},{Core.ModSetting.SearchTimeOptimization[1]}" +
                          $",{Core.ModSetting.SearchTimeOptimization[2]},{Core.ModSetting.SearchTimeOptimization[3]}" +
                          $",{Core.ModSetting.SearchTimeOptimization[4]},{Core.ModSetting.SearchTimeOptimization[5]}");      
#endif
                ModSettingAPI.RemoveUI("TimeGroup");
                Core.ModSetting.ResetSearchTimeOptimization();
#if DEBUG
                Debug.Log($"{Constants.FancyItemsConstants.LogPrefix}after remove:{Core.ModSetting.SearchTimeOptimization[0]},{Core.ModSetting.SearchTimeOptimization[1]}" +
                          $",{Core.ModSetting.SearchTimeOptimization[2]},{Core.ModSetting.SearchTimeOptimization[3]}" +
                          $",{Core.ModSetting.SearchTimeOptimization[4]},{Core.ModSetting.SearchTimeOptimization[5]}");      
#endif
            }
        }
    }
}