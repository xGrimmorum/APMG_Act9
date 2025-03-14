using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public float radius = 1f;
    public int segments = 12;
    public Vector3 spherePosition = Vector3.zero; // Customizable position
    public Material sphereMaterial;

    public float focalLength = 10f;

    private void OnPostRender()
    {
        DrawSphere();
    }

    private void OnDrawGizmos()
    {
        DrawSphere();
    }

    private void DrawSphere()
    {
        
        if (sphereMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        sphereMaterial.SetPass(0);

        for (int lat = 0; lat < segments; lat++)
        {
            float latAngle1 = Mathf.PI * lat / segments;
            float latAngle2 = Mathf.PI * (lat + 1) / segments;

            for (int lon = 0; lon < segments; lon++)
            {
                float lonAngle1 = 2f * Mathf.PI * lon / segments;
                float lonAngle2 = 2f * Mathf.PI * (lon + 1) / segments;

                Vector3 p1 = GetPoint(latAngle1, lonAngle1) + spherePosition;
                Vector3 p2 = GetPoint(latAngle1, lonAngle2) + spherePosition;
                Vector3 p3 = GetPoint(latAngle2, lonAngle1) + spherePosition;
                Vector3 p4 = GetPoint(latAngle2, lonAngle2) + spherePosition;
                GL.Color(Color.cyan);

                // Horizontal lines
                GL.Vertex(p1);
                GL.Vertex(p2);

                // Vertical lines
                GL.Vertex(p1);
                GL.Vertex(p3);
            }
        }

        GL.End();
        GL.PopMatrix();
    }

    private Vector3 GetPoint(float latAngle, float lonAngle)
    {
        Vector3 point = new Vector3(
            Mathf.Sin(latAngle) * Mathf.Cos(lonAngle),
            Mathf.Cos(latAngle),
            Mathf.Sin(latAngle) * Mathf.Sin(lonAngle)
        ) * radius;

        // Apply focal length effect
        float scale = focalLength / (point.z + focalLength);
        return point * scale;
    }
}
