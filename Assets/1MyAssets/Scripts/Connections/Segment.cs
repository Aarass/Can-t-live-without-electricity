using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public abstract class Segment : IEquatable<Segment>
{
    public Network network;
    public Path path;
    public Cell start;
    public Cell end;
    public Segment(Cell start, Cell end, Network network)
    {
        this.path = null;
        this.start = start;
        this.end = end;
        this.network = network;
    }
    public abstract void OnSuccessfulCreation();
    public abstract void Destroy();
    public abstract void ApplyColor();
    public abstract bool RequestCorrectIntersectionCheck(Segment caller);
    public abstract bool IsIntersecting(Wire other);
    public abstract bool IsIntersecting(Pipe other);
    public abstract bool IsIntersecting(Invisible other);
    public bool IsIntersecting(Segment other)
    {
        return other.RequestCorrectIntersectionCheck(this);
    }
    public bool IsOverlaping(Segment segment)
    {
        return ((this.start == segment.start && this.end == segment.end) ||
                (this.end == segment.start && this.start == segment.end));
    }
    public bool Equals(Segment other)
    {
        return (this.start == other.start && this.end == other.end) || (this.start == other.end && this.end == other.start);
    }
}
