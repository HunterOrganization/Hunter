using UnityEngine;
using System.Collections;
using System;

namespace Hunter.Utility
{
    public static class TimeUtility
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}