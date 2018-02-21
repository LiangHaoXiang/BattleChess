using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseChess : MonoBehaviour
{
    public static event ChooseEventHandler ChooseEvent;//选择本棋子事件，通知其他棋子为取消选择状态
    public static event TipsKillEventHandler TipsKillEvent;
    public static event EatEventHandler EatEvent;
    public static event MoveEventHandler MoveEvent;

    //protected CreateManager createManager;
    protected ChessReciprocalState chessReciprocalState;    //棋子交互状态
    protected ChessSituationState chessSituationState;      //棋子形势状态

    public virtual void Awake()
    {
        //createManager = GameObject.Find("CreateManager").GetComponent<CreateManager>();
        chessReciprocalState = ChessReciprocalState.unChoosed;
        chessSituationState = ChessSituationState.Idle;
        PoolManager.PushEvent += SubscribeEvents;//棋子被创建时就该订阅这一堆事件
        PoolManager.TakeEvent += SubscribeEvents;
        PoolManager.RestoreEvent += CancelSubscribeEvents;

        PointCell.PointCellClickEvent += Move;
        MoveEvent += Move;
    }

    public virtual void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Move();
        //}
    }
    /// <summary>
    /// 通过鼠标点击来移动
    /// </summary>
    public void Move(Vector2 point)
    {
        if (chessReciprocalState == ChessReciprocalState.beChoosed)
        {
            Vector2[] canMovePoints = CanMovePoints(GameCache.Chess2Vector, GameCache.Vector2Chess).ToArray();
            Vector3[] canMovePos = new Vector3[canMovePoints.Length];

            if (canMovePoints.Length > 0)
            {
                for (int i = 0; i < canMovePoints.Length; i++)
                {
                    if(GameUtil.CompareVector2(point, canMovePoints[i]) == true)
                    {
                        int x = (int)point.x;
                        int y = (int)point.y;
                        Vector3 target = Scene3_UI.cells[x, y].transform.position;
                        
                        chessReciprocalState = ChessReciprocalState.moving;//这里在移动的时候时间是0.1秒，这个时间段的状态改成moving状态，还需要改进，否则会有bug
                        iTween.MoveTo(gameObject, iTween.Hash("time", 0.1f, "position", target,
                                                            "easetype", iTween.EaseType.linear));
                        break;
                    }
                    else if (chessReciprocalState != ChessReciprocalState.moving)
                    {
                        CancelChoose();
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
        Vector2[] canMovePoints = CanMovePoints(chess2Vector, vector2Chess).ToArray();
        //真正的移动
        if (realMove)
        {

        }
        else//假设移动
        {
            //for (int i = 0; i < canMovePoints.Length; i++)
            //{
            //    if (target == canMovePoints[i])
            //    {
            //        //假设移动完后，获取移动后的棋局状况 然后再检测有没有将军
            //        ArrayList moveAssumptionData = CalculateUtil.MoveAssumption(gameObject, target);
            //        //这里假设后的检测将军需要检测是否是我方受将军，是则不允许这么走
            //        //.......TODO
            //        Chess_Boss.DetectBeAttacked((Dictionary<GameObject, Vector2>)moveAssumptionData[0], (Dictionary<Vector2, GameObject>)moveAssumptionData[1]);
            //    }
            //}
        }
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
    /// 吃
    /// </summary>
    public void Eat(GameObject chess)
    {
        if (chess == gameObject)
        {
            //CancelSubscribeEvents();//需要取消订阅事件，否则回收物体后可能会空引用
            Killed();
        }
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
    public void Killed()
    {
        //播放音效

        //被杀 回收
        PoolManager.Restore(gameObject);
    }
    /// <summary>
    /// 判断是否会将军
    /// </summary>
    public bool DetectJiangJun(Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess)
    {
        //就是判断当前可移动的点包含将军的位置
        Vector2[] canMovePoints = CanMovePoints(chess2Vector, vector2Chess).ToArray();

        for (int i = 0; i < canMovePoints.Length; i++)
        {
            //if (GetComponent<ChessCamp>().camp == Camp.Red)
            //{
            //    if (canMovePoints[i] == chess2Vector[createManager.GetBlackBoss()])
            //    {
            //        Debug.Log("将军，黑方注意");
            //        chessSituationState = ChessSituationState.Attacking;
            //        createManager.GetBlackBoss().GetComponent<Chess_Boss>().chessSituationState = ChessSituationState.BeAttacked;
            //        return true;
            //    }
            //}
            //else
            //{
            //    if (canMovePoints[i] == chess2Vector[createManager.GetRedBoss()])
            //    {
            //        Debug.Log("将军，红方注意");
            //        chessSituationState = ChessSituationState.Attacking;
            //        createManager.GetRedBoss().GetComponent<Chess_Boss>().chessSituationState = ChessSituationState.BeAttacked;
            //        return true;
            //    }
            //}
        }
        chessSituationState = ChessSituationState.Idle;
        return false;
    }

    /// <summary>
    /// 棋子点击事件
    /// </summary>
    public void ChesseClicked()
    {
        //if ((GameController.whoWalk == 着法状态.到红方走 && GetComponent<ChessCamp>().camp == Camp.Red) ||
        //    (GameController.whoWalk == 着法状态.到黑方走 && GetComponent<ChessCamp>().camp == Camp.Black))
        //{
        if(chessSituationState == ChessSituationState.BeTaget) //若被标记为红点，就是要吃你
        {
            MoveEvent(GameCache.Chess2Vector[gameObject]);
            EatEvent(gameObject);            
        }
        else if (chessReciprocalState == ChessReciprocalState.unChoosed) //若还没被选中
        {
            //ChooseEvent();
            BeChoosed();
        }
        else
        {
            CancelChoose();
        }
        //}
    }
    /// <summary>
    /// 被选中
    /// </summary>
    public void BeChoosed()
    {
        //有白边圈住
        transform.FindChild("baibian").gameObject.SetActive(true);

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

    /// <summary>
    /// 取消选中
    /// </summary>
    public void CancelChoose()
    {
        //将被选中时的所有变化还原
        Reset();
        for (int i = 0; i < PoolManager.work_List.Count; i++)
        {
            PoolManager.work_List[i].transform.FindChild("redpoint").gameObject.SetActive(false);
        }
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
        foreach(GameObject cell in Scene3_UI.cells)
        {
            cell.GetComponent<Image>().enabled = false;
        }
        ResetChessReciprocalState();
        ResetChessSituationState();
    }
    /// <summary>
    /// 订阅一堆的事件
    /// </summary>
    /// <param name="chess">增加判断是否是自身，因为这个方法在每个派生类中都添加订阅了，事件触发时只执行自身的方法，其他方法没用</param>
    public void SubscribeEvents(GameObject chess)
    {
        if (chess == gameObject)
        {
            ChooseEvent += new ChooseEventHandler(CancelChoose);//订阅取消选择事件
            TipsKillEvent += TipsBeTarget;
            EatEvent += new EatEventHandler(Eat);               //订阅吃事件
            //GameController.ResetChessReciprocalStateEvent += CancelChoose; //订阅重置棋子状态事件
            //Chess_Boss.DetectBeAttackedEvent += DetectJiangJun;            //订阅检测将军事件
        }
    }
    /// <summary>
    /// 取消订阅一堆的事件
    /// </summary>
    /// <param name="chess">增加判断是否是自身，因为这个方法在每个派生类中都添加订阅了，事件触发时只执行自身的方法，其他方法没用</param>
    public void CancelSubscribeEvents(GameObject chess)
    {
        if (chess == gameObject)
        {
            EatEvent -= Eat;
            TipsKillEvent -= TipsBeTarget;
            ChooseEvent -= CancelChoose;
            //GameController.ResetChessReciprocalStateEvent -= CancelChoose;
            //Chess_Boss.DetectBeAttackedEvent -= DetectJiangJun;
        }
    }
}
