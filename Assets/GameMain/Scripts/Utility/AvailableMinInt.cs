using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IUV.SDN
{
    /// <summary>
    /// 可用的最小整数
    /// </summary>
    public class AvailableMinInt
    {
        /// <summary>
        /// 当前可用的最小整数
        /// </summary>
        public int currAvailableInt { get; private set; }

        /// <summary>
        /// 起始的整数
        /// </summary>
        private int m_startInt;
        /// <summary>
        /// 已经被使用的整数的列表(用字典提高搜索速度)
        /// </summary>
        private Dictionary<int, byte> m_usedIntDic;

        /// <summary>
        /// 构造函数.需要指定起始的整数,也就是最小的可用整数.
        /// </summary>
        /// <param name="startInt">起始的整数</param>
        public AvailableMinInt(int startInt)
        {
            m_startInt = startInt;
            currAvailableInt = startInt;
            m_usedIntDic = new Dictionary<int, byte>();
        }

        /// <summary>
        /// 使用当前可用的值。返回这个值。
        /// </summary>
        /// <returns></returns>
        public int Use()
        {
            int value = currAvailableInt;

            Use(currAvailableInt);

            return value;
        }
        /// <summary>
        /// 使用指定的值
        /// </summary>
        /// <param name="value"></param>
        public void Use(int value)
        {
            //小于初始整数，不处理
            if (value < m_startInt) return;

            if (!m_usedIntDic.ContainsKey(value))
            {
                //添加
                m_usedIntDic.Add(value, 0);

                //更新当前的可用整数
                if (value == currAvailableInt)
                {
                    currAvailableInt++;
                    while (m_usedIntDic.ContainsKey(currAvailableInt))
                    {
                        currAvailableInt++;
                    }
                }
            }
        }

        /// <summary>
        /// 释放指定的值
        /// </summary>
        /// <param name="value"></param>
        public void Release(int value)
        {
            if (m_usedIntDic.ContainsKey(value))
            {
                m_usedIntDic.Remove(value);

                //更新当前的可用整数
                if (value < currAvailableInt) currAvailableInt = value;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            currAvailableInt = m_startInt;
            m_usedIntDic = new Dictionary<int, byte>();
        }
        /// <summary>
        /// 使用指定的起始整数重置
        /// </summary>
        /// <param name="startInt"></param>
        public void Reset(int startInt)
        {
            m_startInt = startInt;
            Reset();
        }

    }
}