using Assets.Scripts.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Player : SingletonMonoBehaviour<GridManager>
{
    [SerializeField]
    float wires;
    [SerializeField]
    int jumpers;
    [SerializeField]
    int generators;
    [SerializeField]
    int pipes;
    [SerializeField]
    int bridges;
    [SerializeField]
    int hammers;

    public bool Charge(Bill bill)
    {
        if (this.wires < bill.wires) return false;
        if (this.jumpers < bill.jumpers) return false;
        if (this.generators< bill.generators) return false;
        if (this.pipes < bill.pipes ) return false;
        if (this.bridges < bill.bridges) return false;
        if (this.hammers < bill.hammers) return false;

        this.wires -= bill.wires;
        this.jumpers -= bill.jumpers;
        this.generators -= bill.generators;
        this.pipes -= bill.pipes;
        this.bridges -= bill.bridges;
        this.hammers -= bill.hammers;

        return true;
    }
}
public struct Bill
{
    public float wires;
    public int jumpers;
    public int generators;
    public int pipes;
    public int bridges;
    public int hammers;

    public void ZeroOut()
    {
        wires = 0;
        jumpers = 0;
        generators = 0;
        pipes = 0;
        bridges = 0;
        hammers = 0;
    }
}
