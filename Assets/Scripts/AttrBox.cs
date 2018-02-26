using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttrType
{
    Hp = 0,
    Attack = 1,
    Defence = 2,
}

public class AttrBox : MonoBehaviour
{
    private int hp;
    private int attack;
    private int defence;

    public int Hp { get { return hp; } set { hp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defence { get { return defence; } set { defence = value; } }

    public int Combat
    {
        get
        {
            return GameUtil.CalCombat(Hp, Attack, Defence);
        }
    }

    public void SetAttrList(List<int> attrList)
    {
        Hp = attrList[(int)AttrType.Hp];
        Attack = attrList[(int)AttrType.Attack];
        Defence = attrList[(int)AttrType.Defence];
    }
}
