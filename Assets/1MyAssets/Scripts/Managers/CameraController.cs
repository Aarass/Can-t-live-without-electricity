using Assets.Scripts.GeneralPurpose;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

enum Mode
{
    Idle,
    Moving,
    Rotating,
    Boundary,
    Building,
    Erasing
}

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField] GameObject cameraDriver;
    [SerializeField] GameObject gridMeshParent;
    [SerializeField] float minDist;
    [SerializeField] float maxDist;
    [SerializeField] float minRotation;
    [SerializeField] float maxRotation;
    [SerializeField] float rotatingSpeed;
    [SerializeField] float movingSpeed;
    [SerializeField] float zoomingSpeed;

    MeshCollider gridMesh;

    float rotatedAmountX;
    float rotatedAmountY;
    Mode previousMode;
    Mode currentMode;
    Mode nextMode;
    Vector3 previousMousePosition;
    Vector3 diff;
    bool clickedOverMesh;
    void Start()
    {
        rotatedAmountX = 0f;
        rotatedAmountY = 45f;
        Setup();
    }
    public void Setup()
    {
        gridMesh = gridMeshParent.transform.GetChild(0).GetComponent<MeshCollider>();
        previousMode = Mode.Idle;
        currentMode = Mode.Idle;
        diff = Vector3.zero;
        clickedOverMesh = false;
    }
    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            clickedOverMesh = gridMesh.Raycast(ray, out _, 500f);
            diff = Input.mousePosition - previousMousePosition;
        }
        ChangeState();

        if (LeftButton() || RightButton() || MiddleButton() || Scroll()) { }

        if (previousMode < Mode.Boundary && currentMode > Mode.Boundary)
            ToolManager.Instance.EnableTool();
        else if (previousMode > Mode.Boundary && currentMode < Mode.Boundary)
            ToolManager.Instance.DisableTool();

        previousMode = currentMode;
        previousMousePosition = Input.mousePosition;
    }
    void ChangeState()
    {
        currentMode = nextMode;
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            nextMode = Mode.Idle;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (currentMode == Mode.Idle)
            {
                if (clickedOverMesh)
                    nextMode = Mode.Building;
                else
                    nextMode = Mode.Moving;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (currentMode == Mode.Idle)
            {
                if (clickedOverMesh)
                    nextMode = Mode.Erasing;
                else
                    nextMode = Mode.Rotating;
            }
        }
    }
    bool LeftButton()
    {
        if (!Input.GetMouseButton(0)) return false;

        if (previousMode == Mode.Moving && currentMode == Mode.Moving)
        {
            Vector3 tmp = cameraDriver.transform.right * diff.x + Vector3.Cross(cameraDriver.transform.right, Vector3.up) * diff.y;
            transform.position = transform.position - movingSpeed * Time.deltaTime * tmp;
        }
        return true;
    }
    bool RightButton()
    {
        if (previousMode == Mode.Rotating && currentMode == Mode.Rotating)
        {
            rotatedAmountX -= diff.y * rotatingSpeed * Time.deltaTime;
            rotatedAmountX = Mathf.Clamp(rotatedAmountX, minRotation, maxRotation);
            rotatedAmountY += diff.x * rotatingSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rotatedAmountX, rotatedAmountY, 0f);
            return true;
        }
        if(previousMode != Mode.Erasing && currentMode == Mode.Erasing)
        {
            ToolManager.Instance.TemporaryChangeTool<Tool_Eraser>();
            return true;
        }
        if (previousMode == Mode.Erasing && currentMode != Mode.Erasing)
        {
            ToolManager.Instance.RecoverPreviousTool();
            return true;
        }
        return false;
    }
    bool MiddleButton()
    {
        if (!Input.GetMouseButtonDown(2)) return false;

        transform.position = Vector3.zero;
        return true;
    }
    bool Scroll()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            float newOrthographicSize = Camera.main.orthographicSize - Input.mouseScrollDelta.y * zoomingSpeed;

            if (newOrthographicSize > minDist && newOrthographicSize < maxDist)
                Camera.main.orthographicSize = newOrthographicSize;
            return true;
        }
        return false;
    }
}
