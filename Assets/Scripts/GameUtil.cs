using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtil : MonoBehaviour
{
    private Transform GridsTrans;
    public static GameObject[,] cells;
    /// <summary>
    /// 场景cell或棋子与平面直角坐标(x方向0-8，y方向0-9)之间的映射关系
    /// </summary>
    public static Dictionary<GameObject, Vector2> coords;
    /// <summary>
    /// 棋子与他现在二维坐标的映射
    /// </summary>
    public static Dictionary<GameObject, Vector2> chess2Vector;
    /// <summary>
    /// 棋子二维坐标与自身的映射
    /// </summary>
    public static Dictionary<Vector2, GameObject> vector2Chess;
    void Awake()
    {
        GridsTrans = GameObject.Find("Grids").transform;
        cells = new GameObject[9, 10];
        for (int y = 0; y <= 9; y++)
        {
            Transform row = GridsTrans.FindChild("row" + y);
            for (int x = 0; x <= 8; x++)
            {
                cells[x, y] = row.FindChild("cell" + x).gameObject;
            }
        }

        coords = new Dictionary<GameObject, Vector2>();
        for (int x = 0; x <= 8; x++)
        {
            for (int y = 0; y <= 9; y++)
            {
                coords.Add(cells[x, y], new Vector2(x, y));
            }
        }
        chess2Vector = new Dictionary<GameObject, Vector2>();
        vector2Chess = new Dictionary<Vector2, GameObject>();
    }
	
	void Update () {
		
	}
}
