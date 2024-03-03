using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Path : Colored, IEnumerable<Segment>
{
    [SerializeField] PowerColor clr;
    public PowerColor color
    {
        get { return clr; }
        set
        {
            clr = value;
            ApplyColor();
        }
    }
    private readonly List<Segment> segments;
    public readonly HashSet<PowerSource> sources;
    public Path()
    {
        segments = new();
        sources = new();
    }
    public void AddSegment(Segment segment)
    {
        segments.Add(segment);
        segment.path = this;
    }
    public void RemoveSegment(Segment segment)
    {
        //Segment original = segments.Find(s => s.Equals(segment));
        //segments.Remove(original);
        //return original;
        segments.Remove(segment);
    }
    public void Merge(Path other)
    {
        foreach (Segment segment in other.segments)
            AddSegment(segment);
        foreach (PowerSource ps in other.sources)
            AddSource(ps);
        CheckForPower();
    }
    public void CheckForPower()
    {
        foreach (PowerSource s in sources)
        {
            if (s.color != PowerColor.Gray)
            {
                color = s.color;
                return;
            }
        }
        color = PowerColor.Gray;
    }
    private void ApplyColor()
    {
        foreach (Segment segment in segments)
            segment.ApplyColor();
    }
    public void AddSource(PowerSource ps)
    {
        sources.Add(ps);
    }
    public bool IsCompatibleTo(Colored other)
    {
        return color == PowerColor.Gray || other.color == PowerColor.Gray || color == other.color;
    }
    public IEnumerator<Segment> GetEnumerator()
    {
        return segments.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
