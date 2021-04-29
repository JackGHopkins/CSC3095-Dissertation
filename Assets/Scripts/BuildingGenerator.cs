using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

namespace Assets.Scripts
{
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

    [System.Serializable]
    public class WaypointPath
    {
        public string name;
        public int buildingSpacing;
        public bool perimeter;
        public GameObject[] meshes;
    }

    public class MyWaypointScript : MonoBehaviour
    {
        public WaypointPath[] paths;
    }

    public class BuildingGenerator : MonoBehaviour
    {
        public int minPieces = 1;
        public int maxPieces = 5;

        public uint textureGridStepSizeX = 4;
        public uint textureGridStepSizeY = 4;
        public uint imgWidth = 512;
        public uint imgHeight = 512;
        public float multipledImgSize = 1.0f;

        public bool stack = false;
        public bool fourWay = false;
        public bool recursion = false;
        public bool spanFill = false;
        public bool walkFill = false;
        public bool neighbourChecking = false;


        private bool instantiated;
        private bool positionValid;

        private int buildingIndex;
        private int buildingNo;

        [SerializeField] Texture2D texture;
        [SerializeField] WaypointPath[] meshesArray;
        [SerializeField] Color32[] colours;


        // Start is called before the first frame update
        void Awake()
        {
            Build();
            Build();
        }

        void Build()
        {
            SpawnBuildingFloodFill();
        }

        void SpawnBuildingFloodFill()
        {
            FloodFill ff = gameObject.AddComponent<FloodFill>();

            for (int i = 0; i < colours.Length; i++)
            {
                if (stack)
                {
                    Stack<Vector2> shape = new Stack<Vector2>();
                    shape = ff.FFStack(shape, texture, (int)imgWidth, (int)imgHeight, colours[i], recursion, fourWay, spanFill, neighbourChecking, walkFill);

                    for (int j = 0; j < shape.Count;)
                    {
                        //sortShape(shape);
                        //SpawnBuilding(meshesArray[i].meshes, shape.ToArray()[j]);

                        j = j + meshesArray[i].buildingSpacing;
                    }
                }
                else
                {
                    Queue<Vector2> shape = new Queue<Vector2>();
                    shape = ff.FFQueue(shape, texture, (int)imgWidth, (int)imgHeight, colours[i], recursion, fourWay, spanFill, neighbourChecking, walkFill);

                    for (int j = 0; j < shape.Count;)
                    {
                        //sortShape(shape);
                        //SpawnBuilding(meshesArray[i].meshes, shape.ToArray()[j]);

                        j = j + meshesArray[i].buildingSpacing;
                    }
                }
            }
        }

        void SpawnBuilding(GameObject[] buildingArray, Vector2 position)
        {
            buildingNo = Random.Range(0, buildingArray.Length - 1);
            Transform randomTransform = buildingArray[buildingNo].transform;
            GameObject clone = Instantiate(randomTransform.gameObject, new Vector3((position.x + 0.5f) * multipledImgSize, 0, (position.y + 0.5f) * multipledImgSize), Quaternion.identity) as GameObject;
            Mesh cloneMesh = clone.GetComponentInChildren<MeshFilter>().mesh;
            Bounds bouasdfasnds = cloneMesh.bounds;
        }

        // Turns Stack / Queue into an Array
        void sortShape(Stack<Vector2> shape)
        {
            sortShape(shape.ToArray());
        }
    
        void sortShape(Queue<Vector2> shape)
        {
            sortShape(shape.ToArray());
        }

        void sortShape(Vector2[] shape)
        {
            for (int i = 0; i < shape.Length - 1; i++)
            {
                int yAxis = 0;
                int diagonal = 0;
                // Continue if next coord is [x+1 || x-1,y]
                if (shape[i].y == shape[i + 1].y && (shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x))
                    continue;

                for (int j = i + 2; j < shape.Length; j++)
                {
                    if (shape[i].y == shape[j].y && (shape[i].x + 1 == shape[j].x || shape[i].x - 1 == shape[j].x))
                    {
                        Vector2 temp = shape[i + 1];
                        shape[i + 1] = shape[j];
                        shape[j] = temp;
                        break;
                    }

                    if (shape[i].x == shape[j].x && (shape[i].y + 1 == shape[j].y || shape[i].y - 1 == shape[j].y))
                    {
                        yAxis = j;
                        continue;
                    }

                    // Continue if next coord is [x+1 || x-1, y+1 || y-1]
                    if ((shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x) && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                        diagonal = j;
                }

                // Continue if next coord is [x, y+1 || y-1]
                if (shape[i].x == shape[i + 1].x && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                    continue;

                if (shape[i].x == shape[yAxis].x && (shape[i].y + 1 == shape[yAxis].y || shape[i].y - 1 == shape[yAxis].y))
                {
                    Vector2 temp = shape[i + 1];
                    shape[i + 1] = shape[yAxis];
                    shape[yAxis] = temp;
                    continue;
                }

                // Continue if next coord is [x+1 || x-1, y+1 || y-1]
                if ((shape[i].x + 1 == shape[i + 1].x || shape[i].x - 1 == shape[i + 1].x) && (shape[i].y + 1 == shape[i + 1].y || shape[i].y - 1 == shape[i + 1].y))
                    continue;

                if ((shape[i].x + 1 == shape[diagonal].x || shape[i].x - 1 == shape[diagonal].x) && (shape[i].y + 1 == shape[diagonal].y || shape[i].y - 1 == shape[diagonal].y))
                {
                    Vector2 temp = shape[i + 1];
                    shape[i + 1] = shape[diagonal];
                    shape[diagonal] = temp;
                    continue;
                }
            }
        }

        static void PrintText(string text)
        {
            string filegen, filetxt;
            ProcessStartInfo psi;
            Process proc;

            filegen = Path.GetTempFileName();
            filetxt = filegen + ".txt";
            File.Move(filegen, filetxt);
            File.AppendAllText(filetxt, text);

            psi = new ProcessStartInfo(filetxt);
            psi.Verb = "PRINT";
            proc = Process.Start(psi);
            proc.WaitForExit();
            File.Delete(filetxt);
        }
    }
}

 