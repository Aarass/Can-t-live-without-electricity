using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Invisible : Segment
{
    readonly Teleport teleport;
    public Invisible(Cell start, Cell end, Network network, Teleport teleport) : base(start, end, network) 
    {
        this.teleport = teleport;
    }

    public override void OnSuccessfulCreation()
    {

    }

    public override void ApplyColor()
    {
        teleport.ApplyColor();
    }

    public override void Destroy() 
    {

    }

    public override bool IsIntersecting(Wire other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
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
