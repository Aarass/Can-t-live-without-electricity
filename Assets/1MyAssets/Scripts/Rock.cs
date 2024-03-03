using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class Rock : MonoBehaviour, NewSegmentObserver, RemovedSegmentObserver
{
    public static SparseMatrix<Rock> Collection = new (Map.Width, Map.Height);

    Cell cell;
    bool isOccupied;
    public bool IsOccupied
    {
        get
        {
            return isOccupied;
        }
        set
        {
            if(value != isOccupied)
            {
                isOccupied = value;
                ApplyStyle();
            }
        }
    }

    public void Start()
    {
        cell = GridManager.Instance.WorldToCell(this.transform.position);
        Collection[cell] = this;
        ApplyStyle();
    }

    private void ApplyStyle()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        if (IsOccupied)
            Instantiate(AssetsServer.Instance.hole, transform.position, Quaternion.identity, transform);
        else
            Instantiate(AssetsServer.Instance.rocks, transform.position, Quaternion.identity, transform);
    }
    public void ObserveNewSegment(Segment segment)
    {
        IsOccupied = true;
    }
    public void ObserveRemovedSegment(Segment segment)
    {
        IsOccupied = false;
    }
}