using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCache : MonoBehaviour
{
    public static MaterialCache instance;

    public Dictionary<(Color, float, FaceType, Vector2, Shader), Material> Cache = new Dictionary<(Color, float, FaceType, Vector2, Shader), Material>(); // Brick Color, Transparency, Face Type, Scale, Shader
    public Material BrickMaterial;
    public Material BrickMaterialTransparent;
    public Texture Stud;
    public Texture Inlet;
    public Texture Spawnpoint;
    public Dictionary<int, Material> TextureMaterials = new Dictionary<int, Material>();
    public Dictionary<int, Texture2D> TextureDictionary = new Dictionary<int, Texture2D>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public Material GetMaterial ((Color, float, FaceType, Vector2, Shader) MaterialProperties) {
        if (Cache.TryGetValue(MaterialProperties, out Material mat)) {
            return mat;
        } else {
            Material newMat = MaterialProperties.Item2 == 1.0f ? new Material(BrickMaterial) : new Material(BrickMaterialTransparent); // use a different material if the brick has transparency

            // Set Shader
            newMat.shader = MaterialProperties.Item5;
            if (MaterialProperties.Item2 != 1.0f) newMat.renderQueue = 3000; // without this, transparent bricks have rendering issues. this also must be set after the shader, otherwise the shader settings will overwrite it

            // Set Color
            Color materialColor = MaterialProperties.Item1;
            materialColor.a = MaterialProperties.Item2;
            newMat.color = materialColor;

            // Set Texture & Scale
            if (MaterialProperties.Item3 == FaceType.Stud) {
                newMat.mainTexture = Stud;
                newMat.mainTextureScale = MaterialProperties.Item4;
            } else if (MaterialProperties.Item3 == FaceType.Inlet) {
                newMat.mainTexture = Inlet;
                newMat.mainTextureScale = MaterialProperties.Item4;
            } else if (MaterialProperties.Item3 == FaceType.Spawnpoint) {
                newMat.mainTexture = Spawnpoint;
                //newMat.mainTextureScale = MaterialProperties.Item4;
            }

            Cache.Add(MaterialProperties, newMat);
            return newMat;
        }
    }

    public Material CreateBrushTexture ((Color, float, Vector2, Shader) MaterialProperties, string local, int id) {
        if (TextureMaterials.TryGetValue(id, out Material mat)) {
            return mat;
        } else {
            Material newMat = new Material(BrickMaterial); // use a different material if the brick has transparency

            // Set Shader
            newMat.shader = MaterialProperties.Item4;
            if (MaterialProperties.Item2 != 1.0f) newMat.renderQueue = 3000; // without this, transparent bricks have rendering issues. this also must be set after the shader, otherwise the shader settings will overwrite it

            // Set Color
            // Color materialColor = MaterialProperties.Item1;
            // materialColor.a = MaterialProperties.Item2;
            // newMat.color = materialColor;

            // Set Texture & Scale

                // newMat.mainTexture = Stud;
                // newMat.mainTextureScale = MaterialProperties.Item4;


            if (TextureDictionary.TryGetValue(id, out Texture2D texturepog)) {
                newMat.mainTexture = texturepog;
            } else {
                // Button newbutton = Instantiate(buttoninst);
                byte[] bytes = System.IO.File.ReadAllBytes(local);
                Texture2D ltexture = new Texture2D(2, 2);
                ltexture.LoadImage(bytes);

                TextureDictionary.Add(id, ltexture);

                newMat.mainTexture = ltexture;
                //newbutton.transform.SetParent(texturelib.transform, true);
            }

            newMat.mainTextureScale = new Vector2(1.0f, 1.0f);

            

            TextureMaterials.Add(id, newMat);
            return newMat;
        }
    }

    public Material GetBrushMaterial (int id) {
        if (TextureMaterials.TryGetValue(id, out Material mat)) {
            return mat;
        }else{
            Material newMat = new Material(BrickMaterialTransparent); // use a different material if the brick has transparency
            

            TextureMaterials.Add(-1, newMat);
            return newMat;
        }
    }

    public enum FaceType {
        Smooth,
        Stud,
        Inlet,
        Spawnpoint
    }
}
