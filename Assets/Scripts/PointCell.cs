using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCell : MonoBehaviour
{
    [HideInInspector]
    public Vector2 point;
    [HideInInspector]
    public Transform parent;
    public static event PointCellClickEventHandler PointCellClickEvent;

    void Awake()
    {
        parent = transform.parent;
        point = GameUtil.Str2Vector(parent.name, gameObject.name);
    }
    public void OnCellClick()
    {
        PointCellClickEvent(point);
    }
}
