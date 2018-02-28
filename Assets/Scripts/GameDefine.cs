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
    BeTaget,                //被成为目标状态，敌人可吃
    NoWayOut,               //无路可走状态
    Death,                  //阵亡
}

public delegate void PushEventHandler(GameObject chess);
public delegate void TakeEventHandler(GameObject chess);
public delegate void RestoreEventHandler(GameObject chess);

/// <summary>
/// //选择本棋子事件，通知其他棋子为取消选择状态
/// </summary>
public delegate void ChooseEventHandler();
/// <summary>
/// 提示可以吃子
/// </summary>
/// <param name="point">传入可吃子的位置信息</param>
public delegate void TipsKillEventHandler(Vector2 point);
/// <summary>
/// 设置攻击方事件，
/// </summary>
/// <param name="chess">传自身棋子即进攻方</param>
public delegate void SetAttackerEventHandler(GameObject chess);
/// <summary>
/// 设置防守方事件，
/// </summary>
/// <param name="chess">传自身棋子即被进攻方</param>
public delegate void SetDefenderEventHandler(GameObject chess);
/// <summary>
/// 移动，选中状态的棋子才会动，其他会过滤掉
/// </summary>
/// <param name="point">传入目标位置</param>
public delegate void MoveEventHandler(Vector2 point);
/// <summary>
/// 阵亡者事件
/// </summary>
/// <param name="chess">阵亡者物体</param>
public delegate void KilledEventHandler(GameObject chess);
/// <summary>
/// 重置棋子交互状态
/// </summary>
public delegate void ResetReciprocalStateEventHandler();

public class GameDefine{

}
