using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Linq;

namespace Hunter.Tools
{
    public class Lib
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">long 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(long d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (long)(time - startTime).TotalSeconds;
            return intResult;
        }

        /// <summary>
        /// 从区间内随机出N个不重复随机数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static List<long> RandomNumberFromRange(int num, long min, long max, System.Random rand = null)
        {
            List<long> numList = new List<long>();

            if (num <= 0 || min >= max)
                return numList;

            if (rand == null)
                rand = new System.Random(unchecked((int)DateTime.Now.Ticks));

            int dRange = ((int)(max - min))/num;
            for (int i = 0; i < num; i++)
            { 
                int randNum = rand.Next(dRange - 1);
                if(i == num - 1)
                    randNum = rand.Next((int)(max - (long)(i * dRange)));
                numList.Add((long)(randNum + i * dRange) + min);
            }
            return numList;
        }

        public static void Log(params string[] msgs)
        {
            Debug.Log(String.Join(" ", msgs));
        }

        /// <summary>
        /// List的深度复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="List">被复制的listg</param>
        /// <returns></returns>
        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }

        /// <summary>
        /// 根据权值随机获取多个索引值(不重复)
        /// </summary>
        /// <param name="values">权值队列</param>
        /// <param name="count">获取随机数个数</param>
        /// <returns>索引队列</returns>
        public static int[] GetIndexByWeight(int[] values, int count)
        {
            if (values.Length < count)
                return null;

            int[] flagArray = Enumerable.Repeat(-1, values.Length).ToArray();
            int[] result = new int[count];
            int totalValue = 0, tempValue = 0, resultCount = 0;
            do
            {
                totalValue = 0;
                tempValue = 0;
                for (int i = 0; i < values.Length; ++i)
                {
                    if(flagArray[i] < 0)
                        totalValue = totalValue + values[i];
                }

                int randomValue = UnityEngine.Random.Range(0, totalValue);
                
                for (int i = 0; i < values.Length; ++i)
                {
                    if (flagArray[i] > 0)
                        continue;

                    tempValue = tempValue + values[i];

                    if (randomValue < tempValue)
                    {
                        flagArray[i] = 1;
                        result[resultCount] = i;
                        ++resultCount;
                        //Debug.Log(string.Format("范围：{0}, 随机数：{1}", totalValue, randomValue));
                        break;
                    }
                }
            } while (resultCount < count);

            return result;
        }

        /// <summary>
        /// 提取字符串中的数值 如(10,12)
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns>返回list<int></returns>
        public static List<int> GetIntInString(string data)
        {
            List<int> resList = new List<int>();

            var result = Regex.Matches(data, @"\d+");
            foreach (var item in result)
            {
                resList.Add(int.Parse(item.ToString()));
            }

            return resList;
        }
        
        /// <summary>  
        /// 按字符串长度切分成数组  
        /// </summary>  
        public static void GetSubStringList(string str, int length, List<string> list)
        {
            string contentSub = "";
            string nextSub = "";
            byte[] content = System.Text.Encoding.Unicode.GetBytes(str);
            if (length > content.Length)
            {
                contentSub = str;
                if (contentSub != "")
                {
                    list.Add(contentSub);
                }
            }
            else
            {
                int index = 0;
                for (int i = 0; i < length * 2 && i < str.Length * 2; i++)
                {
                    if (i % 2 != 0 && content[i] == 0)
                    {
                        index++;
                    }
                }
                index += (int)Math.Floor((double)(length - index) / 2f); //汉字数*2+字符数=length
                if (index > str.Length)
                {
                    index = str.Length;
                }
                contentSub = str.Substring(0, index);
                nextSub = str.Substring(index);
                list.Add(contentSub);

                GetSubStringList(nextSub, length, list);
            }
        }
    }
}

