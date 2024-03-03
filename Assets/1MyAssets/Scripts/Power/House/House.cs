using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class House : MonoBehaviour, Colored, NetworkObserver
{
    public PowerColor color { get; set; }
    public Cell cell;
    private GameObject sign;

    void Start()
    {
        cell = GridManager.GetInstance().WorldToCell(transform.position);
        color = PowerPlantsManager.GetInstance().RandomColor();

        Network.Instance.Subscribe(this);
        HousesManager.GetInstance().RegisterNewHouse(this);

        IsGeometryEnabled = false;
    }
    private bool _isGeometryEnabled;
    private bool IsGeometryEnabled
    {
        get { return _isGeometryEnabled; }
        set
        {
            if (value != _isGeometryEnabled)
            {
                _isGeometryEnabled = value;
                ApplyStyle();
            }
        }
    }
    private bool _isPowered;
    public bool IsPowered
    {
        get { return _isPowered; }
        private set
        {
            if(value != _isPowered)
            {
                _isPowered = value;
                if(_isGeometryEnabled)
                    ApplyStyle();
            }
        }
    }
    public void EnableGeometry()
    {
        IsGeometryEnabled = true;
    }
    private void ApplyStyle()
    {
        if (IsPowered)
        {
            if (!sign.IsDestroyed())
                UnityEngine.Object.Destroy(sign);
        }
        else
        {
            if (sign == null || sign.IsDestroyed())
            {
                Vector3 position = GridManager.Instance.CellToWorld(cell) + Vector3.up;
                sign = UnityEngine.Object.Instantiate(AssetsServer.Instance.signPrefab, position, Quaternion.identity);
            }
        }
    }
    public void ObserveNewSegment(Segment segment)
    {
        List<Segment> connected = Network.Instance[cell];

        if (
            connected != null &&
            connected.Count != 0 &&
            connected.First().path != null &&
            connected.First().path.color == color
           )
            IsPowered = true;
    }
    public void ObserveRemovedSegment(Segment segment)
    {
        List<Segment> connected = Network.Instance[cell];

        if (
            connected == null || 
            connected.Count == 0 || 
            connected.First().path == null || 
            connected.First().path.color != color
           )
            IsPowered = false;
    }
    public bool IsCompatibleTo(Colored other)
    {
        return color == PowerColor.Gray || other.color == PowerColor.Gray || color == other.color;
    }
}