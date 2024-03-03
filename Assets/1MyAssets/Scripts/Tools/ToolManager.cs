using Assets.Scripts.GeneralPurpose;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance;
    private void Awake() => Instance = this;

    [SerializeField] 
    GameObject borderGameObject;
    Material borderMaterial;

    private Tool currentTool;
    void Start()
    {
        currentTool = null;

        borderMaterial = borderGameObject.GetComponent<Image>().material;
        borderMaterial.SetFloat("_AnimT", -1.0f);
        borderMaterial.SetFloat("_AnimDir", -1.0f);
    }
    void RemoveOldTool()
    {
        if (currentTool == null)
            return;

        Destroy(currentTool);
        currentTool = null;
    }
    public void ChangeTool<T>(GameObject caller) where T : Tool
    {
        if (currentTool != null && borderGameObject.transform.parent == caller.transform)
        {
            borderMaterial.SetFloat("_AnimDir", -1.0f);
            RemoveOldTool();
        }
        else
        {
            borderGameObject.transform.SetParent(caller.transform, false);
            borderMaterial.SetFloat("_AnimDir", 1.0f);

            RemoveOldTool();
            currentTool = gameObject.AddComponent<T>();
        }
        borderMaterial.SetFloat("_AnimT", Time.timeSinceLevelLoad);
    }
    private Tool previousTool;
    public void TemporaryChangeTool<T>() where T : Tool
    {
        if (currentTool != null)
        {
            previousTool = currentTool;
            previousTool.Disable();
        }

        currentTool = gameObject.AddComponent<T>();
        currentTool.Enable();
    }
    public void RecoverPreviousTool()
    {
        if (previousTool == null) return;

        Destroy(currentTool);

        currentTool = previousTool;
        currentTool.Enable();
    }
    public void EnableTool()
    {
        currentTool?.Enable();
    }
    public void DisableTool()
    {
        currentTool?.Disable();
    }

}
