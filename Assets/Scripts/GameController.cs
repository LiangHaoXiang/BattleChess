using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool hadLoad = false;
    private static GameController instance = null;
    public static GameController Instance { get { return instance; } }

    public static event KilledEventHandler KilledEvent;
    public static event UpdateGameDataCompleteEventHandler UpdateGameDataCompleteEvent;
    public static int step = 0;                         //第几步
    public static bool IsBattle = false;                //是否发生战斗
    private static GameObject attacker;                 //攻击方
    private static GameObject defender;                 //被攻击方
    public static GameStatus gameStatus;                //游戏状态
    public static Playing playing;                      //游戏时状态

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;   //加载场景事件监听
        if (hadLoad == false)
        {
            hadLoad = true;
            DontDestroyOnLoad(gameObject);  //就算切换回场景1也不会执行，只执行最开始的那一遍
        }
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        IsBattle = false;
        step = 0;
        gameStatus = GameStatus.NotBegin;
        playing = Playing.None;
        GameCache.ClearCache();

        if (scene.name.Equals("scene3(Main)"))  //若主场景加载完毕并切换到主场景
        {
            Debug.Log("主场景已加载完毕并切换到主场景");  //切换场景时，再次进入主场景，加载了两次
            Scene3_UI.AddAttrCompleteEvent += UpdateBout;
        }
        else
        {
            BaseChess.SetAttackerEvent -= SetAttacker;
            BaseChess.SetDefenderEvent -= SetDefender;
            Scene3_UI.AddAttrCompleteEvent -= UpdateBout;
        }
    }

    public static void SetAttacker(GameObject chess)
    {
        attacker = chess;
    }

    public static void SetDefender(GameObject chess)
    {
        defender = chess;
    }

    /// <summary>
    /// 更新游戏
    /// </summary>
    /// <returns></returns>
    public void UpdateGame()
    {
        if (gameStatus == GameStatus.Going)
        {
            if (IsBattle == true)
            {
                //战斗结束后再更新数据
                IsBattle = false;
                GameObject loser = GameUtil.Battle(attacker, defender);
                //这里再写入一个映射 阵亡者与步数的映射，复盘用到
                GameCache.SetLoserStepDic(loser, step);
                KilledEvent(loser);
            }
            UpdateBout();
        }
    }

    /// <summary>
    /// 更新回合
    /// </summary>
    public void UpdateBout()
    {
        if (playing == Playing.None)
        {
            UpdateGameData();
            playing = Playing.OnRed;
        }
        else if (playing == Playing.OnRed)
        {
            playing = Playing.RedAdding;
        }
        else if (playing == Playing.RedAdding)
        {
            UpdateGameData();
            playing = Playing.OnBlack;
        }
        else if (playing == Playing.OnBlack)
        {
            playing = Playing.BlackAdding;
        }
        else if (playing == Playing.BlackAdding)
        {
            UpdateGameData();
            playing = Playing.OnRed;
        }
    }

    public void UpdateGameData()
    {
        GameCache.UpdateChessData();
        GameCache.SetMaps();
        GameCache.SetAttrMaps();
        UpdateGameDataCompleteEvent();
        step++;
    }

    public static void BeginGame()
    {
        IsBattle = false;
        step = 0;
        CreateManager.Instance.InitChessBoard();
        gameStatus = GameStatus.Going;
        Instance.UpdateGame();
    }

    /// <summary>
    /// 重置游戏数据
    /// </summary>
    public static void ResetGame()
    {
        IsBattle = false;
        step = 0;
        gameStatus = GameStatus.NotBegin;
        playing = Playing.None;
        GameCache.ClearCache();
    }

    /// <summary>
    /// 复盘模式时游戏处理
    /// </summary>
    public static void ReplayModeGame()
    {
        IsBattle = false;
        step = 0;
        gameStatus = GameStatus.Replay;
        playing = Playing.None;
        CreateManager.Instance.InitChessBoard();
    }

    public void OnDestroy()
    {
        Debug.Log("销毁GameController");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
