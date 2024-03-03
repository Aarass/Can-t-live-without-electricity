using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class Tool_WireBuilder : Tool
{
    Cell previousCell;
    protected override void Setup()
    {
        previousCell = Cell.Invalid;
    }
    protected override void Run()
    {
        Cell currentCell = GridManager.Instance.IntersectNarrower(Input.mousePosition);

        if (ConditionsFulfilled(previousCell, currentCell))
            Network.Instance.AddSegment(new Wire(previousCell, currentCell, Network.Instance));

        if(currentCell != Cell.Invalid)
            previousCell = currentCell;
    }
    private bool ConditionsFulfilled(Cell start, Cell end)
    {
        if (!start || !end) return false;
        if (start == end) return false;

        Cell diff = Cell.Abs(end - start);
        if (diff.x > 1 || diff.z > 1) return false;

        return true;
    }
    protected override void ClearState()
    {
        previousCell = Cell.Invalid;
    }
}
