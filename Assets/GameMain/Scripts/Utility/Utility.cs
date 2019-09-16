using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public static class Utility
    {
        public static void TrySet<T>(string[] ss, ref int i, List<T> ret)
        {

            string[] act = new string[ret.Count];
            for (int j = 0; j < ret.Count; ++j)
            {
                act[j] = ret[j].ToString();
            }
            ss[i] = string.Join("^", act);
            i++;
        }

        public static void TrySet(string[] ss, ref int i, string s1)
        {
            ss[i] = s1;
            i++;
        }

        public static void TrySet(string[] ss, ref int i, int s1)
        {
            ss[i] = s1.ToString();
            i++;
        }

        public static void TrySet(string[] ss, ref int i, bool ret)
        {
            ss[i] = ret ? "1" : "0";
            i++;
        }
        public static void TrySet(string[] ss, ref int i, DateTime t)
        {
            ss[i] = t.ToString();
            i++;
        }

        public static bool TryParse(string[] ss, ref int i, out DateTime t)
        {
            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }
            DateTime.TryParse(s, out t);
            i++;
            return true;
        }
        public static bool TryParse(string[] ss, ref int i, out bool s1)
        {
            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }
            s1 = (s == "1" ? true : false);
            i++;
            return true;
        }
        public static bool TryParse(string[] ss, ref int i, out string s1)
        {
            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }
            s1 = s == null? "": s;
            i++;
            return true;
        }

        public static bool TryParse(string[] ss, ref int i, out int s1)
        {
            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }

            bool ret = int.TryParse(s, out s1);
            i++;
            return ret;
        }

        public static bool TryParse(string[] ss, ref int i, out float s1)
        {
            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }

            bool ret = float.TryParse(s, out s1);
            i++;
            return ret;
        }

        public static bool TryParse(string[] ss, ref int i, List<string> list)
        {
            list.Clear();

            string s = null;
            if (ss != null && ss.Length > i)
            {
                s = ss[i];
            }

            if (String.IsNullOrEmpty(s))
            {
                return true;
            }
            var sss = s.Split('^');
            for (int j = 0; j < sss.Length; ++j)
            {
                var a = sss[j];
                list.Add(a);
            }

            i++;
            return true;
        }

        public static string ToString(string subtype)
        {

            return subtype = (string.IsNullOrEmpty(subtype) || subtype == "null") ? null : subtype;
        }

        /// <summary>
        /// 深度克隆对象  注意在克隆的对象必须要有一个不带参数的公共构造函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            Type type = typeof(T);
            System.Object destination = null;
            try
            {
                destination = Activator.CreateInstance(type);
            }
            catch { }

            int i = 0;
            Type desType = destination.GetType();
            foreach (FieldInfo mi in type.GetFields())
            {
                try
                {
                    FieldInfo des = desType.GetField(mi.Name);
                    if (des != null && des.FieldType == mi.FieldType)
                    {
                        des.SetValue(destination, mi.GetValue(source));
                        i++;
                    }
                }
                catch (Exception) { }
            }
            foreach (PropertyInfo pi in type.GetProperties())
            {
                try
                {
                    PropertyInfo des = desType.GetProperty(pi.Name);
                    if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                    {
                        des.SetValue(destination, pi.GetValue(source, null), null);
                        i++;
                    }
                }
                catch { }
            }
            return (T) destination;

        }

        /// <summary>
        /// int为32位除4位，数组为8
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static byte[] IntToByte4(int num)
        {
            byte[] result = new byte[4];
            result[0] = (byte) (num >> 24); //取最高8位放到0下标
            result[1] = (byte) (num >> 16); //取次高8为放到1下标
            result[2] = (byte) (num >> 8); //取次低8位放到2下标
            result[3] = (byte) (num); //取最低8位放到3下标
            return result;
        }
        /**
         * 将4字节的byte数组转成int值
         * @param b
         * @return
         */
        public static int Byte4ToInt(byte[] b)
        {
            byte[] a = new byte[4];
            int i = a.Length - 1, j = b.Length - 1;
            for (; i >= 0; i--, j--)
            { //从b的尾部(即int值的低位)开始copy数据
                if (j >= 0)
                    a[i] = b[j];
                else
                    a[i] = 0; //如果b.length不足4,则将高位补0
            }
            int v0 = (a[0] & 0xff) << 24; //&0xff将byte值无差异转成int,避免Java自动类型提升后,会保留高位的符号位
            int v1 = (a[1] & 0xff) << 16;
            int v2 = (a[2] & 0xff) << 8;
            int v3 = (a[3] & 0xff);
            return v0 + v1 + v2 + v3;
        }

        /// <summary>
        /// Activates or deactivates a gameobject for any Unity version.
        /// </summary>
        public static void Activate(GameObject obj, bool activate = true)
        {

#if UNITY_3_5
            obj.SetActiveRecursively(activate);
#else
            obj.SetActive(activate);
#endif

        }
        public static bool IsActive(GameObject go)
        {
            bool ret = false;
            ret = go != null && go.activeSelf;
            return ret;
        }

        /// <summary>
        /// Dictionary of type aliases for error messages.
        /// </summary>
        private static readonly Dictionary<Type, string> m_TypeAliases = new Dictionary<Type, string>()
        {

            { typeof(void), "void" }, { typeof(byte), "byte" }, { typeof(sbyte), "sbyte" }, { typeof(short), "short" }, { typeof(ushort), "ushort" }, { typeof(int), "int" }, { typeof(uint), "uint" }, { typeof(long), "long" }, { typeof(ulong), "ulong" }, { typeof(float), "float" }, { typeof(double), "double" }, { typeof(decimal), "decimal" }, { typeof(object), "object" }, { typeof(bool), "bool" }, { typeof(char), "char" }, { typeof(string), "string" }, { typeof(UnityEngine.Vector2), "Vector2" }, { typeof(UnityEngine.Vector3), "Vector3" }, { typeof(UnityEngine.Vector4), "Vector4" }

        };

        /// <summary>
        /// Returns the 'syntax style' formatted version of a type name.
        /// for example: passing 'System.Single' will return 'float'.
        /// </summary>
        public static string GetTypeAlias(Type type)
        {

            string s = "";

            if (!m_TypeAliases.TryGetValue(type, out s))
                return type.ToString();

            return s;

        } /// <summary>
        /// Performs a stack trace to see where things went wrong
        /// for error reporting.
        /// </summary>
        public static string GetErrorLocation(int level = 1, bool showOnlyLast = false)
        {

            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string result = "";
            string declaringType = "";

            for (int v = stackTrace.FrameCount - 1; v > level; v--)
            {
                if (v < stackTrace.FrameCount - 1)
                    result += " --> ";
                System.Diagnostics.StackFrame stackFrame = stackTrace.GetFrame(v);
                if (stackFrame.GetMethod().DeclaringType.ToString() == declaringType)
                    result = ""; // only report the last called method within every class
                declaringType = stackFrame.GetMethod().DeclaringType.ToString();
                result += declaringType + ":" + stackFrame.GetMethod().Name;
            }

            if (showOnlyLast)
            {
                try
                {
                    result = result.Substring(result.LastIndexOf(" --> "));
                    result = result.Replace(" --> ", "");
                }
                catch { }
            }

            return result;

        }
        /// <summary>
        /// if target is a transform, returns its parent. if not, returns its
        /// transform. will return null if:
        /// 1) target is null
        /// 2) target's transform is null (has somehow been deleted)
        /// 3) target transform's parent is null (we have hit the scene root)
        /// </summary>
        public static Component GetParent(Component target)
        {

            if (target == null)
                return null;

            if (target != target.transform)
                return target.transform;

            return target.transform.parent;

        }

        public static void DebugInfo(object obj)
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                object value = i.GetValue(obj, null);
                string name = i.Name;
                Log.Info(name + "\t=\t" + value);
            }
        }
        static System.Globalization.DateTimeFormatInfo dtFormat;
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            if (dtFormat == null)
            {
                dtFormat = new System.Globalization.DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss fff";
            }
            DateTime dtStart = System.Convert.ToDateTime(timeStamp, dtFormat);
            return dtStart;
        }
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int) (time - startTime).TotalSeconds;
        }
        /// <summary>
        /// 将字节数组转字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string BytesToStr(byte[] content)
        {
            StreamReader stream = new StreamReader(new MemoryStream(content), Encoding.GetEncoding("UTF-8"));
            string newStr = stream.ReadToEnd();
            return newStr;
        }

        public static void ShowAppMsg(string str)
        {

        }

        /// <summary>
        /// 将指定类型的List转换成另一种类型的List.
        /// <para>使用as转换,两种类型必须是子类/父类的关系,否则转换不成功.</para>
        /// </summary>
        /// <typeparam name="SrcT">要进行转换的类型</typeparam>
        /// <typeparam name="ResultT">返回的类型</typeparam>
        /// <param name="srcList">要转换的数据</param>
        /// <returns></returns>
        public static List<ResultT> ConvertList<SrcT, ResultT>(List<SrcT> srcList) where ResultT : class
        {
            List<ResultT> resultList = new List<ResultT>();
            for (int i = 0; i < srcList.Count; i++)
            {
                resultList.Add(srcList[i] as ResultT);
            }
            return resultList;
        }

        public static void ConvertList<SrcT, ResultT>(List<SrcT> srcList, List<ResultT> retList) where ResultT : class
        {
            retList.Clear();
            for (int i = 0; i < srcList.Count; i++)
            {
                retList.Add(srcList[i] as ResultT);
            }
        }

        public static string ToJson(object o)
        {
            return LitJson.JsonMapper.ToJson(o);
        }

        public static T ToObject<T>(string s)
        {
            return LitJson.JsonMapper.ToObject<T>(s);
        }
    }
}