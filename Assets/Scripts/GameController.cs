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

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        //Debug.Log("啊啊啊");
        //if (scene.isLoaded)
        //{
            IsBattle = false;
            gameStatus = GameStatus.NotBegin;
            playing = Playing.None;
            GameCache.ClearMaps();
            GameCache.ClearChessVectorDic();

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
        //}
    }

    void Start () {
		
	}
	
	void Update () {
		
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
    /// 更新游戏信息
    /// </summary>
    /// <returns></returns>
    public void UpdateGameData()
    {
        if (IsBattle == true)
        {
            //战斗结束后再更新数据
            IsBattle = false;
            GameObject loser = GameUtil.Battle(attacker, defender);
            KilledEvent(loser);
        }
        GameCache.UpdateChessData();
        GameCache.SetMaps();
        if (gameStatus == GameStatus.Going)
        {
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
            playing = Playing.OnRed;
        }
        else if (playing == Playing.OnRed)
        {
            playing = Playing.RedAdding;
        }
        else if (playing == Playing.RedAdding)
        {
            playing = Playing.OnBlack;
        }
        else if (playing == Playing.OnBlack)
        {
            playing = Playing.BlackAdding;
        }
        else if (playing == Playing.BlackAdding)
        {
            playing = Playing.OnRed;
        }
    }

    public void OnDestroy()
    {
        Debug.Log("销毁GameController");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
