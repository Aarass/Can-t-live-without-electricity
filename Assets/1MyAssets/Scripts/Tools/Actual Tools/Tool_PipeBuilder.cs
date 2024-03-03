using UnityEngine;

public class Tool_PipeBuilder : Tool
{
    Cell previousCell;
    protected override void Setup()
    {
        previousCell = Cell.Invalid;
    }
    protected override void Run()
    {
        Cell cell = GridManager.Instance.IntersectNarrower(Input.mousePosition);

        if (previousCell == Cell.Invalid)
        {
            previousCell = cell;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (ConditionsFulfilled(previousCell, cell))
            {
                Network.Instance.AddSegment(new Pipe(previousCell, cell, Network.Instance));
            }
        }
    }
    private bool ConditionsFulfilled(Cell start, Cell end)
    {
        if (!start || !end) return false;
        if (start == end) return false;
        return true;
    }
    protected override void ClearState() 
    {
        previousCell = Cell.Invalid;
    }
}