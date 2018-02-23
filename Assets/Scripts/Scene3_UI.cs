using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene3_UI : MonoBehaviour
{
    #region 左
    private Text blackAllTime;
    private Text blackStepTime;
    private Text redAllTime;
    private Text redStepTime;
    #endregion
    #region 中
    private Transform GridsTrans;
    public static GameObject[,] cells;
    private GameObject beginBtn;
    #endregion
    #region 右
    private GameObject blackDetailPanel;
    private GameObject redDetailPanel;
    #endregion

    void Awake()
    {
        blackAllTime = GameObject.Find("Canvas/Left/GameMode/Black/AllTime/Value").GetComponent<Text>();
        blackStepTime = GameObject.Find("Canvas/Left/GameMode/Black/StepTime/Value").GetComponent<Text>();
        redAllTime = GameObject.Find("Canvas/Left/GameMode/Red/AllTime/Value").GetComponent<Text>();
        redStepTime = GameObject.Find("Canvas/Left/GameMode/Red/StepTime/Value").GetComponent<Text>();

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

        beginBtn = GameObject.Find("Canvas/Middle/BeginBtn");
        blackDetailPanel = GameObject.Find("Canvas/Right/BlackAttrDetail");
        redDetailPanel = GameObject.Find("Canvas/Right/RedAttrDetail");
    }

    //临时
    //public GameObject redJu;
    public List<GameObject> chessList;
    public CreateManager createManager;
    void Start()
    {
        blackAllTime.text = "00:00";
        blackStepTime.text = "00:00";
        redAllTime.text = "00:00";
        redStepTime.text = "00:00";
        blackDetailPanel.SetActive(false);
        redDetailPanel.SetActive(false);

        //redJu = GameObject.Find("红車");
        //chessList = new List<GameObject>();
        //chessList.Add(redJu);
        //chessList.Add(GameObject.Find("红車 (2)"));
        //chessList.Add(GameObject.Find("黑車"));
        //GameCache.SetChessVectorDic(chessList);


        //Debug.Log("先打印");
        //Debug.Log(PoolManager.work_List[0].transform.position);
        //foreach (KeyValuePair<Vector3, Vector2> kvp in GameCache.Coords)
        //{
        //    Debug.Log("coords的键值对：" + kvp.Key + "   " + kvp.Value);
        //    if (GameUtil.CompareVector3(kvp.Key, PoolManager.work_List[0].transform.position))
        //    {
        //        Debug.Log("找到相等的三维坐标， ");
        //        Debug.Log(kvp.Value);
        //    }
        //}


    }

    /// <summary>
    /// 开始按钮点击事件
    /// </summary>
    public void OnBeginClick()
    {
        createManager = CreateManager.Instance;
        createManager.InitChessBoard();
        GameController.Instance.UpdateGameData();

        beginBtn.SetActive(false);
    }
}
