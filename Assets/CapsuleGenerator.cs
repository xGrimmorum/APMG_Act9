using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGenerator : MonoBehaviour
{
    public float radius = 1f;
    public float height = 2f;
    public int segments = 12;
    public Vector3 capsulePosition = Vector3.zero; // Customizable position
    public Vector3 rotation = Vector3.zero;        // Customizable rotation
    public Material capsuleMaterial;

    private void OnPostRender()
    {
        DrawCapsule();
    }

    private void OnDrawGizmos()
    {
        DrawCapsule();
    }

    private void DrawCapsule()
    {
        if (capsuleMaterial == null) return;

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        capsuleMaterial.SetPass(0);

        Quaternion rotationQuat = Quaternion.Euler(rotation);

        // Cylinder body
        Vector3[] topCircle = new Vector3[segments];
        Vector3[] bottomCircle = new Vector3[segments];

        float halfHeight = (height / 2f) - radius; // Ensures the hemispheres connect correctly

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            topCircle[i] = rotationQuat * (new Vector3(x, halfHeight, z)) + capsulePosition;
            bottomCircle[i] = rotationQuat * (new Vector3(x, -halfHeight, z)) + capsulePosition;

            // Vertical lines
            GL.Color(capsuleMaterial.color);
            GL.Vertex(topCircle[i]);
            GL.Vertex(bottomCircle[i]);
        }

        // Draw circles
        for (int i = 0; i < segments; i++)
        {
            GL.Color(capsuleMaterial.color);

            GL.Vertex(topCircle[i]);
            GL.Vertex(topCircle[(i + 1) % segments]);

            GL.Vertex(bottomCircle[i]);
            GL.Vertex(bottomCircle[(i + 1) % segments]);
        }

        // Draw hemispheres
        DrawHemisphere(topCircle[0], rotationQuat, capsulePosition + new Vector3(0, halfHeight, 0), true);  // Top
        DrawHemisphere(bottomCircle[0], rotationQuat, capsulePosition + new Vector3(0, -halfHeight, 0), false); // Bottom (now inverted)

        GL.End();
        GL.PopMatrix();
    }

    private void DrawHemisphere(Vector3 center, Quaternion rotationQuat, Vector3 positionOffset, bool isTop)
    {
        int hemisphereSegments = segments / 2;

        for (int lat = 0; lat <= hemisphereSegments; lat++)
        {
            float latAngle1 = Mathf.PI * lat / (2 * hemisphereSegments);
            float latAngle2 = Mathf.PI * (lat + 1) / (2 * hemisphereSegments);

            for (int lon = 0; lon < segments; lon++)
            {
                float lonAngle1 = 2f * Mathf.PI * lon / segments;
                float lonAngle2 = 2f * Mathf.PI * (lon + 1) / segments;

                Vector3 p1 = rotationQuat * GetHemispherePoint(latAngle1, lonAngle1, isTop) + positionOffset;
                Vector3 p2 = rotationQuat * GetHemispherePoint(latAngle1, lonAngle2, isTop) + positionOffset;
                Vector3 p3 = rotationQuat * GetHemispherePoint(latAngle2, lonAngle1, isTop) + positionOffset;

                GL.Color(capsuleMaterial.color);

                GL.Vertex(p1);
                GL.Vertex(p2);

                GL.Vertex(p1);
                GL.Vertex(p3);
            }
        }
    }

    private Vector3 GetHemispherePoint(float latAngle, float lonAngle, bool isTop)
    {
        float verticalOffset = isTop ? radius : -radius;  // Invert bottom hemisphere
        float direction = isTop ? 1f : -1f;                // Flip the bottom hemisphere's curve

        return new Vector3(
            Mathf.Sin(latAngle) * Mathf.Cos(lonAngle) * radius,
            direction * Mathf.Cos(latAngle) * radius,
            Mathf.Sin(latAngle) * Mathf.Sin(lonAngle) * radius
        );
    }
}
