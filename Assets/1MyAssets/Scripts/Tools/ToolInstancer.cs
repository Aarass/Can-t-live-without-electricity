using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ToolName
{
    Wire,
    Jumper,
    Generator,
    Pipe,
    Eraser
}

public class ToolInstancer : MonoBehaviour
{
    [SerializeField] ToolName toolName;
    void Start()
    {
        this.AddComponent<Button>().onClick.AddListener(Click);
    }
    void Click()
    {
        switch(toolName)
        {
            case ToolName.Wire:
                ToolManager.Instance.ChangeTool<Tool_WireBuilder>(gameObject);

                break;
            case ToolName.Jumper:

                break;
            case ToolName.Generator:

                break;
            case ToolName.Pipe:
                ToolManager.Instance.ChangeTool<Tool_PipeBuilder>(gameObject);

                break;
            case ToolName.Eraser:

                break;
        }
    }
}
