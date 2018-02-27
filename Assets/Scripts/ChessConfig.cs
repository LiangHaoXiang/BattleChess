using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChessConfig
{
    public static List<ChessCfg> cfg;
    public static Dictionary<string, List<int>> chessDic;   //棋子名与棋子一堆属性的映射

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
        public List<ChessCfg> Attributes;
    }
    /// <summary>
    /// 读取Json数据
    /// </summary>
    public static List<ChessCfg> ReadJson()
    {
        if (cfg == null)
        {
            TextAsset textAsset = new TextAsset();
            textAsset = Resources.Load("Json/t_ChessConfig") as TextAsset;

            string txt = textAsset.text;

            if (txt != string.Empty)
            {
                CfgClass item = JsonUtility.FromJson<CfgClass>(txt);     //反序列化后存储到类或结构体
                cfg = item.Attributes;    //获取类的对象拥有的属性列表
            }
        }
        return cfg;
    }

    public static Dictionary<string, List<int>> GetChessDic()
    {
        if (chessDic == null)
        {
            chessDic = new Dictionary<string, List<int>>();     //映射表，把chessName作为key
            cfg = ReadJson();
            foreach(ChessCfg cc in cfg)
            {
                List<int> attrList = new List<int>();
                attrList.Add(cc.hp);                //attrList索引为0
                attrList.Add(cc.attack);            //attrList索引为1
                attrList.Add(cc.defence);           //attrList索引为2
                chessDic.Add(cc.name, attrList);
            }
        }

        return chessDic;
    }

    /// <summary>
    /// 获取某棋子初始属性值列表
    /// </summary>
    /// <param name="chessName"></param>
    /// <returns></returns>
    public static List<int> GetAttrList(string chessName)
    {
        chessDic = GetChessDic();
        return chessDic[chessName];
    }
}
