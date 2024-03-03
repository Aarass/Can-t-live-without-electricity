using Assets.Scripts.GeneralPurpose;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.Rendering;

[RequireComponent(typeof(Grid))]
public class GridManager : SingletonMonoBehaviour<GridManager>
{
    public static GridManager Instance;
    private void Awake()
    {
        Instance = this;
        grid = GetComponent<Grid>();
    }

    [SerializeField] float threshold = .5f;

    Plane plane;
    Grid grid;
    Vector3 hitPoint;

    [SerializeField, HideInInspector] Texture2D texture;
    bool[,] alpha;
    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
        grid = GetComponent<Grid>();

        alpha = new bool[Map.Width, Map.Height];
        for (int i = 0; i < Map.Width; i++)
            for (int j = 0; j < Map.Height; j++)
                alpha[i, j] = texture.GetPixel(i, j).a > .5f;
    }
    public void Setup(Texture2D texture)
    {
        transform.position = new Vector3(-((float)Map.Width * 3 / 2), 0, -((float)Map.Height * 3 / 2));
        grid = GetComponent<Grid>();
        this.texture = texture;
    }
    public Cell Intersect(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        Cell cell = Cell.Invalid;
        if (plane.Raycast(ray, out float dist))
        {
            hitPoint = ray.GetPoint(dist);
            cell = (Cell)grid.WorldToCell(hitPoint);
        }
        return cell;
    }
    bool a;
    public Cell IntersectNarrower(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (!plane.Raycast(ray, out float dist)) return Cell.Invalid;

        hitPoint = ray.GetPoint(dist);
        Cell cell = (Cell)grid.WorldToCell(hitPoint);


        Vector3 pointLocal= grid.WorldToLocal(hitPoint);
        Vector3 pointLocalTranslated = new Vector3(
            pointLocal.x / grid.cellSize.x - .5f,
            0,
            pointLocal.z / grid.cellSize.z - .5f
        );

        float len = (pointLocalTranslated - (Vector3)cell).magnitude;

        if (len < threshold &&
            cell.x >= 0 && cell.x < alpha.GetLength(0) &&
            cell.z >= 0 && cell.z < alpha.GetLength(1) &&
            alpha[cell.x, cell.z])
            return cell;
        else
            return Cell.Invalid;
    }
    public Vector3 CellToWorld(Cell point)
    {
        return grid.CellToWorld(new Vector3Int(point.x, 0, point.z)) + new Vector3(1.5f, 0, 1.5f);
    }
    public Cell WorldToCell(Vector3 point)
    {
        return (Cell)grid.WorldToCell(point);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(hitPoint, 1f);
    }
}
