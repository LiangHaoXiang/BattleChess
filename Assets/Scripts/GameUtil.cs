using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameUtil
{
    /// <summary>
    /// 根据父物体名字后的数字和自身名字后的数字，得到二位坐标
    /// </summary>
    /// <param name="parentName">"rowY"</param>
    /// <param name="cellName">"cellX"</param>
    public static Vector2 Str2Vector(string parentName, string cellName)
    {
        string xStr = cellName.Substring(4, 1);
        string yStr = parentName.Substring(3, 1);
        int x = Convert.ToInt32(xStr);
        int y = Convert.ToInt32(yStr);

        return new Vector2(x, y);
    }

    /// <summary>
    /// 判断两个二维坐标是否相等，微弱的差别可忽略
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool CompareVector2(Vector2 a, Vector2 b)
    {
        if (((int)a.x == (int)b.x) && ((int)a.y == (int)b.y))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断两个三维坐标是否相等，微弱的差别可忽略
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool CompareVector3(Vector3 a,Vector3 b)
    {
        if (((int)a.x == (int)b.x) && ((int)a.y == (int)b.y) && ((int)a.z == (int)b.z))
            return true;
        else
            return false;
    }

    /// <summary>
    /// 计算战力
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <returns></returns>
    public static int CalCombat(int hp, int attack, int defence)
    {
        return (int)(hp * 0.4 + attack * 10 + defence * 10);
    }

    /// <summary>
    /// 两棋子战斗比较，返回阵亡者，且胜者血量相应损失
    /// </summary>
    /// <param name="attacker">进攻方</param>
    /// <param name="defender">防守方</param>
    /// <returns></returns>
    public static GameObject Battle(GameObject attacker, GameObject defender)
    {
        AttrBox a = GetChessAttrList(attacker);
        AttrBox b = GetChessAttrList(defender);
        int a_times = b.Hp / (a.Attack - b.Defence);      //a打死b所需回合数
        int b_times = a.Hp / (b.Attack - a.Defence);      //b打死a所需回合数
        if (a_times <= b_times)
        {
            a.Hp -= (b.Attack - a.Defence) * (a_times - 1);
            return defender;
        }
        else
        {
            b.Hp -= (a.Attack - b.Defence) * b_times;
            return attacker;
        }
    }

    /// <summary>
    /// 根据棋子物体获取棋子对象实例
    /// </summary>
    /// <param name="chess">棋子物体</param>
    /// <returns></returns>
    public static BaseChess GetChessInstance(GameObject chess)
    {
        Component[] components = chess.GetComponents<Component>();
        Type type = components[2].GetType();    //默认第3个组件都是继承同一父类BaseChess的脚本
        BaseChess bc = chess.GetComponent(type) as BaseChess;   //关键是这一句
        return bc;
    }

    /// <summary>
    /// 获取棋子Id
    /// </summary>
    /// <param name="chess"></param>
    /// <returns></returns>
    public static int GetChessId(GameObject chess)
    {
        return GetChessInstance(chess).chessId;
    }

    public static AttrBox GetChessAttrList(GameObject chess)
    {
        return GetChessInstance(chess).attrBox;
    }

    public static string GetChessName(GameObject chess)
    {
        return GetChessInstance(chess).chessName;
    }

    public static int GetChessCombat(GameObject chess)
    {
        return GetChessInstance(chess).attrBox.Combat;
    }

    public static int GetChessCombat(AttrBox chessAttrBox)
    {
        return chessAttrBox.Combat;
    }

    //public static GameObject GetAttacker()
    //{

    //}
}
