﻿
using UnityEngine;

namespace IUV.SDN
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        { 
            InitBuiltinComponents();
            InitCustomComponents();
        }
    }
}