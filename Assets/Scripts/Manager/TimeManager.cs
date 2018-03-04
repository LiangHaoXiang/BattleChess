using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static float r_AllTime = 0.0f;       //红局时，单位s
    public static float b_AllTime = 0.0f;
    public static float r_StepTime = 0.0f;      //红步时，单位s
    public static float b_StepTime = 0.0f;

    void Awake()
    {
        r_AllTime = 60.0f * 20.0f;
        b_AllTime = 60.0f * 20.0f;
        r_StepTime = 60.0f * 0.5f;
        b_StepTime = 60.0f * 0.5f;

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
                    r_StepTime = 0;
                }
                if (r_AllTime > 0)
                    r_AllTime -= Time.deltaTime;
                else
                {
                    r_AllTime = 0;
                }
            }
            else if (GameController.playing == Playing.OnBlack || GameController.playing == Playing.BlackAdding)
            {
                if (b_StepTime > 0)
                    b_StepTime -= Time.deltaTime;
                else
                {
                    b_StepTime = 0;
                }
                if (b_AllTime > 0)
                    b_AllTime -= Time.deltaTime;
                else
                {
                    b_AllTime = 0;
                }
            }
        }
	}

    /// <summary>
    /// 重置步时
    /// </summary>
    public void ResetStepTime()
    {
        r_StepTime = 60.0f * 0.5f;
        b_StepTime = 60.0f * 0.5f;
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
