using FancyItems.Constants;
using Newtonsoft.Json;
using SodaCraft.Localizations;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FancyItems.Localizations
{
    public static class ModLocalization
    {
        private static readonly Dictionary<string, string> DefaultDict = new Dictionary<string, string> {
            { "qualityToggle", "是否启用品质视觉效果" }, 
            { "timeToggle", "是否启用搜索时间乘数" }, 
            { "audioToggle", "是否启用音效" }, 
            { "qualityGroup", "物品颜色设置(修改后需要重启)" }, 
            { "qualityList0", "垃圾物品颜色" }, 
            { "qualityList1", "普通物品颜色" }, 
            { "qualityList2", "优良物品颜色" }, 
            { "qualityList3", "精良物品颜色" }, 
            { "qualityList4", "史诗物品颜色" }, 
            { "qualityList5", "传说物品颜色" }, 
            { "qualityList6", "神话物品颜色" }, 
            { "timeGroup", "搜索时间设置" }, 
            { "timeSlider0", "垃圾物品搜索时间乘数" }, 
            { "timeSlider1", "普通物品搜索时间乘数" }, 
            { "timeSlider2", "优良物品搜索时间乘数" }, 
            { "timeSlider3", "精良物品搜索时间乘数" }, 
            { "timeSlider4", "史诗物品搜索时间乘数" }, 
            { "timeSlider5", "传说物品搜索时间乘数" }, 
            { "timeSlider6", "神话物品搜索时间乘数" }, 
            { "colorName0", "无（原版）" }, 
            { "colorName1", "透明灰色" }, 
            { "colorName2", "柔和浅绿" }, 
            { "colorName3", "天蓝浅色" }, 
            { "colorName4", "亮浅紫" }, 
            { "colorName5", "柔亮橙" }, 
            { "colorName6", "明亮红" }, 
            { "colorName7", "透明浅白" }
        };

        private static Dictionary<SystemLanguage, Dictionary<string, string>> languagePack;
        private static SystemLanguage CurrentLanguage => LocalizationManager.CurrentLanguage;
        public static void Init() => languagePack = new Dictionary<SystemLanguage, Dictionary<string, string>> {
            [SystemLanguage.ChineseSimplified] =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FancyItemsConstants.DLLPath+"/lang/zh_CN.json"))??DefaultDict,
            [SystemLanguage.English] = 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FancyItemsConstants.DLLPath+"/lang/en.json"))??DefaultDict,
            [SystemLanguage.Japanese] = 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FancyItemsConstants.DLLPath+"/lang/ja.json"))??DefaultDict,
            [SystemLanguage.Korean] = 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FancyItemsConstants.DLLPath+"/lang/kr.json"))??DefaultDict,
            [SystemLanguage.Russian] = 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FancyItemsConstants.DLLPath+"/lang/ru.json"))??DefaultDict
        };

        public static void Clear() => languagePack.Clear();

        public static string GetText(string key) => GetText(CurrentLanguage, key);

        private static string GetText(SystemLanguage language, string key) =>
            !languagePack.TryGetValue(language, out Dictionary<string, string>? dictionary) ? key : dictionary.GetValueOrDefault(key, key);
    }

}