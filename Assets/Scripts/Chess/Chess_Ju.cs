using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chess_Ju : BaseChess
{
    public override void Awake()
    {
        if (gameObject.tag == "Red")
            chineseChessName = "红车";
        else
            chineseChessName = "黑车";
        chessName = "Ju";
        base.Awake();
    }

    void Start()
    {

    }

    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// 该棋子能移动的所有位置,返回的是平面二维坐标，如(0,0)、(3,5)、(6,6)等
    /// </summary>
    /// <returns></returns>
    public override List<Vector2> CanMovePoints(Dictionary<GameObject, Vector2> chess2Vector, Dictionary<Vector2, GameObject> vector2Chess)
    {
        Vector2 currentPos = chess2Vector[gameObject];
        List<Vector2> canMovePoints = new List<Vector2>();

        for (int i = (int)currentPos.x - 1; i >= 0; i--)     //向左检索
        {
            Vector2 value = new Vector2(i, currentPos.y);
            bool findOtherChess = false;
            JudgeMovePoint(value, ref findOtherChess, canMovePoints, vector2Chess);
            if (findOtherChess) break;
        }

        for (int i = (int)currentPos.x + 1; i <= 8; i++)    //向右检索
        {
            Vector2 value = new Vector2(i, currentPos.y);
            bool findOtherChess = false;
            JudgeMovePoint(value, ref findOtherChess, canMovePoints, vector2Chess);
            if (findOtherChess) break;
        }

        for (int i = (int)currentPos.y + 1; i <= 9; i++)     //向上检索
        {
            Vector2 value = new Vector2(currentPos.x, i);
            bool findOtherChess = false;
            JudgeMovePoint(value, ref findOtherChess, canMovePoints, vector2Chess);
            if (findOtherChess) break;
        }

        for (int i = (int)currentPos.y - 1; i >= 0; i--)     //向下检索
        {
            Vector2 value = new Vector2(currentPos.x, i);
            bool findOtherChess = false;
            JudgeMovePoint(value, ref findOtherChess, canMovePoints, vector2Chess);
            if (findOtherChess) break;
        }

        return canMovePoints;
    }
    /// <summary>
    /// 车专属判断是否可以走这个点
    /// </summary>
    /// <param name="value"></param>
    void JudgeMovePoint(Vector2 value, ref bool findOtherChess, List<Vector2> canMovePoints, Dictionary<Vector2, GameObject> vector2Chess)
    {
        if (vector2Chess.ContainsKey(value))    //若有其他棋子，那就停下来
        {
            GameObject otherChess = vector2Chess[value];
            if(otherChess.tag != gameObject.tag)    //判断是否为己方棋子 tag在编辑器里设置为"Red"或"Black"
            {
                canMovePoints.Add(value);
            }
            findOtherChess = true;
        }
        else
            canMovePoints.Add(value);
    }
}

