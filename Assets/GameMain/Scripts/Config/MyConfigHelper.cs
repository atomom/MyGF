using System;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class MyConfigHelper : DefaultConfigHelper
    {
        private static readonly string[] RowSplitSeparator = new string[] { "\r\n", "\r", "\n" };
        private static readonly string[] ColumnSplitSeparator = new string[] { "\t" };
        private const int ColumnCount = 4;
        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public override bool ParseConfig(string text, object userData)
        {
            try
            {
                string[] rowTexts = text.Split(RowSplitSeparator, StringSplitOptions.None);
                for (int i = 0; i < rowTexts.Length; i++)
                {
                    if (i < 3 || rowTexts[i].Length <= 0 || rowTexts[i][0] == '#')
                    {
                        continue;
                    }

                    string[] splitLine = rowTexts[i].Split(ColumnSplitSeparator, StringSplitOptions.None);
                    if (splitLine.Length < ColumnCount)
                    {
                        Log.Warning("Can not parse config '{0}'.", text);
                        return false;
                    }

                    string configName = splitLine[1];
                    string configValue = splitLine[3];
                    if (!AddConfig(configName, configValue))
                    {
                        Log.Warning("Can not add raw string with config name '{0}' which may be invalid or duplicate.", configName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config '{0}' with exception '{1}'.", text, exception.ToString());
                return false;
            }
        }
    }
}