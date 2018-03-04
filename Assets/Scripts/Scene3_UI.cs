using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene3_UI : MonoBehaviour
{
    private static Scene3_UI instance = null;
    public static Scene3_UI Instance { get { return instance; } }

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
    private GameObject addAtrrPanel;        //加属性面板
    private Text addChessName;
    private Text addHpValue;
    private Text addAttackValue;
    private Text addDefenceValue;
    private GameObject endPanel;            //结束面板
    private Text winer;
    #endregion
    #region 右
    private GameObject rightGameMode;
    private GameObject rightReplayMode;
    private GameObject blackDetailPanel;
    private Text b_Hp;
    private Text b_Attack;
    private Text b_Defence;
    private Text b_Combat;
    private GameObject redDetailPanel;
    private Text r_Hp;
    private Text r_Attack;
    private Text r_Defence;
    private Text r_Combat;
    #endregion

    private GameObject curAddChess;
    private AttrBox curAttr;
    public static event AddAttrCompleteEventHandler AddAttrCompleteEvent;

    void Awake()
    {
        if (instance == null)
            instance = this;
        leftGameMode = GameObject.Find("Canvas/Left/GameMode");
        leftReplayMode = GameObject.Find("Canvas/Left/ReplayMode");
        blackAllTime = GameObject.Find("Canvas/Left/GameMode/Black/AllTime/Value").GetComponent<Text>();
        blackStepTime = GameObject.Find("Canvas/Left/GameMode/Black/StepTime/Value").GetComponent<Text>();
        redAllTime = GameObject.Find("Canvas/Left/GameMode/Red/AllTime/Value").GetComponent<Text>();
        redStepTime = GameObject.Find("Canvas/Left/GameMode/Red/StepTime/Value").GetComponent<Text>();
        /*****************中******************/
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
        addAtrrPanel = GameObject.Find("Canvas/Middle/AddAttrPanel");
        addChessName = GameObject.Find("Canvas/Middle/AddAttrPanel/ChessName").GetComponent<Text>();
        addHpValue = GameObject.Find("Canvas/Middle/AddAttrPanel/Grid/Hp/Value").GetComponent<Text>();
        addAttackValue = GameObject.Find("Canvas/Middle/AddAttrPanel/Grid/Attack/Value").GetComponent<Text>();
        addDefenceValue = GameObject.Find("Canvas/Middle/AddAttrPanel/Grid/Defence/Value").GetComponent<Text>();
        endPanel = GameObject.Find("Canvas/Middle/EndPanel");
        winer = GameObject.Find("Canvas/Middle/EndPanel/ResultLabel").GetComponent<Text>();
        /******************右******************/
        blackDetailPanel = GameObject.Find("Canvas/Right/BlackAttrDetail");
        b_Hp = blackDetailPanel.transform.FindChild("Grid/Hp/Value").GetComponent<Text>();
        b_Attack = blackDetailPanel.transform.FindChild("Grid/Attack/Value").GetComponent<Text>();
        b_Defence = blackDetailPanel.transform.FindChild("Grid/Defence/Value").GetComponent<Text>();
        b_Combat = blackDetailPanel.transform.FindChild("Combat/Value").GetComponent<Text>();
        redDetailPanel = GameObject.Find("Canvas/Right/RedAttrDetail");
        r_Hp = redDetailPanel.transform.FindChild("Grid/Hp/Value").GetComponent<Text>();
        r_Attack = redDetailPanel.transform.FindChild("Grid/Attack/Value").GetComponent<Text>();
        r_Defence = redDetailPanel.transform.FindChild("Grid/Defence/Value").GetComponent<Text>();
        r_Combat = redDetailPanel.transform.FindChild("Combat/Value").GetComponent<Text>();

        AddAttrCompleteEvent += HideAddAttrPanel;
        AddAttrCompleteEvent += HideAttrPanel;
    }

    public List<GameObject> chessList;
    public CreateManager createManager;
    void Start()
    {
        blackAllTime.text = "00:00";
        blackStepTime.text = "00:00";
        redAllTime.text = "00:00";
        redStepTime.text = "00:00";
        addAtrrPanel.SetActive(false);
        addHpValue.text = "50";
        addAttackValue.text = "5";
        addDefenceValue.text = "5";
        blackDetailPanel.SetActive(false);
        redDetailPanel.SetActive(false);
        SetMode(true);
    }

    void Update()
    {
        if (GameController.gameStatus == GameStatus.Going)
        {
            blackAllTime.text = GameUtil.TimeToStr(TimeManager.GetAllTime("Black"));
            redAllTime.text = GameUtil.TimeToStr(TimeManager.GetAllTime("Red"));
            blackStepTime.text = GameUtil.TimeToStr(TimeManager.GetStepTime("Black"));
            redStepTime.text = GameUtil.TimeToStr(TimeManager.GetStepTime("Red"));
        }
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
        CreateManager.Instance.InitChessBoard();
        GameController.gameStatus = GameStatus.Going;
        GameController.Instance.UpdateGameData();

        beginBtn.SetActive(false);
    }
    /// <summary>
    /// 重置棋盘所有点状态
    /// 1.开始移动时
    /// 2.取消自身选中时
    /// 3.切换选中时
    /// </summary>
    public static void ResetChessBoardPoints()
    {
        foreach (GameObject cell in cells)
        {
            cell.GetComponent<Image>().enabled = false;
        }
    }

    /// <summary>
    /// 监听棋子被选中事件
    /// </summary>
    /// <param name="chess"></param>
    public void OnChoose(GameObject chess)
    {
        curAddChess = chess;
        curAttr = GameUtil.GetChessAttrList(chess);
        UpdateAttrPanel(chess);
        bool isAdding = GameController.playing == Playing.RedAdding || GameController.playing == Playing.BlackAdding;
        addAtrrPanel.SetActive(isAdding);
        addChessName.text = isAdding ? chess.name : "";
    }

    public void UpdateAttrPanel()
    {
        if (curAddChess != null && curAttr != null)
        {
            SetAttrTexts(curAttr, curAddChess.tag);
        }
    }

    public void UpdateAttrPanel(GameObject chess)
    {
        AttrBox attrBox = GameUtil.GetChessAttrList(chess);
        SetAttrTexts(attrBox, chess.tag);
    }

    public void OnHpClick()
    {
        curAttr.Hp += 50;
        AddAttrCompleteEvent();
    }

    public void OnAttackClick()
    {
        curAttr.Attack += 5;
        AddAttrCompleteEvent();
    }

    public void OnDefenceClick()
    {
        curAttr.Defence += 5;
        AddAttrCompleteEvent();
    }

    public void HideAddAttrPanel()
    {
        addAtrrPanel.SetActive(false);
    }

    public void HideAttrPanel()
    {
        redDetailPanel.SetActive(false);
        blackDetailPanel.SetActive(false);
    }

    /// <summary>
    /// 设置属性面板
    /// </summary>
    /// <param name="attrBox">属性</param>
    /// <param name="tag">棋子阵营</param>
    public void SetAttrTexts(AttrBox attrBox, string tag)
    {
        if (attrBox.Hp == 0)
        {
            redDetailPanel.SetActive(false);
            blackDetailPanel.SetActive(false);
            return;
        }
        redDetailPanel.SetActive(tag == "Red");
        blackDetailPanel.SetActive(tag == "Black");
        if (tag == "Red")
        {
            r_Hp.text = attrBox.Hp.ToString();
            r_Attack.text = attrBox.Attack.ToString();
            r_Defence.text = attrBox.Defence.ToString();
            r_Combat.text = attrBox.Combat.ToString();
        }
        else
        {
            b_Hp.text = attrBox.Hp.ToString();
            b_Attack.text = attrBox.Attack.ToString();
            b_Defence.text = attrBox.Defence.ToString();
            b_Combat.text = attrBox.Combat.ToString();
        }
    }

    /// <summary>
    /// 点击返回事件
    /// </summary>
    public void OnBackClick()
    {
        SceneManager.LoadScene("scene1");
    }

    public void OnDestroy()
    {
        AddAttrCompleteEvent -= HideAddAttrPanel;
        AddAttrCompleteEvent -= HideAttrPanel;
    }
}