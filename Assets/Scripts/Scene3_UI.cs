using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene3_UI : MonoBehaviour
{
    #region 左
    private GameObject leftGameMode;
    private GameObject leftReplayMode;
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
    private GameObject rightGameMode;
    private GameObject rightReplayMode;
    private GameObject blackDetailPanel;
    private GameObject redDetailPanel;
    #endregion

    void Awake()
    {
        leftGameMode = GameObject.Find("Canvas/Left/GameMode");
        leftReplayMode = GameObject.Find("Canvas/Left/ReplayMode");
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
        rightGameMode = GameObject.Find("Canvas/Right/GameMode");
        rightReplayMode = GameObject.Find("Canvas/Right/ReplayMode");
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
        SetMode(true);
    }

    /// <summary>
    /// 设置模式，游戏模式还是复盘模式
    /// </summary>
    public void SetMode(bool isGameMode)
    {
        leftGameMode.SetActive(isGameMode);
        rightGameMode.SetActive(isGameMode);
        leftReplayMode.SetActive(!isGameMode);
        rightReplayMode.SetActive(!isGameMode);
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
        //foreach(GameObject chess in PoolManager.work_List)
        //{
        //    Component[] components = chess.GetComponents<Component>();
        //    Type type = components[2].GetType();    //组件第3个都是继承同一父类BaseChess的脚本
            
        //    BaseChess bc = chess.GetComponent(type) as BaseChess;   //关键是这一句
        //    Debug.Log(bc.chessName);
        //    Debug.Log(bc.attrBox.Hp);
        //    Debug.Log(bc.attrBox.Attack);
        //    Debug.Log(bc.attrBox.Defence);
        //}