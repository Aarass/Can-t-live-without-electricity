using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class Pipe : Segment
{
    GameObject geometry;
    public Pipe(Cell start, Cell end, Network network) : base(start, end, network) { }

    public override void OnSuccessfulCreation()
    {
        CreateGeometry();
        ApplyColor();
    }
    // TODO: Use prefab instead
    private void CreateGeometry()
    {
        Vector3 wrldStart = GridManager.Instance.CellToWorld(start);
        Vector3 wrldEnd = GridManager.Instance.CellToWorld(end);

        wrldStart.y = -1.5f;
        wrldEnd.y = -1.5f;

        System.Random random = new(start.GetHashCode() + end.GetHashCode());
        float offset = ((float)random.NextDouble() * 2 - 1f) * .5f;
        wrldStart.y += offset;
        wrldEnd.y += offset;

        GameObject obj = new("Pipe");
        obj.transform.position = wrldStart;

        Mesh mesh = new();

        Vector3 diff = wrldEnd - wrldStart;
        Vector3 dir = diff.normalized;
        int resolution = 8;
        float radius = .1f;


        Vector3[] ring = new Vector3[resolution + 1];
        float delta = 360 / resolution;
        for (int i = 0; i < ring.Length - 1; i++)
            ring[i] = Quaternion.AngleAxis(i * delta, dir) * Vector3.up * radius;
        ring[ring.Length - 1] = Quaternion.AngleAxis(0, dir) * Vector3.up * radius;


        List<Vector3> vertices = new();
        List<Vector2> uv = new();
        for (int i = 0; i < ring.Length; i++)
        {
            vertices.Add(ring[i]);
            vertices.Add(ring[i] + diff);

            float x = (float)i / (ring.Length - 1);
            uv.Add(new Vector2(x, 0.0f));
            uv.Add(new Vector2(x, 1.0f));
        }
        List<int> triangles = new();
        for (int i = 0; i < ring.Length - 1; i++)
        {
            triangles.Add(i * 2);
            triangles.Add(i * 2 + 2);
            triangles.Add(i * 2 + 1);

            triangles.Add(i * 2 + 3);
            triangles.Add(i * 2 + 1);
            triangles.Add(i * 2 + 2);
        }



        mesh.SetVertices(vertices, 0, vertices.Count);
        mesh.SetUVs(0, uv);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();


        obj.AddComponent<MeshFilter>().mesh = mesh;

        MeshRenderer pipeRenderer = obj.AddComponent<MeshRenderer>();
        pipeRenderer.material = AssetsServer.Instance.pipeMaterial;
        pipeRenderer.material.SetFloat("_Length", (wrldEnd - wrldStart).magnitude);
        pipeRenderer.material.SetFloat("_AnimT", Time.timeSinceLevelLoad);

        GameObject child = new("Rings");
        child.transform.parent = obj.transform;
        child.transform.localPosition = Vector3.zero;


        CombineInstance[] combine = new CombineInstance[2];
        combine[0].mesh = AssetsServer.Instance.ring;
        combine[1].mesh = AssetsServer.Instance.ring;

        combine[0].transform = Matrix4x4.Translate(new Vector3(0, -wrldStart.y, 0));
        combine[1].transform = Matrix4x4.Translate(new Vector3(diff.x, -wrldStart.y, diff.z));

        Mesh rings = new();
        rings.CombineMeshes(combine);

        child.AddComponent<MeshFilter>().mesh = rings;

        MeshRenderer ringsRenderer = child.AddComponent<MeshRenderer>();
        ringsRenderer.material = AssetsServer.Instance.pipeMaterial;
        ringsRenderer.material.SetFloat("_Length", 8f);
        ringsRenderer.material.SetFloat("_AnimT", Time.timeSinceLevelLoad);

        geometry = obj;
    }
    public override void ApplyColor()
    {
        if (geometry == null) return;
        geometry.GetComponent<MeshRenderer>().material.color = Assets.Helper.PowerColorToColor(path.color);
        geometry.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Assets.Helper.PowerColorToColor(path.color);
    }
    public override void Destroy()
    {
        path.RemoveSegment(this);
        network.RemoveSegment(this);

        if (geometry)
            UnityEngine.Object.Destroy(geometry);
    }

    public override bool IsIntersecting(Pipe other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
    }

    public override bool IsIntersecting(Wire other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
    }

    public override bool IsIntersecting(Invisible other)
    {
        return (start == other.start || start == other.end ||
                end == other.start || end == other.end);
    }
    public override bool RequestCorrectIntersectionCheck(Segment caller)
    {
        return caller.IsIntersecting(this);
    }
}