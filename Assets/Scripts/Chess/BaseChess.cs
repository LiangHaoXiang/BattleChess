using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseChess : MonoBehaviour
{
    public static event ChooseEventHandler ChooseEvent;//选择本棋子事件，通知其他棋子为取消选择状态
    public static event TipsKillEventHandler TipsKillEvent;
    public static event SetAttackerEventHandler SetAttackerEvent;
    public static event SetDefenderEventHandler SetDefenderEvent;
    public static event MoveEventHandler MoveEvent;
    public static event MoveCompleteHandler MoveCompleteEvent;

    protected CreateManager createManager;
    protected ChessReciprocalState chessReciprocalState;    //棋子交互状态
    protected ChessSituationState chessSituationState;      //棋子形势状态

    public static int count = 0;

    [HideInInspector]
    public string chineseChessName;
    [HideInInspector]
    public string chessName;
    [HideInInspector]
    public AttrBox attrBox;

    public virtual void Awake()
    {
        createManager = CreateManager.Instance;
        chessReciprocalState = ChessReciprocalState.unChoosed;
        chessSituationState = ChessSituationState.Idle;
        PoolManager.PushEvent += SubscribeEvents;//棋子被创建时就该订阅这一堆事件
        PoolManager.TakeEvent += SubscribeEvents;
        PoolManager.RestoreEvent += CancelSubscribeEvents;

        gameObject.AddComponent<AttrBox>();
        attrBox = gameObject.GetComponent<AttrBox>();
        attrBox.SetAttrList(ChessConfig.GetAttrList(chessName));
    }

    public virtual void Update()
    {

    }
    /// <summary>
    /// 通过鼠标点击来移动
    /// </summary>
    public void Move(Vector2 point)
    {
        if (chessReciprocalState == ChessReciprocalState.beChoosed)
        {
            Vector2[] canMovePoints = CanMovePoints(GameCache.Chess2Vector, GameCache.Vector2Chess).ToArray();

            if (canMovePoints.Length > 0)
            {
                for (int i = 0; i < canMovePoints.Length; i++)
                {
                    if(GameUtil.CompareVector2(point, canMovePoints[i]) == true)
                    {
                        if(GameController.IsBattle == true)
                        {
                            SetAttackerEvent(gameObject);//派发事件设置攻击者
                        }
                        int x = (int)point.x;
                        int y = (int)point.y;
                        Vector3 target = Scene3_UI.cells[x, y].transform.position;
                        
                        iTween.MoveTo(gameObject, iTween.Hash("time", 0.1f, "position", target,
                            "easetype", iTween.EaseType.linear, 
                            "onstart", "OnMoveStart",
                            "onstarttarget", gameObject,
                            "oncomplete", "OnMoveComplete", 
                            "oncompletetarget", gameObject));
                        break;
                    }
                    else if (chessReciprocalState != ChessReciprocalState.moving)
                    {
                        CancelChoose();
                        Scene3_UI.ResetChessBoardPoints();
                    }
                    else
                    {
                        CancelChoose();
                        Scene3_UI.ResetChessBoardPoints();
                    }
                }
            }
            else
            {
                CancelChoose();
            }
        }
    }

    /// <summary>
    /// 通过AI来移动
    /// </summary>
    /// <param name="chess2Vector"></param>
    /// <param name="vector2Chess"></param>
    /// <param name="target">移动的目标位置</param>
    /// <param name="realMove">是真的移动还是假设移动？</param>
    public void MoveByAI(Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess, Vector2 target, bool realMove)
    {
        //Vector2[] canMovePoints = CanMovePoints(chess2Vector, vector2Chess).ToArray();
        ////真正的移动
        //if (realMove)
        //{

        //}
        //else//假设移动
        //{
        //    for (int i = 0; i < canMovePoints.Length; i++)
        //    {
        //        if (target == canMovePoints[i])
        //        {
        //            //假设移动完后，获取移动后的棋局状况 然后再检测有没有将军
        //            ArrayList moveAssumptionData = CalculateUtil.MoveAssumption(gameObject, target);
        //            //这里假设后的检测将军需要检测是否是我方受将军，是则不允许这么走
        //            //.......TODO
        //            Chess_Boss.DetectBeAttacked((Dictionary<GameObject, Vector2>)moveAssumptionData[0], (Dictionary<Vector2, GameObject>)moveAssumptionData[1]);
        //        }
        //    }
        //}
    }
    /// <summary>
    /// 提示可吃事件触发，自身状态被标记为可吃
    /// </summary>
    /// <param name="point"></param>
    public void TipsBeTarget(Vector2 point)
    {
        if (GameUtil.CompareVector2(GameCache.Chess2Vector[gameObject], point))
            chessSituationState = ChessSituationState.BeTaget;
    }
    /// <summary>
    /// 根据棋局信息，该棋子能移动的所有位置,返回的是平面二维坐标，如(0,0)、(3,5)、(6,6)等
    /// </summary>
    /// <param name="chess2Vector">棋局信息</param>
    /// <param name="vector2Chess">棋局信息</param>
    /// <returns></returns>
    public abstract List<Vector2> CanMovePoints(Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess);    
    /// <summary>
    /// 被杀
    /// </summary>
    public void Killed(GameObject chess)
    {
        if (chess == gameObject)
        {
            PoolManager.Restore(gameObject);//被杀 回收
        }
    }

    public void OnMoveStart()
    {
        chessReciprocalState = ChessReciprocalState.moving;
        Scene3_UI.ResetChessBoardPoints();
    }

    public void OnMoveComplete()
    {
        MoveCompleteEvent();
    }

    /// <summary>
    /// 棋子点击事件
    /// </summary>
    public virtual void ChesseClicked()
    {
        if (GameController.playing == Playing.OnRed)
        {
            if (gameObject.tag == "Red")
            {
                if (chessReciprocalState == ChessReciprocalState.unChoosed) //若还没被选中
                {
                    ChooseEvent(gameObject);
                    BeChoosed(false);
                }
                else
                {
                    CancelChoose();
                    Scene3_UI.ResetChessBoardPoints();
                }
            }
            else
            {
                if (chessSituationState == ChessSituationState.BeTaget)
                {//若被选中且被标记为红点，就是有人要吃你，把自身物体派发出去
                    GameController.IsBattle = true;
                    SetDefenderEvent(gameObject);   //将自身防守方派发出去
                    MoveEvent(GameCache.Chess2Vector[gameObject]);
                }
                else return;
            }
        }
        else if (GameController.playing == Playing.OnBlack)
        {
            if (gameObject.tag == "Black")
            {
                if (chessReciprocalState == ChessReciprocalState.unChoosed) //若还没被选中
                {
                    ChooseEvent(gameObject);
                    BeChoosed(false);
                }
                else
                {
                    CancelChoose();
                    Scene3_UI.ResetChessBoardPoints();
                }
            }
            else
            {
                if (chessSituationState == ChessSituationState.BeTaget)
                {//若被选中且被标记为红点，就是有人要吃你，把自身物体派发出去
                    GameController.IsBattle = true;
                    SetDefenderEvent(gameObject);   //将自身防守方派发出去
                    MoveEvent(GameCache.Chess2Vector[gameObject]);
                }
                else return;
            }
        }
        else if (GameController.playing == Playing.RedAdding)
        {
            if (gameObject.tag == "Red")
            {
                ChooseEvent(gameObject);
                BeChoosed(true);
            }
            else return;
        }
        else if (GameController.playing == Playing.BlackAdding)
        {
            if (gameObject.tag == "Black")
            {
                ChooseEvent(gameObject);
                BeChoosed(true);
            }
            else return;
        }
    }
    /// <summary>
    /// 被选中
    /// </summary>
    /// <param name="isAdding">是否在加属性</param>
    public void BeChoosed(bool isAdding)
    {
        transform.FindChild("baibian").gameObject.SetActive(true);  //有白边圈住
        if (!isAdding)
        {
            Scene3_UI.ResetChessBoardPoints();
            //可以提示出该棋子能移动的所有位置
            Vector2[] canMovePoints = CanMovePoints(GameCache.Chess2Vector, GameCache.Vector2Chess).ToArray();
            for (int i = 0; i < canMovePoints.Length; i++)
            {
                int x = (int)canMovePoints[i].x;
                int y = (int)canMovePoints[i].y;

                Scene3_UI.cells[x, y].GetComponent<Image>().enabled = true;
                //若可移动点上存在其他棋子，那肯定就是敌方棋子了，提示可以击杀之
                if (GameCache.Vector2Chess.ContainsKey(canMovePoints[i]))
                {
                    GameCache.Vector2Chess[canMovePoints[i]].transform.FindChild("redpoint").gameObject.SetActive(true);
                    TipsKillEvent(canMovePoints[i]);
                }
            }
            chessReciprocalState = ChessReciprocalState.beChoosed;
        }
    }

    public void CancelChoose(GameObject chess)
    {
        if (gameObject != chess)
        {
            Reset();    //将被选中时的所有变化还原
        }
    }

    public void CancelChoose()
    {    
        Reset(); //将被选中时的所有变化还原
    }

    /// <summary>
    /// 重置棋子交互状态
    /// </summary>
    public void ResetChessReciprocalState()
    {
        if (chessReciprocalState != ChessReciprocalState.unChoosed)
            chessReciprocalState = ChessReciprocalState.unChoosed; 
    }
    /// <summary>
    /// 重置棋子情境状态
    /// </summary>
    public void ResetChessSituationState()
    {
        if (chessSituationState != ChessSituationState.Idle)
            chessSituationState = ChessSituationState.Idle;
    }

    protected void Reset()
    {
        transform.FindChild("baibian").gameObject.SetActive(false);
        transform.FindChild("redpoint").gameObject.SetActive(false);
        ResetChessReciprocalState();
        ResetChessSituationState();
    }
    /// <summary>
    /// 订阅一堆的事件
    /// </summary>
    /// <param name="chess">增加判断是否是自身，因为这个方法在每个派生类中都添加订阅了，事件触发时只执行自身的方法，其他方法没用</param>
    public virtual void SubscribeEvents(GameObject chess)
    {
        if (chess == gameObject)
        {
            ChooseEvent += new ChooseEventHandler(CancelChoose);//订阅 取消选择事件
            TipsKillEvent += TipsBeTarget;
            MoveEvent += Move;
            GameController.KilledEvent += Killed;
            PointCell.PointCellClickEvent += Move;
            MoveCompleteEvent += CancelChoose; //订阅重置棋子状态事件
            Scene3_UI.AddAttrCompleteEvent += CancelChoose;
            Scene3_UI.UndoEvent += Reset;
            Scene3_UI.ReplayModeEvent += Reset;
            TimeManager.TimeUpEvent += Reset;
        }
    }
    /// <summary>
    /// 取消订阅一堆的事件
    /// </summary>
    /// <param name="chess">增加判断是否是自身，因为这个方法在每个派生类中都添加订阅了，事件触发时只执行自身的方法，其他方法没用</param>
    public virtual void CancelSubscribeEvents(GameObject chess)
    {
        if (chess == gameObject)
        {
            PointCell.PointCellClickEvent -= Move;
            MoveEvent -= Move;
            GameController.KilledEvent -= Killed;
            TipsKillEvent -= TipsBeTarget;
            ChooseEvent -= CancelChoose;
            MoveCompleteEvent -= CancelChoose;
            Scene3_UI.AddAttrCompleteEvent -= CancelChoose;
            Scene3_UI.UndoEvent -= Reset;
            Scene3_UI.ReplayModeEvent -= Reset;
            TimeManager.TimeUpEvent -= Reset;
        }
    }

    public void OnDestroy()
    {
        CancelSubscribeEvents(gameObject);
        //切换场景时，棋子会被销毁，取消Awake函数添加订阅的事件
        PoolManager.PushEvent -= SubscribeEvents;
        PoolManager.TakeEvent -= SubscribeEvents;
        PoolManager.RestoreEvent -= CancelSubscribeEvents;
    }
}
