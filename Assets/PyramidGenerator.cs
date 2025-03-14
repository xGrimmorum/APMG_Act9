using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public float pyramidBaseSize = 2f;
    public float pyramidHeight = 2f;
    public Vector3 pyramidCenter;
    public Material pyramidMaterial;
    public float focalLength = 10f;
    public Vector3 rotationAngles;

    private void OnPostRender()
    {
        DrawPyramid();
    }

    private void OnDrawGizmos()
    {
        DrawPyramid();
    }

    private void DrawPyramid()
    {
        if (pyramidMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        pyramidMaterial.SetPass(0);

        // Rotation matrix
        Quaternion rotation = Quaternion.Euler(rotationAngles);

        // Square base
        Vector3[] baseCorners = new Vector3[]
        {
            rotation * (pyramidCenter + new Vector3(-pyramidBaseSize / 2f, 0, -pyramidBaseSize / 2f)),
            rotation * (pyramidCenter + new Vector3(pyramidBaseSize / 2f, 0, -pyramidBaseSize / 2f)),
            rotation * (pyramidCenter + new Vector3(pyramidBaseSize / 2f, 0, pyramidBaseSize / 2f)),
            rotation * (pyramidCenter + new Vector3(-pyramidBaseSize / 2f, 0, pyramidBaseSize / 2f))
        };

        Vector3 apex = rotation * (pyramidCenter + new Vector3(0, pyramidHeight, 0));

        float baseScale = focalLength / ((pyramidCenter.z) + focalLength);
        float apexScale = focalLength / ((pyramidCenter.z + pyramidHeight) + focalLength);

        // Draw base
        for (int i = 0; i < baseCorners.Length; i++)
        {
            Vector3 p1 = baseCorners[i] * baseScale;
            Vector3 p2 = baseCorners[(i + 1) % baseCorners.Length] * baseScale;

            GL.Color(Color.yellow);
            GL.Vertex3(p1.x, p1.y, 0);
            GL.Vertex3(p2.x, p2.y, 0);
        }

        // Draw triangular sides
        foreach (var corner in baseCorners)
        {
            Vector3 p1 = corner * baseScale;
            Vector3 p2 = apex * apexScale;

            GL.Color(Color.yellow);
            GL.Vertex3(p1.x, p1.y, 0);
            GL.Vertex3(p2.x, p2.y, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
}
