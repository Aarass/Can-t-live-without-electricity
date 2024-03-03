using System;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Eraser : Tool
{
    protected override void Run()
    {
        Cell cell = GridManager.Instance.Intersect(Input.mousePosition);
        Network.Instance.RemoveSegments(cell);
    }
    protected override void Setup() { }
    protected override void ClearState() { }
}