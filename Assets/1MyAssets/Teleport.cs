using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour, Colored
{
    [SerializeField]
    List<GameObject> ports;
    List<Segment> segments;

    public PowerColor color { get; set; }

    public void Start()
    {
        segments = new();
        for (int i = 0; i < ports.Count - 1; i++)
        {
            Cell start = GridManager.Instance.WorldToCell(ports[i].transform.position);
            Cell end = GridManager.Instance.WorldToCell(ports[i + 1].transform.position);
            Segment segment = new Invisible(start, end, Network.Instance, this);
            segments.Add(segment);
            Network.Instance.AddSegment(segment);
        }
    }
    public void ApplyColor()
    {
        PowerColor color = segments.First().path.color;
        if (this.color == color) return;

        this.color = color;
        for (int i = 0; i < ports.Count; i++)
        {
            ports[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Helper.PowerColorToColor(color));
            //((Network.Instance[cells[i]]?.Count ?? 0) > 0 ? Network.Instance[cells[i]] : null)?.First()?.path?.CheckForPower();
        }
    }

    public bool IsCompatibleTo(Colored other)
    {
        return color == PowerColor.Gray || other.color == PowerColor.Gray || color == other.color;
    }
}