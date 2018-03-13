using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 士/仕
/// </summary>
public class Chess_Shi : BaseChess
{
    public override void Awake()
    {
        if (gameObject.tag == "Red")
            chineseChessName = "红仕";
        else
            chineseChessName = "黑士";
        chessName = "Shi";
        base.Awake();
    }

    void Start()
    {

    }

    public override void Update()
    {
        base.Update();
    }

    public override List<Vector2> CanMovePoints(Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess)
    {
        Vector2 currentPos = chess2Vector[gameObject];
        List<Vector2> canMovePoints = new List<Vector2>();

        if (gameObject.tag == "Red")
        {
            if (currentPos.x < 5 && currentPos.y < 2)   //45°斜向上走
            {
                Vector2 value = new Vector2(currentPos.x + 1, currentPos.y + 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x < 5 && currentPos.y > 0)   //-45°斜向下走
            {
                Vector2 value = new Vector2(currentPos.x + 1, currentPos.y - 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x > 3 && currentPos.y < 2)   //135°斜向上走
            {
                Vector2 value = new Vector2(currentPos.x - 1, currentPos.y + 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x > 3 && currentPos.y > 0)   //-135°斜向下走
            {
                Vector2 value = new Vector2(currentPos.x - 1, currentPos.y - 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }
        }

        if (gameObject.tag == "Black")
        {
            if (currentPos.x < 5 && currentPos.y < 9)   //45°斜向上走
            {
                Vector2 value = new Vector2(currentPos.x + 1, currentPos.y + 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x < 5 && currentPos.y > 7)   //-45°斜向下走
            {
                Vector2 value = new Vector2(currentPos.x + 1, currentPos.y - 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x > 3 && currentPos.y < 9)   //135°斜向上走
            {
                Vector2 value = new Vector2(currentPos.x - 1, currentPos.y + 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }

            if (currentPos.x > 3 && currentPos.y > 7)   //-135°斜向下走
            {
                Vector2 value = new Vector2(currentPos.x - 1, currentPos.y - 1);
                JudgeMovePoint(value, canMovePoints, vector2Chess);
            }
        }

        return canMovePoints;
    }

    /// <summary>
    /// 士专属判断是否可以走这个点
    /// </summary>
    /// <param name="value"></param>
    void JudgeMovePoint(Vector2 value, List<Vector2> canMovePoints, Dictionary<Vector2, GameObject> vector2Chess)
    {
        //若网格存在，即在棋盘内
        if (GameUtil.IsInChessBoard(value))
        {
            //若有棋子
            if (vector2Chess.ContainsKey(value))
            {
                GameObject otherChess = vector2Chess[value];
                if (otherChess.tag != gameObject.tag)
                    canMovePoints.Add(value);
            }
            else
                canMovePoints.Add(value);
        }
    }
}

