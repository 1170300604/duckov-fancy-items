using Duckov.Modding;
using FancyItems.Core;
using FancyItems.Localizations;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FancyItems
{
    /// <summary>
    ///     FancyItems Mod 入口类 - 游戏期望的 ModBehaviour 类
    /// </summary>
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private FancyItemsMod fancyItemsMod;

        private void OnEnable()
        {
            Debug.Log("[FancyItems] ModBehaviour 已启用");
            ModLocalization.Init();
            ModManager.OnModActivated += ModManager_OnModActivated;
            
            // 创建并初始化核心Mod组件
            fancyItemsMod = gameObject.AddComponent<FancyItemsMod>();
        }

        private void OnDisable()
        {
            Debug.Log("[FancyItems] ModBehaviour 已禁用");
            ModManager.OnModActivated -= ModManager_OnModActivated;
            ModLocalization.Clear();

            if (fancyItemsMod != null)
            {
                // 调用清理方法
                fancyItemsMod.CleanupMod();
                Destroy(fancyItemsMod);
                fancyItemsMod = null;
            }
        }

        private void OnDestroy() => OnDisable();

        private void ModManager_OnModActivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2)
        {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //(触发时机:此mod在ModSetting之前启用)检查启用的mod是否是ModSetting,是进行初始化
            //先从ModSetting中读取配置
            ModSetting.Init();
            AddUI();
        }
        protected override void OnAfterSetup()
        {
            //(触发时机:ModSetting在此mod之前启用)此mod，Setup后,尝试进行初始化
            if (!ModSettingAPI.Init(info)) return;
            //先从ModSetting中读取配置
            ModSetting.Init();
            AddUI();
        }

        private void AddUI()
        {
            ModSettingAPI.AddToggle("quality", ModLocalization.GetText("qualityToggle"), 
                ModSetting.EnableQualityVisuals, QualityUICallback);
            ModSettingAPI.AddToggle("time", ModLocalization.GetText("timeToggle"), 
                ModSetting.EnableSearchOptimization, TimeUICallback);
            ModSettingAPI.AddToggle("audio", ModLocalization.GetText("audioToggle"),
                ModSetting.EnableSoundEffects, ModSetting.SetAudio);

            if (ModSetting.EnableQualityVisuals)
                AddQualityUI();
            
            if (ModSetting.EnableSearchOptimization)
                AddTimeUI();
        }

        private void AddQualityUI()
        {
            var colors = new List<string> {
                ModLocalization.GetText("colorName0"),
                ModLocalization.GetText("colorName1"),
                ModLocalization.GetText("colorName2"),
                ModLocalization.GetText("colorName3"),
                ModLocalization.GetText("colorName4"),
                ModLocalization.GetText("colorName5"),
                ModLocalization.GetText("colorName6"),
                ModLocalization.GetText("colorName7"),
            };
            ModSettingAPI.AddDropdownList("lv0color", ModLocalization.GetText("qualityList0"), 
                colors, colors[ModSetting.QualityColor[0]], ModSetting.SetLv0Color);
            ModSettingAPI.AddDropdownList("lv1color", ModLocalization.GetText("qualityList1"), 
                colors, colors[ModSetting.QualityColor[1]], ModSetting.SetLv1Color);
            ModSettingAPI.AddDropdownList("lv2color", ModLocalization.GetText("qualityList2"), 
                colors, colors[ModSetting.QualityColor[2]], ModSetting.SetLv2Color);
            ModSettingAPI.AddDropdownList("lv3color", ModLocalization.GetText("qualityList3"), 
                colors, colors[ModSetting.QualityColor[3]], ModSetting.SetLv3Color);
            ModSettingAPI.AddDropdownList("lv4color", ModLocalization.GetText("qualityList4"), 
                colors, colors[ModSetting.QualityColor[4]], ModSetting.SetLv4Color);
            ModSettingAPI.AddDropdownList("lv5color", ModLocalization.GetText("qualityList5"), 
                colors, colors[ModSetting.QualityColor[5]], ModSetting.SetLv5Color);
            ModSettingAPI.AddDropdownList("lv6color", ModLocalization.GetText("qualityList6"), 
                colors, colors[ModSetting.QualityColor[6]], ModSetting.SetLv6Color);
            ModSettingAPI.AddGroup("ColorGroup", ModLocalization.GetText("qualityGroup") , new List<string> {
                "lv0color",
                "lv1color",
                "lv2color",
                "lv3color",
                "lv4color",
                "lv5color",
                "lv6color"
            });
        }
        private void QualityUICallback(bool value)
        {

            if (value)
            {
                AddQualityUI();
                ModSetting.SetQuality(value);
            }
            else
            {
                ModSettingAPI.RemoveUI("ColorGroup");
                ModSetting.ResetQualityOptimization();
            }
        }

        private void AddTimeUI()
        {
            ModSettingAPI.AddSlider("lv0time", ModLocalization.GetText("timeSlider0"), 
                ModSetting.SearchTimeOptimization[0], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv0Time);
            ModSettingAPI.AddSlider("lv1time", ModLocalization.GetText("timeSlider1"), 
                ModSetting.SearchTimeOptimization[1], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv1Time);
            ModSettingAPI.AddSlider("lv2time", ModLocalization.GetText("timeSlider2"), 
                ModSetting.SearchTimeOptimization[2], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv2Time);
            ModSettingAPI.AddSlider("lv3time", ModLocalization.GetText("timeSlider3"), 
                ModSetting.SearchTimeOptimization[3], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv3Time);
            ModSettingAPI.AddSlider("lv4time", ModLocalization.GetText("timeSlider4"), 
                ModSetting.SearchTimeOptimization[4], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv4Time);
            ModSettingAPI.AddSlider("lv5time", ModLocalization.GetText("timeSlider5"), 
                ModSetting.SearchTimeOptimization[5], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv5Time);
            ModSettingAPI.AddSlider("lv6time", ModLocalization.GetText("timeSlider6"), 
                ModSetting.SearchTimeOptimization[6], new Vector2(0.1f, 3.0f),
                ModSetting.SetLv6Time);
            ModSettingAPI.AddGroup("TimeGroup", ModLocalization.GetText("timeGroup"), new List<string> {
                "lv0time",
                "lv1time",
                "lv2time",
                "lv3time",
                "lv4time",
                "lv5time",
                "lv6time"
            });
        }
        private void TimeUICallback(bool value)
        {
            if (value)
            {
                AddTimeUI();
                ModSetting.SetTime(value);

            }
            else
            {
                ModSettingAPI.RemoveUI("TimeGroup");
                ModSetting.ResetSearchTimeOptimization();

            }
        }
    }
}