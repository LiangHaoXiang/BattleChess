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

    public static void SetChessAndVectorDic(List<GameObject> chessList)
    {
        chess2Vector = new Dictionary<GameObject, Vector2>();
        vector2Chess = new Dictionary<Vector2, GameObject>();
        for (int i = 0; i < chessList.Count; i++)
        {
            Vector3 pos = chessList[i].transform.position;
            int a = (int)pos.x;      //强制为整形，消除微弱差别的影响
            int b = (int)pos.y;
            int c = (int)pos.z;
            Vector3 v3 = new Vector3(a, b, c);
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

    public static void ClearChessAndVectorDic()
    {
        if(Chess2Vector != null || Vector2Chess != null)
        {
            chess2Vector.Clear();
            vector2Chess.Clear();
        }
    }
}
