using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class SaveShaderTexture : MonoBehaviour
{
    public int TextureLength = 1024;

    private Texture2D texture;
    public void Save()
    {
        RenderTexture buffer = new RenderTexture(
                               TextureLength,
                               TextureLength,
                               0,                            // No depth/stencil buffer
                               RenderTextureFormat.ARGB32,   // Standard colour format
                               RenderTextureReadWrite.sRGB // No sRGB conversions
                               );
 //RenderTexture buffer = new RenderTexture(
 //                              TextureLength,
 //                              TextureLength,
 //                              0,                            // No depth/stencil buffer
 //                              RenderTextureFormat.ARGB32,   // Standard colour format
 //                              RenderTextureReadWrite.sRGB // No sRGB conversions
 //                          );

        texture = new Texture2D(TextureLength, TextureLength, TextureFormat.ARGB32, false);

        //MeshRenderer render = GetComponent<MeshRenderer>();
        ////texture = render.sharedMaterial.GetTexture("_MainTex") as Texture2D;
        //Material material = render.sharedMaterial;

        //Graphics.Blit(null, buffer, material);
        //RenderTexture.active = buffer;           // If not using a scene camera

        //texture.ReadPixels(
        //  new Rect(0, 0, TextureLength, TextureLength), // Capture the whole texture
        //  0, 0,                          // Write starting at the top-left texel
        //  false);                          // No mipmaps

        //System.IO.File.WriteAllBytes(Application.dataPath + "/" + "SkinLut.png", texture.EncodeToPNG());
        //// texture.Save();
        ///
        Texture2D sourceTexture = new Texture2D(TextureLength, TextureLength, TextureFormat.ARGB32, false);
        Material material = gameObject.GetComponent<Renderer>().material;
        Graphics.Blit(sourceTexture, buffer, material, -1);

        RenderTexture.active = buffer;           // If not using a scene camera
        texture.ReadPixels(
                  new Rect(0, 0, TextureLength, TextureLength), // Capture the whole texture
                  0, 0,                          // Write starting at the top-left texel
                  false                          // No mipmaps
        );

        string path = "D:\\Repos\\Barebones\\Assets\\Prototyping\\SaveTexture" + "\\ myTexture2.png";

        StreamWriter writer = new StreamWriter(path, true);
        byte[] bytes = texture.EncodeToPNG();
        //writer.Write(bytes);
        //writer.Close();
        System.IO.File.WriteAllBytes(path, bytes);
        //File.WriteAllBytes(path, bytes);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Save();
    }

    // Update is called once per frame
    void Update()
    {
        // Save();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
    }
}