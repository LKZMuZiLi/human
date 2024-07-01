using System.Data;
using UnityEngine;

namespace LKZ.Utilitys
{
    internal static class DataSave
    {
        private const string VoiceTTSID_Key = "VoiceTTSID";
        private const string RoleSetting_Key = "RoleSetting";

        public static int GetVoiceID()
        {
            if (!PlayerPrefs.HasKey(VoiceTTSID_Key))
            {
                return 1;
            }

            return PlayerPrefs.GetInt(VoiceTTSID_Key);
        }

        /// <summary>
        /// 设置音色id
        /// </summary>
        /// <param name="id"></param>
        public static void SetVoiceID(int id)
        {
            PlayerPrefs.SetInt(VoiceTTSID_Key, id);
        }


        public static bool GetRoleSetting(out string data)
        {
            var has = PlayerPrefs.HasKey(RoleSetting_Key);
            if (!has)
            {
                data = "你是木子李，你的职业是程序员，你来自湛江，你现在住广州，你会Unity游戏引擎、CSharp、数据结构以算法、编程思维和逻辑思维，你是一名男生，你出生于1999年，你的身高是175cm，体重是115斤，你的性格特点：智慧、睿智、耐心，善于解答问题_你是木子李，你的职业是程序员";

            }
            else
                data = PlayerPrefs.GetString(RoleSetting_Key);
            return has;
        }

        public static void SetRoleSetting(string role)
        {
            PlayerPrefs.SetString(RoleSetting_Key, role);
        }

        public static void DeleteRoleSetting()
        {
            PlayerPrefs.DeleteKey(RoleSetting_Key );

        }
    }
}
