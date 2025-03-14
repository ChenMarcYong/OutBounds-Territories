using System.Diagnostics;
using UnityEngine;

public class MergeMeshes : MonoBehaviour
{
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        if (meshFilters.Length == 0)
        {
            UnityEngine.Debug.LogError("Aucun MeshFilter trouvé sur les enfants de " + gameObject.name);
            return; // Sortie de la fonction si aucun MeshFilter n'est trouvé
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (meshFilters[i].sharedMesh == null) continue; // Ignore les objets sans Mesh
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // Désactive les anciens objets
        }

        Mesh mergedMesh = new Mesh();
        mergedMesh.CombineMeshes(combine);

        // Vérifie si le GameObject a un MeshFilter, sinon ajoute-en un
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = mergedMesh;

        // Ajoute un MeshCollider basé sur le Mesh fusionné
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null) meshCollider = gameObject.AddComponent<MeshCollider>();

        meshCollider.sharedMesh = mergedMesh;
        meshCollider.convex = true; // Utile si le vaisseau doit avoir un Rigidbody

        gameObject.SetActive(true);
    }
}
