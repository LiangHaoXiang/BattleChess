using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameUtil
{
    /// <summary>
    /// 判断传入的二维点是否在棋盘内
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool IsInChessBoard(Vector2 value)
    {
        return (int)value.x >= 0 && (int)value.x <= 8 && (int)value.y >= 0 && (int)value.y <= 9;
    }
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
    /// 双方战斗比较，返回阵亡者，且胜者血量相应损失
    /// </summary>
    /// <param name="attacker">进攻方</param>
    /// <param name="defender">防守方</param>
    /// <returns>阵亡者</returns>
    public static GameObject Battle(GameObject attacker, GameObject defender)
    {
        AttrBox a = GetChessAttrList(attacker);
        AttrBox b = GetChessAttrList(defender);
        int a_damage = a.Attack <= b.Defence ? 1 : a.Attack - b.Defence;    //a对b造成的伤害
        int b_damage = b.Attack <= a.Defence ? 1 : b.Attack - a.Defence;    //b对a造成的伤害
        int a_times = b.Hp / a_damage;      //a打死b所需回合数
        int b_times = a.Hp / b_damage;      //b打死a所需回合数
        if (a_times <= b_times)
        {
            a.Hp -= b_damage * (a_times - 1);
            b.Hp = 0;
            return defender;
        }
        else
        {
            b.Hp -= a_damage * b_times;
            a.Hp = 0;
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
        //Type baseType = type.BaseType;          //其实这样也可以的，这个是参考反射的
        BaseChess bc = chess.GetComponent(type) as BaseChess;   //关键是这一句
        return bc;
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

    /// <summary>
    /// 获取时间 格式如："13:26"  (13分26秒)
    /// </summary>
    /// <param name="time">秒</param>
    /// <returns></returns>
    public static string TimeToStr(float time)
    {
        int min = (int)time / 60;
        int second = (int)time % 60;
        string m = min >= 10 ? min.ToString() : "0" + min;
        string s = second >= 10 ? second.ToString() : "0" + second;
        return m + ":" + s;
    }


    /// <summary>
    /// 获取在场的红方或黑方所有棋子
    /// </summary>
    /// <param name="chessList">工作区棋子列表</param>
    /// <param name="tag">"Red"  "Black"</param>
    /// <returns></returns>
    public static List<GameObject> getChessListByTag(List<GameObject> chessList, string tag)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < chessList.Count; i++)
        {
            if (chessList[i].tag == tag)
                result.Add(chessList[i]);
        }
        return result;
    }


    /// <summary>
    /// 获取红方或黑方所有可走点
    /// </summary>
    /// <param name="chessList">工作区棋子</param>
    /// <param name="tag">阵营</param>
    /// <param name="chess2Vector">棋局信息</param>
    /// <param name="vector2Chess">棋局信息</param>
    /// <returns></returns>
    public static List<Vector2> getTeamMovePoints(List<GameObject> chessList, string tag, Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess)
    {
        List<Vector2> allMovePoints = new List<Vector2>();
        List<GameObject> chessTeam = getChessListByTag(chessList, tag);
        foreach(GameObject chess in chessTeam)
        {
            List<Vector2> chessPoints = GetChessInstance(chess).CanMovePoints(chess2Vector, vector2Chess);
            foreach(Vector2 point in chessPoints)
            {
                allMovePoints.Add(point);
            }
        }
        return allMovePoints;
    }
}
