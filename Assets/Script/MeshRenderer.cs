using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRenderer : MonoBehaviour
{

    void Start()
    {
        SpriteRenderer imageSprite = GetComponent<SpriteRenderer>();

        // Get the Texture2D from the sprite
        Texture2D imageTexture = GetReadableTexture(imageSprite.sprite.texture);

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
                    // Normalize the vertex position based on image dimensions
                    float normalizedX = x / (float)width;
                    float normalizedY = y / (float)height;
                    vertices[vertIndex] = new Vector3(normalizedX, normalizedY, 0);
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

        // Check if Rigidbody already exists on the GameObject
        Rigidbody existingRigidbody = gameObject.GetComponent<Rigidbody>();
        if (existingRigidbody == null)
        {
            // Add Rigidbody only if it doesn't exist
            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        }


        // Attach Mesh Collider to the GameObject
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true; // Convex should be true for dynamic objects
    }

    // Function to get a readable copy of the texture
    Texture2D GetReadableTexture(Texture2D originalTexture)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(originalTexture.width, originalTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(originalTexture, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D readableTexture = new Texture2D(originalTexture.width, originalTexture.height);
        readableTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return readableTexture;
    }
}
