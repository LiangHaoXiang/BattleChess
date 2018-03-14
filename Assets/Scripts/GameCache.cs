using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCache
{
    /// <summary>
    /// 场景cell或棋子与平面直角坐标(x方向0-8，y方向0-9)之间的映射关系
    /// </summary>
    private static Dictionary<Vector3, Vector2> coords;
    /// <summary>
    /// 棋子与他现在二维坐标的映射
    /// </summary>
    private static Dictionary<GameObject, Vector2> chess2Vector;
    /// <summary>
    /// 棋子二维坐标与自身的映射
    /// </summary>
    private static Dictionary<Vector2, GameObject> vector2Chess;
    /// <summary>
    /// 记录每回合的所有棋局图谱信息
    /// </summary>
    public static List<Dictionary<GameObject, Vector2>> maps;
    /// <summary>
    /// 记录每回合的所有棋子对应的属性信息
    /// </summary>
    public static List<Dictionary<GameObject, string>> attrMaps;
    /// <summary>
    /// 记录阵亡者与对应步数的映射
    /// </summary>
    private static Dictionary<GameObject, int> loserStepDic;

    public static void SetCoords(GameObject[,] cells)
    {
        if (coords == null)
        {
            coords = new Dictionary<Vector3, Vector2>();
            for (int x = 0; x <= 8; x++)
            {
                for (int y = 0; y <= 9; y++)
                {
                    int a = (int)cells[x, y].transform.position.x;      //强制为整形，消除微弱差别的影响
                    int b = (int)cells[x, y].transform.position.y;
                    int c = (int)cells[x, y].transform.position.z;
                    coords.Add(new Vector3(a, b, c), new Vector2(x, y));
                }
            }
        }
    }

    public static Dictionary<Vector3, Vector2> Coords
    {
        get { return coords; }
    }

    public static void SetChessVectorDic(List<GameObject> chessList)
    {
        chess2Vector = new Dictionary<GameObject, Vector2>();
        vector2Chess = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < chessList.Count; i++)
        {
            Vector3 pos = chessList[i].transform.position;
            Vector3 v3 = new Vector3((int)pos.x, (int)pos.y, (int)pos.z);//强制为整形，消除微弱差别的影响
            chess2Vector.Add(chessList[i], coords[v3]);
            vector2Chess.Add(coords[v3], chessList[i]);
        }
    }

    public static void SetLoserStepDic(GameObject chess, int step)
    {
        loserStepDic.Add(chess, step);
    }

    public static Dictionary<GameObject, Vector2> Chess2Vector
    {
        get { return chess2Vector; }
    }

    public static Dictionary<Vector2, GameObject> Vector2Chess
    {
        get { return vector2Chess; }
    }

    public static Dictionary<GameObject, int> LoserStepDic
    {
        get { return loserStepDic; }
    }

    public static void ClearChessVectorDic()
    {
        if(Chess2Vector != null || Vector2Chess != null)
        {
            chess2Vector.Clear();
            vector2Chess.Clear();
        }
    }

    /// <summary>
    /// 更新棋局信息
    /// </summary>
    /// <returns></returns>
    public static void UpdateChessData()
    {
        ClearChessVectorDic();
        SetChessVectorDic(PoolManager.work_List);
    }

    /// <summary>
    /// 添加棋谱
    /// </summary>
    public static void SetMaps()
    {
        if (maps == null)
            maps = new List<Dictionary<GameObject, Vector2>>();
        Dictionary<GameObject, Vector2> temp = new Dictionary<GameObject, Vector2>();
        foreach (KeyValuePair<GameObject, Vector2> kvp in Chess2Vector)
        {
            //一定要这样遍历赋值的，否则如果直接temp=CalculateUtil.chess2Vector的话
            //就相当于引用了这个静态字典，每次添加到棋谱里就是同一个temp数据
            temp.Add(kvp.Key, kvp.Value);
        }
        maps.Add(temp);  //添加棋谱
    }

    /// <summary>
    /// 添加每回合全部棋子对应属性信息图谱
    /// </summary>
    public static void SetAttrMaps()
    {
        if (attrMaps == null)
            attrMaps = new List<Dictionary<GameObject, string>>();
        Dictionary<GameObject, string> dic = new Dictionary<GameObject, string>(); 
        foreach (GameObject chess in PoolManager.work_List)
        {
            AttrBox chessAttrs = GameUtil.GetChessAttrList(chess);
            string attrStr = chessAttrs.Hp + "_" + chessAttrs.Attack + "_" + chessAttrs.Defence;
            //AttrBox temp = new AttrBox(); //这里要想办法优化，频繁创建对象消耗性能
            //temp.Hp = chessAttrs.Hp;
            //temp.Attack = chessAttrs.Attack;
            //temp.Defence = chessAttrs.Defence;
            //dic.Add(chess, temp);
            dic.Add(chess, attrStr);
        }
        attrMaps.Add(dic);
    }

    public static void ClearCache()
    {
        if (maps != null)
            maps.Clear();
        if (attrMaps != null)
            attrMaps.Clear();
        if (loserStepDic != null)
            loserStepDic.Clear();
        ClearChessVectorDic();
    }
}
