using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

struct Coordinate
{
    int x;
    int y;
    // To distinguish if road is on left or right of the pixel.
    int shapeNo;
    bool leftOfRoad;

    public Coordinate(int x, int y, int shapeNo, bool leftOfRoad) 
    {
        this.x = x;
        this.y = y;
        this.shapeNo = shapeNo;
        this.leftOfRoad = leftOfRoad;
    }
}

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

    [SerializeField] int buildingSpacing;
    [SerializeField] Color32[] colors;


    // Start is called before the first frame update
    void Awake()
    {
        Build();
    }

    void Build()
    {
        //int targetPieces = Random.Range(minPieces, maxPieces);

        //for (buildingIndex = minPieces - 1; buildingIndex < maxPieces; buildingIndex++)
        //{
        //    SpawnBuilding(baseParts);
        //}

        SpawnBuildingFloodFill();
    }

    void SpawnBuildingFloodFill()
    {
        StackBased sb = gameObject.AddComponent<StackBased>();
        List<Vector2> perimeter = sb.FloodFill(texture, (int)imgWidth, (int)imgHeight, colors);

        for(int i = 0; i < perimeter.Count;)
        {
            SpawnBuilding(baseParts, perimeter[i]);

            i = i + buildingSpacing; 
        }
    }

    void SpawnBuilding(GameObject[] buildingArray, Vector2 position)
    {
        buildingNo = Random.Range(0, buildingArray.Length - 1);
        Transform randomTransform = buildingArray[buildingNo].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, new Vector3(position.x, 0, position.y), Quaternion.identity) as GameObject;      
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

    // This function is trying to map out where the roads are.
    public void ChartRoads()
    {
        bool onRoad;
        int shapeCount = 0;

        List<Coordinate> roadCoord = new List<Coordinate>();
        Color[] pixels = texture.GetPixels(0,0, (int)imgWidth, (int)imgHeight);

        if (pixels[0] == Color.black)
        {
            onRoad = true;
        }
        else
        {
            onRoad = false;
            Coordinate coord = new Coordinate(0, 0, shapeCount, false);
            roadCoord.Add(coord);
        }
        
        for(int i = 1; i <= imgWidth * imgHeight; i++)
        {
           if(pixels[i] != Color.black && pixels[i + 1] == Color.black)
           {
                //roadCoord.Add(new Coordinate(i % (int)imgWidth, Mathf.FloorToInt(imgWidth / i), shapeCount, false));

                /*
                 * 1. Check adjaycent pixel. 3x3 with center being pixel[i]  
                 * 2. If one is free then make index equal that spot. 
                 * 3. 
                 */

                // Check pixel to the right
                if (pixels[i + 1] != Color.black)
                {

                } 
                // Check pixel above. Also, checks above is not null.
                else if (i < (imgWidth * imgHeight) - imgWidth && pixels[i + imgWidth] != Color.black)
                {

                }
                // Check pixel below. Also, checks below is not null.
                else if (i > imgWidth && pixels[i + imgWidth] != Color.black)
                {

                }
                // Check pixel top right corner.
                else if (pixels[(i + imgWidth) + 1] != Color.black)
                {

                } 
                // Check pixel top left corner.
                else if (pixels[(i + imgWidth) - 1] != Color.black)
                {

                }
                // Check pixel bottom right corner.
                else if (pixels[(i - imgWidth) + 1] != Color.black)
                {

                }
                // Check pixel bottom left corner.
                else if (pixels[(i - imgWidth) - 1] != Color.black)
                {

                }

            }
        }
    }

    void checkSurroundingPixels(Color[] pixels, int i)
    {
        // Check pixel to the right
        if(pixels[i + 1] == Color.black)
        {

        }
    }
}
