using System.Collections.Generic;

namespace FancyItems.Core {
    public static class ModSetting
    {
       
        public static bool EnableSoundEffects { get; private set; }
        public static bool EnableSearchOptimization { get; private set; }
        public static bool EnableQualityVisuals { get; private set; }

        public static float[] SearchTimeOptimization =  { 0.8f, 0.8f, 0.9f, 1.0f, 1.0f, 1.2f, 2.0f };
        
        private static Dictionary<string,int> d1 = new Dictionary<string,int>();
        public static int[] QualityColor =  { 1, 1, 2, 3, 4, 5, 6 };
        
        public static void SetAudio(bool value) => EnableSoundEffects = value;
        public static void SetTime(bool value) => EnableSearchOptimization = value;
        public static void SetQuality(bool value) => EnableQualityVisuals = value;
        
        public static void SetLv0Time(float value) => SearchTimeOptimization[0] = value;
        public static void SetLv1Time(float value) => SearchTimeOptimization[1] = value;
        public static void SetLv2Time(float value) => SearchTimeOptimization[2] = value;
        public static void SetLv3Time(float value) => SearchTimeOptimization[3] = value;
        public static void SetLv4Time(float value) => SearchTimeOptimization[4] = value;
        public static void SetLv5Time(float value) => SearchTimeOptimization[5] = value;
        public static void SetLv6Time(float value) => SearchTimeOptimization[6] = value;
        
        public static void SetLv0Color(string value) => QualityColor[0] = d1[value];
        public static void SetLv1Color(string value) => QualityColor[1] = d1[value];
        public static void SetLv2Color(string value) => QualityColor[2] = d1[value];
        public static void SetLv3Color(string value) => QualityColor[3] = d1[value];
        public static void SetLv4Color(string value) => QualityColor[4] = d1[value];
        public static void SetLv5Color(string value) => QualityColor[5] = d1[value];
        public static void SetLv6Color(string value) => QualityColor[6] = d1[value];
        

        public static void Init()
        {
            if (ModSettingAPI.HasConfig())
            {
                d1.Add("无（原版）", 0);
                d1.Add("透明灰色", 1);
                d1.Add("柔和浅绿", 2);
                d1.Add("天蓝浅色", 3);
                d1.Add("亮浅紫", 4);
                d1.Add("柔亮橙", 5);
                d1.Add("明亮红", 6);
                d1.Add("透明浅白", 7);
                
                EnableSoundEffects = !ModSettingAPI.GetSavedValue("audio", out bool t1)|| t1;
                EnableSearchOptimization = !ModSettingAPI.GetSavedValue("time", out bool t2) || t2;
                EnableQualityVisuals = !ModSettingAPI.GetSavedValue("quality", out bool t3)|| t3;
                
                SearchTimeOptimization[0]=ModSettingAPI.GetSavedValue("lv0time", out float lv0 ) ? lv0 : 0.8f;
                SearchTimeOptimization[1]=ModSettingAPI.GetSavedValue("lv1time", out float lv1 ) ? lv1 : 0.8f;
                SearchTimeOptimization[2]=ModSettingAPI.GetSavedValue("lv2time", out float lv2 ) ? lv2 : 0.9f;
                SearchTimeOptimization[3]=ModSettingAPI.GetSavedValue("lv3time", out float lv3 ) ? lv3 : 1.0f;
                SearchTimeOptimization[4]=ModSettingAPI.GetSavedValue("lv4time", out float lv4 ) ? lv4 : 1.0f;
                SearchTimeOptimization[5]=ModSettingAPI.GetSavedValue("lv5time", out float lv5 ) ? lv5 : 1.0f;
                SearchTimeOptimization[6]=ModSettingAPI.GetSavedValue("lv6time", out float lv6 ) ? lv6 : 1.0f;
                
                QualityColor[0]=ModSettingAPI.GetSavedValue("lv0color", out string v0) ? d1[v0] : 1;
                QualityColor[1]=ModSettingAPI.GetSavedValue("lv1color", out string v1) ? d1[v1] : 1;
                QualityColor[2]=ModSettingAPI.GetSavedValue("lv2color", out string v2) ? d1[v2] : 2;
                QualityColor[3]=ModSettingAPI.GetSavedValue("lv3color", out string v3) ? d1[v3] : 3;
                QualityColor[4]=ModSettingAPI.GetSavedValue("lv4color", out string v4) ? d1[v4] : 4;
                QualityColor[5]=ModSettingAPI.GetSavedValue("lv5color", out string v5) ? d1[v5] : 5;
                QualityColor[6]=ModSettingAPI.GetSavedValue("lv6color", out string v6) ? d1[v6] : 6;
            }
            else
            {
                //设置默认值
                EnableSoundEffects = true;
                EnableSearchOptimization = true;
                EnableQualityVisuals = true;
            }
        }

        public static void ResetSearchTimeOptimization()
        {
            SearchTimeOptimization = new[] { 0.8f, 0.8f, 0.9f, 1.0f, 1.0f, 1.2f, 2.0f };
        }

        public static void ResetQualityOptimization()
        {
            QualityColor = new[] { 1, 1, 2, 3, 4, 5, 6 };
        }
    }
}
