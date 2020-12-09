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
    private int buildingNo;


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
        buildingNo = Random.Range(0, buildingArray.Length - 1);
        Transform randomTransform = buildingArray[buildingNo].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, GetSpawnPostion(), Quaternion.identity) as GameObject;      
        Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
        Bounds bounds = cloneMesh.bounds;
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

            if (texture.GetPixel(x,z) != Color.black && BoundsCheck(scaleRX, scaleRZ, x, z))
            {
                positionValid = true;
            }

        }
        Vector3 position = new Vector3((x * scaleRX) - (this.transform.localScale.x / 2), 0, (z * scaleRZ) - (this.transform.localScale.x / 2));
        return position;
    }

    private bool BoundsCheck(float scaleRX, float scaleRZ, int x, int z)
    {
        int sizeX = Mathf.RoundToInt(baseParts[buildingNo].GetComponent<MeshRenderer>().bounds.max.x);
        int sizeZ = Mathf.RoundToInt(baseParts[buildingNo].GetComponent<MeshRenderer>().bounds.max.z);

        int newX = Mathf.CeilToInt((x * scaleRX) - (this.transform.localScale.x / 2));
        int newZ = Mathf.CeilToInt((z * scaleRZ) - (this.transform.localScale.z / 2));

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
