using UnityEngine;

public class Wire : Segment
{
    GameObject geometry;
    public Wire(Cell start, Cell end, Network network) : base(start, end, network) { }

    public override void OnSuccessfulCreation()
    {
        CreateGeometry();
        ApplyColor();
    }
    private void CreateGeometry()
    {
        Cell diff = end - start;

        float angle = Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = 360 + angle;

        int index = Mathf.FloorToInt(angle / 45f);

        GameObject prefab = index % 2 == 0 ? AssetsServer.Instance.shortWire : AssetsServer.Instance.longWire;
        Vector3 position = GridManager.Instance.CellToWorld(start);
        float rotation = index / 2 * -90;

        GameObject geometry = UnityEngine.Object.Instantiate(prefab, position, Quaternion.Euler(0, rotation, 0));
        this.geometry = geometry;
    }
    public override void ApplyColor()
    {
        if (geometry == null) return;
        geometry.GetComponent<Renderer>().material.color = Assets.Helper.PowerColorToColor(path.color);
    }
    public override void Destroy()
    {
        path.RemoveSegment(this);
        network.RemoveSegment(this);

        if (geometry)
            UnityEngine.Object.Destroy(geometry);
    }
    bool IsDiagonal()
    {
        return start.x != end.x && start.z != end.z;
    }
    Wire Mirror()
    {
        Cell start = this.start;
        Cell end = this.end;
        start.z = this.end.z;
        end.z = this.start.z;
        return new Wire(start, end, network);
    }
    public override bool IsIntersecting(Wire other)
    {
        if (start == other.start || start == other.end ||
                end == other.start || end == other.end)
            return true;

        if (IsDiagonal() && other.IsDiagonal())
        {
            Wire mirror = other.Mirror();
            return IsOverlaping(mirror);
        }
        return false;
    }
    public override bool IsIntersecting(Pipe other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
    }
    public override bool IsIntersecting(Invisible other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
    }
    public override bool RequestCorrectIntersectionCheck(Segment caller)
    {
        return caller.IsIntersecting(this);
    }
}