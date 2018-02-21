using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏整体状态
/// </summary>
public enum GameStatus
{
    NotBegin,               //未开始
    Going,                  //进行中
    Pause,                  //暂停
    Replay,                 //复盘
}

/// <summary>
/// 游戏进行时的各个状态
/// </summary>
public enum GameGoing
{
    OnRed,                  //到红方走
    OnBlack,                //到黑方走
    RedAdding,              //到红方加属性
    BlackAdding,            //到黑方加属性
}

/// <summary>
/// 棋子交互状态
/// </summary>
public enum ChessReciprocalState
{
    unChoosed,              //未被选择状态
    beChoosed,              //被选择中
    moving,                 //移动中
}
/// <summary>
/// 棋子所处的情境状态
/// </summary>
public enum ChessSituationState
{
    Idle,                   //安然无恙
    Attacking,              //将军状态
    BeAttacked,             //只有帅/将才拥有的被将军状态
    NoWayOut,               //无路可走状态
    Death,                  //阵亡
}

public delegate void PushEventHandler(GameObject chess);
public delegate void TakeEventHandler(GameObject chess);
public delegate void RestoreEventHandler(GameObject chess);

public delegate void ChooseEventHandler();
public delegate void EatEventHandler(GameObject chess);

public delegate void PointCellClickEventHandler(Vector2 cell);

public class GameDefine{

}
