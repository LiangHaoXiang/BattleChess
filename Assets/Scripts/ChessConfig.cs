using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChessConfig
{
    public static List<ChessCfg> cfg;

    [Serializable]
    public class ChessCfg  //配置的各个字段
    {
        public string name;
        public int hp;
        public int attack;
        public int defence;
    }

    [Serializable]
    public class CfgClass
    {
        public List<ChessCfg> table;
    }

    //void Awake()
    //{
    //    ReadJson();
    //}
    /// <summary>
    /// 读取Json数据
    /// </summary>
    public void ReadJson()
    {
        TextAsset textAsset = new TextAsset();
        textAsset = Resources.Load("Json/t_ChessConfig") as TextAsset;

        string txt = textAsset.text;

        if (txt != string.Empty)
        {
            CfgClass item = JsonUtility.FromJson<CfgClass>(txt);     //反序列化后存储到类或结构体
            cfg = item.table;    //获取类的对象拥有的属性列表
        }
    }
}
