using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    public float radius = 1f;
    public float height = 2f;
    public int segments = 12;

    public Vector3 cylinderCenter;
    public Material cylinderMaterial;

    public float focalLength;

    public Vector3 rotationAngles; // Rotation support

    private void OnPostRender()
    {
        DrawCylinder();
    }

    private void OnDrawGizmos()
    {
        DrawCylinder();
    }

    private void DrawCylinder()
    {
        if (cylinderMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        cylinderMaterial.SetPass(0);

        Quaternion rotation = Quaternion.Euler(rotationAngles); // Apply rotation

        Vector3[] topCircle = new Vector3[segments];
        Vector3[] bottomCircle = new Vector3[segments];

        // Create circle points
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            topCircle[i] = rotation * new Vector3(x, height / 2f, z) + cylinderCenter;
            bottomCircle[i] = rotation * new Vector3(x, -height / 2f, z) + cylinderCenter;
        }

        // Apply focal point scaling
        float topScale = focalLength / ((cylinderCenter.z + height * 0.5f) + focalLength);
        float bottomScale = focalLength / ((cylinderCenter.z - height * 0.5f) + focalLength);

        // Draw circles
        for (int i = 0; i < segments; i++)
        {
            Vector3 top1 = topCircle[i] * topScale;
            Vector3 top2 = topCircle[(i + 1) % segments] * topScale;

            Vector3 bottom1 = bottomCircle[i] * bottomScale;
            Vector3 bottom2 = bottomCircle[(i + 1) % segments] * bottomScale;

            // Top Circle
            GL.Color(Color.red);
            GL.Vertex(top1);
            GL.Vertex(top2);

            // Bottom Circle
            GL.Color(Color.red);
            GL.Vertex(bottom1);
            GL.Vertex(bottom2);

            // Vertical Lines
            GL.Color(Color.red);
            GL.Vertex(top1);
            GL.Vertex(bottom1);
        }

        GL.End();
        GL.PopMatrix();
    }
}
