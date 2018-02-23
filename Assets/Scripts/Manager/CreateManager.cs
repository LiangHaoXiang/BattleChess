﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    #region 棋子预制体
    public GameObject red_Ju;
    public GameObject red_Ma;
    public GameObject red_Pao;
    public GameObject red_Shi;
    public GameObject red_Xiang;
    public GameObject red_Bing;
    public GameObject red_Shuai;

    public GameObject black_Ju;
    public GameObject black_Ma;
    public GameObject black_Pao;
    public GameObject black_Shi;
    public GameObject black_Xiang;
    public GameObject black_Zu;
    public GameObject black_Jiang;
    #endregion
    #region 各棋子实体
    [HideInInspector]
    public Transform redBoss;
    [HideInInspector]
    public Transform blackBoss;
    [HideInInspector]
    public Transform r_Ju1;    //红车1
    [HideInInspector]
    public Transform r_Ju2;    //红车2
    [HideInInspector]
    public Transform b_Ju1;    //黑车1
    [HideInInspector]
    public Transform b_Ju2;    //黑车2
    [HideInInspector]
    public Transform r_Ma1;
    [HideInInspector]
    public Transform r_Ma2;
    [HideInInspector]
    public Transform b_Ma1;
    [HideInInspector]
    public Transform b_Ma2;
    [HideInInspector]
    public Transform r_Pao1;
    [HideInInspector]
    public Transform r_Pao2;
    [HideInInspector]
    public Transform b_Pao1;
    [HideInInspector]
    public Transform b_Pao2;
    [HideInInspector]
    public Transform r_Shi1;
    [HideInInspector]
    public Transform r_Shi2;
    [HideInInspector]
    public Transform b_Shi1;
    [HideInInspector]
    public Transform b_Shi2;
    [HideInInspector]
    public Transform r_Xiang1;
    [HideInInspector]
    public Transform r_Xiang2;
    [HideInInspector]
    public Transform b_Xiang1;
    [HideInInspector]
    public Transform b_Xiang2;
    [HideInInspector]
    public Transform r_Bing1;
    [HideInInspector]
    public Transform r_Bing2;
    [HideInInspector]
    public Transform r_Bing3;
    [HideInInspector]
    public Transform r_Bing4;
    [HideInInspector]
    public Transform r_Bing5;
    [HideInInspector]
    public Transform b_Bing1;
    [HideInInspector]
    public Transform b_Bing2;
    [HideInInspector]
    public Transform b_Bing3;
    [HideInInspector]
    public Transform b_Bing4;
    [HideInInspector]
    public Transform b_Bing5;
    #endregion
    private static CreateManager instance = null;
    public static CreateManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    /// <summary>
    /// 初始化棋盘所有棋子
    /// </summary>
    public void InitChessBoard()
    {

        b_Ju1 = Create(black_Ju, 0, 9); //b_Bing1 = Create(black_Zu, 0, 6);

        //b_Ma1 = Create(black_Ma, 1, 9); b_Pao1 = Create(black_Pao, 1, 7);

        //b_Xiang1 = Create(black_Xiang, 2, 9); b_Bing2 = Create(black_Zu, 2, 6);

        //b_Shi1 = Create(black_Shi, 3, 9);

        //blackBoss = Create(black_Jiang, 4, 9); b_Bing3 = Create(black_Zu, 4, 6);

        //b_Shi1 = Create(black_Shi, 5, 9);

        //b_Xiang2 = Create(black_Xiang, 6, 9); b_Bing4 = Create(black_Zu, 6, 6);

        //b_Ma2 = Create(black_Ma, 7, 9); b_Pao2 = Create(black_Pao, 7, 7);

        b_Ju2 = Create(black_Ju, 8, 9); //b_Bing5 = Create(black_Zu, 8, 6);




        r_Ju1 = Create(red_Ju, 0, 0); //r_Bing1 = Create(red_Bing, 0, 3);

        //r_Ma1 = Create(red_Ma, 1, 0); r_Pao1 = Create(red_Pao, 1, 2);

        //r_Xiang1 = Create(red_Xiang, 2, 0); r_Bing2 = Create(red_Bing, 2, 3);

        //r_Shi1 = Create(red_Shi, 3, 0);

        //redBoss = Create(red_Shuai, 4, 0); r_Bing3 = Create(red_Bing, 4, 3);

        //r_Shi2 = Create(red_Shi, 5, 0);

        //r_Xiang2 = Create(red_Xiang, 6, 0); r_Bing4 = Create(red_Bing, 6, 3);

        //r_Ma2 = Create(red_Ma, 7, 0); r_Pao2 = Create(red_Pao, 7, 2);

        r_Ju2 = Create(red_Ju, 8, 0); //r_Bing5 = Create(red_Bing, 8, 3);

    }

    /// <summary>
    /// 生成棋子
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Transform Create(GameObject prefab, int x, int y)
    {
        GameObject go = Instantiate(prefab);
        PoolManager.Push(go);   //放入池工作区进行管理
        go.transform.position = Scene3_UI.cells[x, y].transform.position;
        go.transform.localScale = Vector3.one;
        return go.transform;
    }

    /// <summary>
    /// 返回红帅棋子
    /// </summary>
    public GameObject GetRedBoss()
    {
        return redBoss.gameObject;
    }
    /// <summary>
    /// 返回黑将棋子
    /// </summary>
    public GameObject GetBlackBoss()
    {
        return blackBoss.gameObject;
    }
}
