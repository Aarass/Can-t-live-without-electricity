using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PowerPlant : PowerSource
{
    void Awake()
    {
        color = clr;
        PowerPlantsManager.GetInstance().RegisterNewPowerPlant(this);
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    //public override bool IsConnectedTo(Segment segment)
    //{
    //    Cell cell = GridManager.Instance.WorldToCell(this.transform.position);
    //    if (cell == segment.start)
    //        return true;
    //    if (cell == segment.end)
    //        return true;
    //    if ((cell + new Cell(-1,  0)) == segment.start)
    //        return true;
    //    if ((cell + new Cell(-1,  0)) == segment.end)
    //        return true;
    //    if ((cell + new Cell(-1, -1)) == segment.start)
    //        return true;
    //    if ((cell + new Cell(-1, -1)) == segment.end)
    //        return true;
    //    if ((cell + new Cell( 0, -1)) == segment.start)
    //        return true;
    //    if ((cell + new Cell( 0, -1)) == segment.end)
    //        return true;
    //    return false;
    //}
}
