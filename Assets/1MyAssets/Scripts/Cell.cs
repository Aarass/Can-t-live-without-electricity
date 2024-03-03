using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public struct Cell : IEquatable<Cell>
{
    public int x;
    public int z;
    public Cell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public Cell(Vector3Int v)
    {
        this.x = v.x;
        this.z = v.z;
    }
    public Vector3 ToVector()
    {
        return new Vector3(x, 0, z);
    }
    public static Cell Invalid
    {
        get
        {
           return new Cell(int.MinValue, int.MinValue);
        }
        private set { }
    }
    public static Cell Zero
    {
        get
        {
            return new Cell(0, 0);
        }
        private set { }
    }
    public static Cell Abs(Cell c)
    {
        return new Cell(Mathf.Abs(c.x), Mathf.Abs(c.z));
    }
    public bool IsValid()
    {
        return x != int.MinValue && z != int.MinValue;
    }
    public static bool operator==(Cell a, Cell b)
    {
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator!=(Cell a, Cell b)
    {
        return a.x != b.x || a.z != b.z;
    }
    public static explicit operator Cell(Vector3Int v)
    {
        return new Cell(v);
    }
    public static explicit operator Vector3(Cell c)
    {
        return new Vector3(c.x, 0, c.z);
    }

    public static implicit operator bool(Cell c)
    {
        return c.IsValid();
    }
    public static implicit operator string(Cell c)
    {
        return "x: " + c.x + " z: " + c.z;
    }
    public static Cell operator- (Cell a, Cell b)
    {
        return new Cell(a.x - b.x, a.z - b.z);
    }
    public static Cell operator+ (Cell a, Cell b)
    {
        return new Cell(a.x + b.x, a.z + b.z);
    }
    public override bool Equals(object obj)
    {
        return obj is Cell cell &&
               x == cell.x &&
               z == cell.z;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public bool Equals(Cell other)
    {
        return x == other.x && z == other.z;
    }
}
