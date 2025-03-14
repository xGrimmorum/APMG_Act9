using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public float cubeSideLength;
    public Vector3 cubeCenter;
    public Material cubeMaterial;
    public float focalLength;

    public Vector3[] GetFrontSquare()
    {
        float halfLength = cubeSideLength * 0.5f;

        return new[]
        {
            new Vector3(cubeCenter.x + halfLength, cubeCenter.y + halfLength, -halfLength),
            new Vector3(cubeCenter.x - halfLength, cubeCenter.y + halfLength, -halfLength),
            new Vector3(cubeCenter.x - halfLength, cubeCenter.y - halfLength, -halfLength),
            new Vector3(cubeCenter.x + halfLength, cubeCenter.y - halfLength, -halfLength)
        };
    }

    public Vector3[] GetBackSquare()
    {
        float halfLength = cubeSideLength * 0.5f;

        return new[]
        {
            new Vector3(cubeCenter.x + halfLength, cubeCenter.y + halfLength, halfLength),
            new Vector3(cubeCenter.x - halfLength, cubeCenter.y + halfLength, halfLength),
            new Vector3(cubeCenter.x - halfLength, cubeCenter.y - halfLength, halfLength),
            new Vector3(cubeCenter.x + halfLength, cubeCenter.y - halfLength, halfLength)
        };
    }

    private void OnPostRender() => DrawLines();
    private void OnDrawGizmos() => DrawLines();

    public void DrawLines()
    {
        if (cubeMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        cubeMaterial.SetPass(0);
        GL.Color(Color.green);

        Vector3[] frontSquare = GetFrontSquare();
        Vector3[] backSquare = GetBackSquare();

        // Front Square (With Focal Length Scaling)
        float frontScale = focalLength / ((cubeCenter.z - cubeSideLength * 0.5f) + focalLength);
        DrawSquare(frontSquare, frontScale);

        // Back Square (With Focal Length Scaling)
        float backScale = focalLength / ((cubeCenter.z + cubeSideLength * 0.5f) + focalLength);
        DrawSquare(backSquare, backScale);

        // Connecting Lines (Front to Back)
        for (int i = 0; i < frontSquare.Length; i++)
        {
            GL.Color(Color.green);
            Vector3 point1 = frontSquare[i] * frontScale;
            Vector3 point2 = backSquare[i] * backScale;
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }

    private void DrawSquare(Vector3[] square, float scale)
    {
        for (int i = 0; i < square.Length; i++)
        {
            Vector3 point1 = square[i] * scale;
            Vector3 point2 = square[(i + 1) % square.Length] * scale;
            GL.Vertex3(point1.x, point1.y, 0);
            GL.Vertex3(point2.x, point2.y, 0);
        }
    }
}
