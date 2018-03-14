using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance = null;
    public static TimeManager Instance { get { return instance; } }

    private static float allTime;
    private static float stepTime;
    public static float r_AllTime = 0.0f;       //红局时，单位s
    public static float b_AllTime = 0.0f;
    public static float r_StepTime = 0.0f;      //红步时，单位s
    public static float b_StepTime = 0.0f;

    public static event GameOverEventHandlerWithParam TimeUpEventWithParam;
    public static event GameOverEventHandler TimeUpEvent;

    void Awake()
    {
        if (instance == null)
            instance = this;
        allTime = 60.0f * 0.3f;
        stepTime = 60.0f * 0.1f;

        r_AllTime = allTime;
        b_AllTime = allTime;
        r_StepTime = stepTime;
        b_StepTime = stepTime;

        Scene3_UI.AddAttrCompleteEvent += ResetStepTime;
    }	

	void Update ()
    {
        if (GameController.gameStatus == GameStatus.Going)
        {
            if (GameController.playing == Playing.OnRed || GameController.playing == Playing.RedAdding)
            {
                if (r_StepTime > 0) 
                    r_StepTime -= Time.deltaTime;
                else
                {
                    GameController.gameStatus = GameStatus.End;
                    TimeUpEventWithParam("Black");
                    TimeUpEvent();
                    r_StepTime = 0;
                }
                if (r_AllTime > 0)
                    r_AllTime -= Time.deltaTime;
                else
                {
                    GameController.gameStatus = GameStatus.End;
                    TimeUpEventWithParam("Black");
                    TimeUpEvent();
                    r_AllTime = 0;
                }
            }
            else if (GameController.playing == Playing.OnBlack || GameController.playing == Playing.BlackAdding)
            {
                if (b_StepTime > 0)
                    b_StepTime -= Time.deltaTime;
                else
                {
                    GameController.gameStatus = GameStatus.End;
                    TimeUpEventWithParam("Red");
                    TimeUpEvent();
                    b_StepTime = 0;
                }
                if (b_AllTime > 0)
                    b_AllTime -= Time.deltaTime;
                else
                {
                    GameController.gameStatus = GameStatus.End;
                    TimeUpEventWithParam("Red");
                    TimeUpEvent();
                    b_AllTime = 0;
                }
            }
        }
	}
    /// <summary>
    /// 重置局时与步时
    /// </summary>
    public static void ResetAllTime()
    {
        r_AllTime = allTime;
        b_AllTime = allTime;
        ResetStepTime();
    }
    /// <summary>
    /// 重置步时
    /// </summary>
    public static void ResetStepTime()
    {
        r_StepTime = r_AllTime < stepTime ? r_AllTime : stepTime;  //当局时小于规定步时，那么步时就等于局时
        b_StepTime = b_AllTime < stepTime ? b_AllTime : stepTime;
    }

    /// <summary>
    /// 返回局时
    /// </summary>
    /// <param name="tag">Red/Black</param>
    /// <returns></returns>
    public static float GetAllTime(string tag)
    {
        if (tag == "Red")
            return r_AllTime;
        else if (tag == "Black")
            return b_AllTime;
        else
            throw new System.Exception("Error to invalid tag");
    }

    /// <summary>
    /// 返回步时
    /// </summary>
    /// <param name="tag">Red/Black</param>
    /// <returns></returns>
    public static float GetStepTime(string tag)
    {
        if (tag == "Red")
            return r_StepTime;
        else if (tag == "Black")
            return b_StepTime;
        else
            throw new System.Exception("Error to invalid tag");
    }

    public void OnDestroy()
    {
        Scene3_UI.AddAttrCompleteEvent -= ResetStepTime;
    }
}
