using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 1;
    public int maxPieces = 5;
    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;

    public uint textureGridStepSizeX = 4;
    public uint textureGridStepSizeY = 4;
    public uint imgWidth = 512;
    public uint imgHeight = 512;
    [SerializeField] Texture2D texture;

    private bool instantiated;
    private bool positionValid;

    private int buildingIndex;


    // Start is called before the first frame update
    void Awake()
    {
        Build(); 
    }

    void Build()
    {
        int targetPieces = Random.Range(minPieces, maxPieces);

        for (buildingIndex = minPieces - 1; buildingIndex < maxPieces; buildingIndex++)
        {
            SpawnBuilding(baseParts);
        }
    }

    void SpawnBuilding(GameObject[] buildingArray)
    {
        Transform randomTransform = buildingArray[Random.Range(0, buildingArray.Length - 1)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, GetSpawnPostion(), Quaternion.identity) as GameObject;      
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
        if (BoundsCheck(Mathf.FloorToInt(clone.transform.position.x), Mathf.FloorToInt(clone.transform.position.z)))
        {
            instantiated = true;
        }
        else
        {
            Destroy(clone);
            buildingIndex--;        
        }
    }

    Vector3 GetSpawnPostion()
    {
        float scaleRX = this.transform.localScale.x / imgWidth;
        float scaleRZ = this.transform.localScale.z / imgHeight;

        positionValid = false;
        int x = 0;
        int z = 0;
        while (!positionValid) {
            x = Mathf.FloorToInt(Random.Range(1, imgWidth));
            z = Mathf.FloorToInt(Random.Range(1, imgHeight));

            if (texture.GetPixel(x,z) != Color.black)
            {
                positionValid = true;
            }

        }
        Vector3 position = new Vector3((x * scaleRX) - (this.transform.localScale.x / 2), 0, (z * scaleRZ) - (this.transform.localScale.x / 2));
        return position;
    }

    private bool BoundsCheck(int x, int z)
    {
        int sizeX = Mathf.FloorToInt(baseParts[buildingIndex].GetComponent<Collider>().bounds.extents.x);
        int sizeZ = Mathf.FloorToInt(baseParts[buildingIndex].GetComponent<Collider>().bounds.extents.z);

        Vector3 x1z1 = new Vector3(x + sizeX, 0, z + sizeZ);
        Vector3 x1z2 = new Vector3(x + sizeX, 0, z - sizeZ);
        Vector3 x2z1 = new Vector3(x - sizeX, 0, z + sizeZ);
        Vector3 x2z2 = new Vector3(x - sizeX, 0, z - sizeZ);

        bool x1z1Flag = false;
        bool x1z2Flag = false;
        bool x2z1Flag = false;
        bool x2z2Flag = false;

        // Make flags

        if (texture.GetPixel(x + sizeX, z + sizeZ) != Color.black)
        {
            x1z1Flag = true;
        }

        if (texture.GetPixel(x + sizeX, z - sizeZ) != Color.black)
        {
            x1z2Flag = true;
        }

        if (texture.GetPixel(x - sizeX, z + sizeZ) != Color.black)
        {
            x2z1Flag = true;
        }

        if (texture.GetPixel(x - sizeX, z - sizeZ) != Color.black)
        {
            x2z2Flag = true;
        }

        if (x1z1Flag && x1z2Flag && x2z1Flag && x2z2Flag)
        {
            return true;
        }

        return false;
    }
}
