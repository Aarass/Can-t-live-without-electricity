using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerSource: MonoBehaviour, Colored
{
    public static SparseMatrix<PowerSource> Collection = new(Map.Width, Map.Height);
    [SerializeField] protected PowerColor clr;
    public virtual PowerColor color { get { return clr; } set { clr = value; } }
    public bool IsCompatibleTo(Colored other)
    {
        return color == PowerColor.Gray || other.color == PowerColor.Gray || color == other.color;
    }
}
