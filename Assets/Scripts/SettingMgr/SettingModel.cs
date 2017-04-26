/******************************************************************************** 
** Copyright(c) ixinyou All Rights Reserved. 
** auth： HHao	
** date： 9/29/2016
** desc： 基础配置文件类
** Ver : V1.0.0 
*********************************************************************************/
using Basic.ResourceMgr;
using Hunter.GameCore;
using Hunter.Utility;

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Hunter.SettingMgr
{
    public interface ISettingModel<T, U> where T : class, U, new()
    {
        void LoadSetting(string key);
        U GetItem(string key);
        Dictionary<string, T>.KeyCollection GetAllKey();
    }

    public class SettingModel<T, U> : ISettingModel<T, U> where T : class, U, new()
    {
        #region 数据属性
        protected IResourceMgr resourceMgr;
        protected string filePath;
        protected Dictionary<string, T> settingData;
        #endregion

        #region 功能方法
        protected Dictionary<string, T> GetSetting<T>(string key) where T : class, new()
        {
            string settingContent = resourceMgr.Load<TextAsset>(filePath).text;

            Dictionary<string, T> settingData = FileUtility.GetCsvData<T>(key, settingContent);

            return settingData;
        }

        public void LoadSetting(string key)
        {
            settingData = GetSetting<T>(key);
        }

        public U GetItem(string key)
        {
            T bSetting;
            if (settingData.TryGetValue(key, out bSetting))
            {
                return (U)bSetting;
            }
            else
                return default(U);
        }

        public Dictionary<string, T>.KeyCollection GetAllKey()
        {
            return settingData.Keys;
        }

        public bool ContainsKey(string key)
        {
            return settingData.ContainsKey(key);
        }
        #endregion
    }
}