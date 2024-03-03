using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    private bool _isEnabled = false;
    protected bool IsEnabled
    {
        get
        {
            return _isEnabled;
        }
        set
        {
            _isEnabled = value;
        }
    }

    public void Start()
    {
        Setup();
    }
    public void Update()
    {
        if (IsEnabled)
        {
            Run();
        }
    }
    public void Enable()
    {
        IsEnabled = true;
    }
    public void Disable()
    {
        IsEnabled = false;
        ClearState();
    }
    protected abstract void ClearState();
    protected abstract void Setup();
    protected abstract void Run();
}
