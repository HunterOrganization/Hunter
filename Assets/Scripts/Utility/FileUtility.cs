using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Reflection;
using LumenWorks.Framework.IO.Csv;

namespace Hunter.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// 获取可读可写目录文件的路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPersistentFilePath(string fileName)
        {
#if UNITY_EDITOR
            return fileName;
#elif UNITY_IOS || UNITY_ANDROID
            return Application.persistentDataPath + "/" + fileName;
#else
            return Application.dataPath + "/" + fileName;
#endif
        }  

        /// <summary>
        /// 得到数据库的连接描述
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string GetStreamingAssetsPath(string fileName)
        {
            string result = string.Empty;
#if UNITY_EDITOR
            result = string.Format("Assets/StreamingAssets/{0}.csv", fileName);
#elif UNITY_IOS
            result = string.Format("{0}/Raw/{1}.csv", Application.dataPath, fileName);
#elif UNITY_ANDROID
            result = string.Format("jar:file://{0}!/assets/{1}.csv", Application.dataPath, fileName);
#else
            result = string.Format("{0}/StreamingAssets/{1}.csv", Application.dataPath, fileName);
#endif
            return result;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPersistentDataPath(string fileName)
        {
            var filePath = string.Empty;
            filePath = string.Format("{0}/{1}.csv", Application.persistentDataPath, fileName);
            return filePath;
        }

        public static string GetFileContent(string filePath)
        {
            string allLine = File.ReadAllText(filePath);
            return allLine;
        }


        /// <summary>
        /// string 转其他类型
        /// </summary>
        /// <param name="pType">类型</param>
        /// <param name="data">string值</param>
        /// <returns></returns>
        public static object ConvertStringToObject(Type pType, string data)
        {
            object ConvertValue = default(object);
            if (pType.IsEnum)
            {
                if (Enum.IsDefined(pType, data))
                {
                    //Debug.Log("aaaaa");
                    ConvertValue = Enum.Parse(pType, data);
                }
                else
                {
                    //Debug.Log("bbbbb");
                    ConvertValue = null;
                    //Debug.Log(string.Format("<color=red>枚举值错误：CSV文件 {0},第{1}行，第{2}列,值错误！..该属性将默认使用相应枚举第一个值！</color>", filePath, i + 2, j));
                }
            }
            else
            {
                var PType = pType.ToString();
                switch (PType)
                {
                    case "System.String":
                        ConvertValue = data;
                        break;
                    case "System.Int32":
                        if (data == "null")
                        {

                        }
                        else
                        {
                            ConvertValue = int.Parse(data);
                        }
                        break;
                    case "System.Int64":
                        if (data == "null")
                        {

                        }
                        else
                        {
                            ConvertValue = long.Parse(data);
                        }
                        break;
                    case "System.Single":
                        if (data == "null")
                        {

                        }
                        else
                        {
                            ConvertValue = float.Parse(data);
                        }
                        break;
                    case "System.Double":
                        if (data == "null")
                        {

                        }
                        else
                        {
                            ConvertValue = double.Parse(data);
                        }
                        break;
                    case "System.Boolean":
                        int value;
                        if (int.TryParse((data), out value))
                        {
                            if (value == 1)
                                ConvertValue = true;
                            else if (value == 0)
                                ConvertValue = false;
                        }
                        else if (data.Equals("Yes"))
                            ConvertValue = true;
                        else if (data.Equals("No"))
                            ConvertValue = false;
                        else if (data.Equals("True"))
                            ConvertValue = true;
                        else if (data.Equals("False"))
                            ConvertValue = false;
                        else if (data.Equals("TRUE"))
                            ConvertValue = true;
                        else if (data.Equals("FALSE"))
                            ConvertValue = false;
                        else if (data.Equals("true"))
                            ConvertValue = true;
                        else if (data.Equals("false"))
                            ConvertValue = false;
                        break;
                    case "System.String[]":
                        string[] sSTemp = data.Split(',');
                        ConvertValue = sSTemp;
                        break;
                    case "System.Int32[]":
                        //Debug.Log("PropertyType:System.Int32[]");
                        string[] sITemp = data.Split(',');
                        int ilen = sITemp.Length;
                        int[] intTemp = new int[ilen];
                        //Debug.Log(ld[i][key].Length);
                        for (int iTemp = 0; iTemp < ilen; iTemp++)
                        {
                            //Debug.Log(ld[i][key][iTemp]);
                            if (sITemp[iTemp] == "null")
                            {

                            }
                            else
                            {
                                intTemp[iTemp] = int.Parse(sITemp[iTemp]);
                            }
                        }
                        ConvertValue = intTemp;
                        break;
                    case "System.Single[]":
                        //Debug.Log("PropertyType:System.Single[]");
                        string[] sFTemp = data.Split(',');
                        int flen = sFTemp.Length;
                        float[] floatTemp = new float[flen];
                        for (int iTemp = 0; iTemp < flen; iTemp++)
                        {
                            if (sFTemp[iTemp] == "null")
                            {

                            }
                            else
                            {
                                floatTemp[iTemp] = float.Parse(sFTemp[iTemp]);
                            }
                        }
                        ConvertValue = floatTemp;
                        break;
                    case "System.Double[]":
                        //Debug.Log("PropertyType:System.Double[]");
                        string[] sDTemp = data.Split(',');
                        int dlen = sDTemp.Length;
                        double[] doubleTemp = new double[dlen];
                        for (int iTemp = 0; iTemp < dlen; iTemp++)
                        {
                            if (sDTemp[iTemp] == "null")
                            {

                            }
                            else
                            {
                                doubleTemp[iTemp] = double.Parse(sDTemp[iTemp]);
                            }
                        }
                        ConvertValue = doubleTemp;;
                        break;
                    default:
                        break;
                }
            }
            return ConvertValue;
        }

        /// <summary>
        /// 读取csv内容
        /// </summary>
        /// <typeparam name="T">表格描述</typeparam>
        /// <param name="content">string 文本</param>
        /// <returns></returns>
        public static IList<T> ReadCSVFile<T>(string content)
        {

            List<T> lt = new List<T>();

            List<string[]> ls = ReadCsv(content);
            string[] headers = ls[0];
            int fieldCount = headers.Length;
            ls.RemoveAt(0);

            Type t = typeof(T);
            string PnameTemp="";
            for (int i = 0; i < ls.Count; i++)
            {
                object obj = Activator.CreateInstance(t);
                for (int j = 0; j < fieldCount; j++)
                {
                    var len = headers[j].IndexOf(":");
                    if (len == -1)
                    {
                        PnameTemp = headers[j];
                    }
                    else {
                        PnameTemp = headers[j].Substring(0,len);
                    }
                    PropertyInfo p = null;
                    p = t.GetProperty(PnameTemp);
                   
                    if (p != null)
                    {
                        p.SetValue(obj, ConvertStringToObject(p.PropertyType, ls[i][j]), null);
                        //StringTypeChange(obj,p,ls[i][j]);
                    }
                }
                lt.Add((T)obj);
            }        
            return lt;
            //return default(IList < T >);
        }

        #region CSV文件处理
        //ggh add
        /// <summary>
        /// 将CSV文件数据放到 List<string[]>中
        /// </summary>
        /// <param name="content">string 文本</param>
        /// <returns></returns>
        public static List<string[]> ReadCsv(string content)
        {
            List<string[]> ls = new List<string[]>();

            Stream stream = StringToStream(content);

            // open the file "data.csv" which is a CSV file with headers           
            using (CsvReader csv = new CsvReader(new StreamReader(stream), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                ls.Add(headers);
                while (csv.ReadNextRecord())
                {
                    string[] arrary = new string[fieldCount];
                    for (int i = 0; i < fieldCount; i++)
                    {
                        //Debug.Log(string.Format("{0} = {1};",headers[i], csv[i]));
                        //dic[headers[i]] = csv[i];
                        if (csv[i].Length==0)
                        {
                            arrary[i] = "null";
                        }
                        else
                        {
                            arrary[i] = csv[i];
                        }
                        
                    }
                    ls.Add(arrary);
                }

                return ls;
            }
        }
        /// <summary>
        /// string 转为 stream
        /// </summary>
        /// <param name="content">string文本</param>
        /// <returns></returns>
        public static Stream StringToStream(string content)
        {
            byte[] bytes=UTF8Encoding.UTF8.GetBytes(content);
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        /// <summary>
        /// string 类型 转为 其他类型
        /// </summary>
        /// <param name="obj">要赋值的类的实例</param>
        /// <param name="p">PropertyInfo类实例</param>
        /// <param name="stringValue">string类型值</param>
        public static void StringTypeChange(object obj, PropertyInfo p, string stringValue)
        {
            var pType = p.PropertyType;

            if (pType.IsEnum)
            {
                if (Enum.IsDefined(pType, stringValue))
                {
                    object enumValue = Enum.Parse(pType, stringValue);
                    p.SetValue(obj, enumValue, null);
                }
                else
                {
                    p.SetValue(obj, null, null);
                    //Debug.Log(string.Format("<color=red>枚举值错误：CSV文件 {0},第{1}行，第{2}列,值错误！..该属性将默认使用相应枚举第一个值！</color>", filePath, i + 2, j));
                }
            }
            else {
                var PType = p.PropertyType.ToString();
                switch (PType)
                {
                    case "System.String":
                        p.SetValue(obj, stringValue, null);
                        break;
                    case "System.Int32":
                        //Debug.Log(ls[i][j]);
                        if (stringValue != "null")
                        {
                            p.SetValue(obj, int.Parse((stringValue)), null);
                        }
                        break;
                    case "System.Int64":
                        if (stringValue != "null")
                        {
                            p.SetValue(obj, long.Parse((stringValue)), null);
                        }
                        break;
                    case "System.Single":
                        if (stringValue != "null")
                        {
                            p.SetValue(obj, float.Parse((stringValue)), null);
                        }
                        break;
                    case "System.Double":
                        if (stringValue != "null")
                        {
                            p.SetValue(obj, double.Parse((stringValue)), null);
                        }
                        break;
                    case "System.Boolean":
                        int value;
                        if (int.TryParse((stringValue), out value))
                        {
                            if (value == 1)
                                p.SetValue(obj, true, null);
                            else if (value == 0)
                                p.SetValue(obj, false, null);
                        }
                        else if (stringValue.Equals("Yes"))
                            p.SetValue(obj, true, null);
                        else if (stringValue.Equals("No"))
                            p.SetValue(obj, false, null);
                        else if (stringValue.Equals("True"))
                            p.SetValue(obj, true, null);
                        else if (stringValue.Equals("False"))
                            p.SetValue(obj, false, null);
                        else if (stringValue.Equals("TRUE"))
                            p.SetValue(obj, true, null);
                        else if (stringValue.Equals("FALSE"))
                            p.SetValue(obj, false, null);
                        else if (stringValue.Equals("true"))
                            p.SetValue(obj, true, null);
                        else if (stringValue.Equals("false"))
                            p.SetValue(obj, false, null);
                        break;
                    case "System.String[]":
                        string[] sSTemp = stringValue.Split(',');
                        p.SetValue(obj, (sSTemp), null);
                        break;
                    case "System.Int32[]":
                        //Debug.Log("PropertyType:System.Int32[]");
                        string[] sITemp = stringValue.Split(',');
                        int ilen = sITemp.Length;
                        int[] intTemp = new int[ilen];
                        //Debug.Log(ld[i][key].Length);
                        for (int iTemp = 0; iTemp < ilen; iTemp++)
                        {
                            //Debug.Log(ld[i][key][iTemp]);
                            if (sITemp[iTemp] != "null")
                            {
                                intTemp[iTemp] = int.Parse(sITemp[iTemp]);
                            }
                        }
                        p.SetValue(obj, intTemp, null);
                        break;
                    case "System.Single[]":
                        //Debug.Log("PropertyType:System.Single[]");
                        string[] sFTemp = stringValue.Split(',');
                        int flen = sFTemp.Length;
                        float[] floatTemp = new float[flen];
                        for (int iTemp = 0; iTemp < flen; iTemp++)
                        {
                            if (sFTemp[iTemp] != "null")
                            {
                                floatTemp[iTemp] = float.Parse(sFTemp[iTemp]);
                            }
                        }
                        p.SetValue(obj, floatTemp, null);
                        break;
                    case "System.Double[]":
                        //Debug.Log("PropertyType:System.Double[]");
                        string[] sDTemp = stringValue.Split(',');
                        int dlen = sDTemp.Length;
                        double[] doubleTemp = new double[dlen];
                        for (int iTemp = 0; iTemp < dlen; iTemp++)
                        {
                            if (sDTemp[iTemp] != "null")
                            {
                                doubleTemp[iTemp] = double.Parse(sDTemp[iTemp]);
                            }
                        }
                        p.SetValue(obj, doubleTemp, null);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region 直接读取/转换功能
        /// Add By HHao
        /// <summary>
        /// 读取CSV文件内容
        /// </summary>
        /// <param name="strin">CSV数据</param>
        /// <param name="encoding">格式</param>
        /// <returns>外层List保存每一行CSV数据 内层List保存每一个选项的数据</returns>
        public static List<List<string>> ReadCsvByEncode(string strin, Encoding encoding = null)
        {
            encoding = encoding != null ? encoding : Encoding.UTF8;

            List<List<string>> ret = new List<List<string>>();

            strin = strin.Replace("\r", "");
            string[] lines = strin.Split('\n');

            if (lines.Length > 0)
            {
                byte[] byt = encoding.GetBytes(lines[0]);
                if (byt.Length >= 3 &&
                    byt[0] == 0xEF &&
                    byt[1] == 0xBB &&
                    byt[2] == 0xBF)
                {
                    lines[0] = encoding.GetString(byt, 3, byt.Length - 3);
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]) ||
                        lines[i].StartsWith("#"))
                    continue;
                List<string> s = split(lines[i], encoding);
                ret.Add(s);
            }
            return ret;
        }
        static List<string> split(string line, Encoding encoding)
        {
            byte[] b = encoding.GetBytes(line);
            List<string> bls = new List<string>();
            int end = b.Length - 1;

            List<byte> bl = new List<byte>();
            bool inQuote = false;
            for (int i = 0; i < b.Length; i++)
            {
                switch ((char)b[i])
                {
                    case ',':
                    case '\t':
                        if (inQuote)
                            bl.Add(b[i]);
                        else
                        {
                            bls.Add(makefield(ref bl, encoding));
                            bl.Clear();
                        }
                        break;
                    case '"':
                        inQuote = !inQuote;
                        bl.Add((byte)'"');
                        break;
                    case '\\':
                        if (i < end)
                        {
                            switch ((char)b[i + 1])
                            {
                                case 'n':
                                    bl.Add((byte)'\n');
                                    i++;
                                    break;
                                case 't':
                                    bl.Add((byte)'\t');
                                    i++;
                                    break;
                                case 'r':
                                    i++;
                                    break;
                                default:
                                    bl.Add((byte)'\\');
                                    break;
                            }
                        }
                        else
                            bl.Add((byte)'\\');
                        break;
                    default:
                        bl.Add(b[i]);
                        break;
                }
            }
            bls.Add(makefield(ref bl, encoding));
            bl.Clear();

            return bls;
        }
        static string makefield(ref List<byte> bl, Encoding encoding)
        {
            if (bl.Count > 1 && bl[0] == '"' && bl[bl.Count - 1] == '"')
            {
                bl.RemoveAt(0);
                bl.RemoveAt(bl.Count - 1);
            }
            int n = 0;
            while (true)
            {
                if (n >= bl.Count)
                    break;
                if (bl[n] == '"')
                {
                    if (n < bl.Count - 1 && bl[n + 1] == '"')
                    {
                        bl.RemoveAt(n + 1);
                        n++;
                    }
                    else
                        bl.RemoveAt(n);
                }
                else
                    n++;
            }

            return encoding.GetString(bl.ToArray());
        }

        /// <summary>  
        /// 转换成Dictionary类型  
        /// </summary>  
        /// <param name="listin"></param>  
        /// <returns></returns>  
        public static Dictionary<string, List<string>> GetDirectory(List<List<string>> listin)
        {
            Dictionary<string, List<string>> dir = new Dictionary<string, List<string>>();
            for (int i = 0; i < listin.Count; i++)
            {
                if (string.IsNullOrEmpty(listin[i][0]))
                    continue;
                dir[listin[i][0]] = listin[i];
            }
            return dir;
        } 
        /// <summary>
        /// 读取CSV文件数据
        /// </summary>
        /// <typeparam name="T">与CSV结构相同的类，属性名必须与CSV的项名完全相同</typeparam>
        /// <param name="key">指定CSV中的某项内容作为返回的Dic的Key(只能为string)</param>
        /// <param name="path">指定的CSV文件路径</param>
        /// <returns>返回以指定项为Key，T为Value的字典</returns>
        public static Dictionary<string, T> GetCsvData<T>(string key, string csvData) where T : class, new()
        {
            Dictionary<string, T> dicResult = new Dictionary<string, T>();

            //string csvData = resourceMgr.Load<TextAsset>(path).text;

            List<List<string>> listCsvData = FileUtility.ReadCsvByEncode(csvData);

            List<string> proName = listCsvData[0];

            for (int row = 1; row < listCsvData.Count; row++)
            {
                T obj = new T();
                object keyName = row;

                PropertyInfo[] objPro = obj.GetType().GetProperties();

                for (int column = 0; column < proName.Count; column ++)
                {
                    for (int j = 0; j < objPro.Length; j++)
                    {
                        if (proName[column] == objPro[j].Name)
                        {
                            object value = ConvertStringToObject(objPro[j].PropertyType, listCsvData[row][column]);

                            objPro[j].SetValue(obj, value, null);

                            keyName = objPro[j].Name == key ? value : keyName;
                        }
                    }
                }
                dicResult[keyName.ToString()] = obj;
            }

            return dicResult;
        }
        #endregion
    }
}