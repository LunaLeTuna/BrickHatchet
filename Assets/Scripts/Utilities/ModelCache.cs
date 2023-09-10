using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCache : MonoBehaviour
{
    public static ModelCache instance;

    public Mesh Default_Model;
    public Dictionary<int, Mesh> ModelMaterials = new Dictionary<int, Mesh>();

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public Mesh CreateLeModel (string local, int id) {
        if (ModelMaterials.TryGetValue(id, out Mesh mat)) {
            return mat;
        } else {
            Mesh newMat = FastObjImporter.Instance.ImportFile(local);

            Vector3[] verts = newMat.vertices;
            for (int i = 0; i < verts.Length; i++){
                Vector3 c = verts[i];
                c.z *= -1;
                verts[i] = c;
            }

            newMat.vertices = verts;
            int[] tris = newMat.triangles;
            for (int i = 0; i < tris.Length / 3; i++){
                int a = tris[i * 3 + 0];
                int b = tris[i * 3 + 1];
                int c = tris[i * 3 + 2];
                tris[i * 3 + 0] = c;
                tris[i * 3 + 1] = b;
                tris[i * 3 + 2] = a;
            }
            newMat.triangles = tris;

            ModelMaterials.Add(id, newMat);
            return newMat;
        }
    }

    public Mesh GetLeModel (int id) {
        if (ModelMaterials.TryGetValue(id, out Mesh mat)) {
            return mat;
        }else{
            //string ke_poject = SettingsManager.Settings.KEProjectPath;
            Mesh newMat = FastObjImporter.Instance.ImportFile("/home/luna/Documents/house/example_projects/funny cubes/pig.obj"); // use a different material if the brick has transparency
            

            //ModelMaterials.Add(-1, newMat);
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
