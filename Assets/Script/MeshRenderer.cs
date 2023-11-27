using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderer : MonoBehaviour
{

    public Texture2D imageTexture;

    void Start()
    {
        // Get the alpha values from the image texture
        Color32[] pixels = imageTexture.GetPixels32();
        int width = imageTexture.width;
        int height = imageTexture.height;

        // Create a list to store mesh vertices
        Vector3[] vertices = new Vector3[width * height];
        int vertIndex = 0;

        // Iterate through the image pixels to create vertices
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (pixels[y * width + x].a > 0) // Check if pixel is not transparent
                {
                    vertices[vertIndex] = new Vector3(x, y, 0);
                    vertIndex++;
                }
            }
        }

        // Create a mesh and set its vertices
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;

        // Create triangles to form a convex mesh
        int[] triangles = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            triangles[i] = i;
        }
        mesh.triangles = triangles;

        // Attach Mesh Collider to the GameObject
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true; // Convex should be true for dynamic objects
    }
}
