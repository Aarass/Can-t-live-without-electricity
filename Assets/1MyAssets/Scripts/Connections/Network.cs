using Assets.Scripts.GeneralPurpose;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;
using static UnityEngine.UI.Image;

public class Network : MonoBehaviour
{
    #region Singleton
    public static Network Instance;
    void Awake() => Instance = this;
    #endregion
    #region Publisher
    private readonly List<NetworkObserver> observers = new();
    public void Subscribe(NetworkObserver observer) => observers.Add(observer);
    public void PublishNewSegment(Segment segment)
    {
        observers.ForEach(o => o.ObserveNewSegment(segment));
        Rock.Collection[segment.start]?.ObserveNewSegment(segment);
        Rock.Collection[segment.end]?.ObserveNewSegment(segment);
    }
    public void PublishRemovedSegment(Segment segment)
    {
        observers.ForEach(o => o.ObserveRemovedSegment(segment));
        if (this[segment.start].Count == 0)
            Rock.Collection[segment.start]?.ObserveRemovedSegment(segment);
        if (this[segment.end].Count == 0)
            Rock.Collection[segment.end]?.ObserveRemovedSegment(segment);
    }
    #endregion
    readonly SparseMatrix<List<Segment>> dic;
    private readonly HashSet<Path> paths;
    public Network()
    {
        dic = new(Map.Width, Map.Height);

        paths = new();
    }
    public List<Segment> this[Cell cell]
    {
        get { return dic[cell]; }
        private set { }
    }
    public Segment AddSegment(Segment segment)
    {
        if (AlreadyExists(segment)) return null;

        if (!ResolveIntersections(segment)) return null;
        if (!ConnectPowerSources(segment)) return null;

        segment.OnSuccessfulCreation();

        AddToCollection(segment);
        segment.path.CheckForPower();
        PublishNewSegment(segment);
        segment.path.CheckForPower();
        return segment;
    }
    public Segment AddExistingSegment(Segment segment)
    {
        if (!ResolveIntersections(segment)) return null;
        if (!ConnectPowerSources(segment)) return null;

        segment.path.CheckForPower();
        return segment;
    }
    public void RemoveSegment(Segment segment)
    {
        dic[segment.start].Remove(segment);
        dic[segment.end].Remove(segment);
    }
    public void RemoveSegments(Cell cell)
    {
        if (!cell.IsValid()) return;

        List<Segment> segments = this[cell]?.ToList();

        if (segments == null || segments.Count == 0) return;

        Path path = segments.First().path;
        paths.Remove(path);

        foreach (Segment segment in segments)
        {
            segment.Destroy();
            PublishRemovedSegment(segment);
        }

        foreach (Segment segment in path)
            AddExistingSegment(segment);
    }
    public void AddToCollection(Segment segment)
    {
        (dic[segment.start] = dic[segment.start] ?? new()).Add(segment);
        (dic[segment.end] = dic[segment.end] ?? new()).Add(segment);
    }
    public bool ResolveIntersections(Segment segment)
    {
        List<Segment> intSegmentsStart = dic[segment.start];
        List<Segment> intSegmentsEnd = dic[segment.end];

        HashSet<Path> intPaths = new(2);
        if (intSegmentsStart != null)
            foreach (Segment other in intSegmentsStart)
                intPaths.Add(other.path);
        if (intSegmentsEnd != null)
            foreach (Segment other in intSegmentsEnd)
                intPaths.Add(other.path);


        Path[] intersections = intPaths.Intersect(paths).ToArray();

        Path path;
        if (intersections.Length == 0)
        {
            path = new();
            paths.Add(path);
        }
        else
        {
            path = intersections[0];
            if(intersections.Length == 2)
            {
                if (intersections[0].IsCompatibleTo(intersections[1]))
                {
                    intersections[0].Merge(intersections[1]);
                    paths.Remove(intersections[1]);
                }
                else
                {
                    ErrorShower.GetInstance().ShowText("Can't connect different colors");
                    return false;
                }
            }
        }
        path.AddSegment(segment);
        return true;
    }
    public bool ConnectPowerSources(Segment segment)
    {
        PowerSource ps1 = PowerSource.Collection[segment.start];
        PowerSource ps2 = PowerSource.Collection[segment.end];

        if(ps1 == null && ps2 != null)
        {
            ps1 = ps2;
            ps2 = null;
        }

        if (ps1 != null)
        {
            if (!ps1.IsCompatibleTo(segment.path))
            {
                ErrorShower.GetInstance().ShowText("Can't connect different colors");
                return false;
            }
            if (ps2 != null)
            {
                if (ps1 == ps2) return false;
                if (!ps1.IsCompatibleTo(ps2))
                {
                    ErrorShower.GetInstance().ShowText("Can't connect different colors");
                    return false;
                }
                segment.path.AddSource(ps2);
            }
            segment.path.AddSource(ps1);
        }
        return true;
    }
    public void ObserveNewPowerPlant(PowerPlant ps)
    {
        Cell initialCell = GridManager.Instance.WorldToCell(ps.transform.position);
        Cell[] cells = new[]
        {
            initialCell,
            initialCell + new Cell(-1, 0),
            initialCell + new Cell(-1, -1),
            initialCell + new Cell(0, -1)
        };

        foreach(Cell cell in cells)
        {
            List<Segment> segments = dic[cell];
            if (segments == null) continue;

            segments.First().path.AddSource(ps);
            segments.First().path.CheckForPower();
        }
    }
    public bool AlreadyExists(Segment segment)
    {
        if (dic[segment.start]?.Any(s => s.Equals(segment)) ?? false)
        {
            ErrorShower.GetInstance().ShowText("Segment already exists");
            return true;
        }
        return false;
    }
}
