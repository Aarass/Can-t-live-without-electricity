using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerColor
{
    Gray,
    Red,
    Yellow,
    Blue,
    Purple,
    Green
}
public interface Colored
{
    public PowerColor color { get; set; }
    public bool IsCompatibleTo(Colored other);
}
