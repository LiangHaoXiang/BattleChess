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
    /// 记录每走一步的所有棋局信息
    /// </summary>
    private static List<Dictionary<GameObject, Vector2>> maps;

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

    public static Dictionary<GameObject, Vector2> Chess2Vector
    {
        get { return chess2Vector; }
    }

    public static Dictionary<Vector2, GameObject> Vector2Chess
    {
        get { return vector2Chess; }
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

    public static List<Dictionary<GameObject, Vector2>> Maps
    {
        get { return maps; }
    }

    public static void ClearMaps()
    {
        if (maps != null)
            maps.Clear();
    }
}
