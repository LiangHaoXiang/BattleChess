using System;
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
/// 游戏时的各个状态
/// </summary>
public enum Playing
{
    None,
    OnRed,                  //到红方走
    RedAdding,              //到红方加属性
    OnBlack,                //到黑方走
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
public delegate void ChooseEventHandler(GameObject chess);
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
/// 移动完成
/// </summary>
public delegate void MoveCompleteHandler();
/// <summary>
/// 阵亡者事件
/// </summary>
/// <param name="chess">阵亡者物体</param>
public delegate void KilledEventHandler(GameObject chess);
/// <summary>
/// 加属性操作完成
/// </summary>
public delegate void AddAttrCompleteEventHandler();

public class GameDefine{

}

/*****************************下面是C#委托的部分底层源码*****************************

public static Delegate RemoveAll(Delegate source, Delegate value)
{
    Delegate newDelegate = null;
    do
    {
        newDelegate = source;
        source = Remove(source, value);
    }
    while (newDelegate != source);
    return newDelegate;
}

public static Delegate Remove(Delegate source, Delegate value)
{
    if (source == null) return null;
    if (value == null) return source;
    if (!InternalEqualTypes(source, value))
        throw new ArgumentException(Environment.GetResourceString("Arg_DlgtTypeMis"));
    return source.RemoveImpl(value);
}

protected override sealed Delegate RemoveImpl(Delegate value)
{
    MulticastDelegate v = value as MulticastDelegate;
    if (v == null) return this;
    if (v._invocationList as Object[] == null)
    {
        Object[] invocationList = _invocationList as Object[];
        if (invocationList == null)
        {
            if (this.Equals(value))
                return null;
        } else
        {
            int invocationCount = (int)_invocationCount;
            for (int i = invocationCount; --i >= 0;)
            {
                if (value.Equals(invocationList[i]))
                {
                    if (invocationCount == 2)
                    {
                        return (Delegate)invocationList[1 - i];
                    }
                    else
                    {
                        Object[] list = DeleteFromInvocationList(invocationList, invocationCount, i, 1);
                        return NewMulticastDelegate(list, invocationCount - 1, true);
                    }
                }
            }
        }
    }
    else
    {
        Object[] invocationList = _invocationList as Object[];
        if (invocationList != null)
        {
            int invocationCount = (int)_invocationCount;
            int vInvocationCount = (int)v._invocationCount;
            for (int i = invocationCount - vInvocationCount; i >= 0; i--)
            {
                if (EqualInvocationLists(invocationList, v._invocationList as Object[], i, vInvocationCount))
                {
                    if (invocationCount - vInvocationCount == 0)
                    { return null; }
                    else if (invocationCount - vInvocationCount == 1)
                    { return (Delegate)invocationList[i != 0 ? 0 : invocationCount - 1]; }
                    else
                    {
                        Object[] list = DeleteFromInvocationList(invocationList, invocationCount, i, vInvocationCount);
                        return NewMulticastDelegate(list, invocationCount - vInvocationCount, true);
                    }
                }
            }
        }
    }
    return this;
}

*******************************************************************************/
