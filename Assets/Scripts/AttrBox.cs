using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrBox : MonoBehaviour
{
    private int combat;
    private int hp;
    private int attack;
    private int defence;

    public int Combat { get { return combat; } set { combat = value; } }
    public int Hp { get { return hp; } set { hp = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defence { get { return defence; } set { defence = value; } }
}
