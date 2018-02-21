using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene3_UI : MonoBehaviour
{
    private Transform GridsTrans;
    public static GameObject[,] cells;

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
                cells[x, y].GetComponent<Image>().enabled = false;
            }
        }
        GameCache.SetCoords(cells);     //将场景找到的cell作为参数，处理写入映射缓存

    }

    //临时
    public GameObject redJu;
    public List<GameObject> chessList;
    void Start()
    {
        redJu = GameObject.Find("红車");
        chessList = new List<GameObject>();
        chessList.Add(redJu);
        chessList.Add(GameObject.Find("红車 (2)"));
        chessList.Add(GameObject.Find("黑車"));
        //foreach (KeyValuePair<Vector3, Vector2> kvp in GameCache.Coords)
        //{
        //    Debug.Log(kvp.Key + "   " + kvp.Value);
        //}
        GameCache.SetChessAndVectorDic(chessList);
    }
}
