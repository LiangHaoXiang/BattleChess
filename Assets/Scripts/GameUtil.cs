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

}
