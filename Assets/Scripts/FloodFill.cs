using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.Scripts
{
    class FloodFill : MonoBehaviour
    {
        bool[] pixelCheck;
        Color32[] textureMip;
        int currentMipPosition;

        // This method is to find the first pixel of that colour in the shape.
        public Stack<Vector2> FFStack(Stack<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, FloodFillAlgorithm algorithm, Stopwatch stopWatch)
        {
            textureMip = texture.GetPixels32();
            currentMipPosition = 0;
            List<long> times = new List<long>();


            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (algorithm == FloodFillAlgorithm.BRUTE_FORCE)
            {
                BruteForce bF = new BruteForce();
                bF.UpdateShape(shape, textureHeight, textureWidth, colour, textureMip);
            }
            else if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
            }
            else
            {
                for (int pixelCount = 0; pixelCount < pixelCheck.Length; pixelCount++)
                {
                    // Go to next loop if pixel has been checked.
                    if (pixelCheck[pixelCount])
                    {
                        currentMipPosition++;
                        continue;
                    }

                    // Start flood fill if colours are the same.
                    if (textureMip[currentMipPosition].Equals(colour))
                    {
                        if (algorithm == FloodFillAlgorithm.FOUR_WAY_RECURSION)
                        {
                            FourWay fW = new FourWay();
                            fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            FourWay fW = new FourWay();
                            fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            SpanFill sF = new SpanFill();
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            WalkBasedFill wF = new WalkBasedFill();
                            wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                    }
                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;
                }
                textureMip = null;
            }

            //PrintTime(times);
            return shape;
        }

        public Queue<Vector2> FFQueue(Queue<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, FloodFillAlgorithm algorithm, Stopwatch stopWatch)
        {
            textureMip = texture.GetPixels32();
            currentMipPosition = 0;
            List<long> times = new List<long>();


            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (algorithm == FloodFillAlgorithm.BRUTE_FORCE)
            {
                BruteForce bF = new BruteForce();
                bF.UpdateShape(shape, textureHeight, textureWidth, colour, textureMip);
            }
            else if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
            }
            else
            {
                for (int pixelCount = 0; pixelCount < pixelCheck.Length; pixelCount++)
                {
                    // Go to next loop if pixel has been checked.
                    if (pixelCheck[pixelCount])
                    {
                        currentMipPosition++;
                        continue;
                    }

                    // Start flood fill if colours are the same.
                    if (textureMip[currentMipPosition].Equals(colour))
                    {
                        if (algorithm == FloodFillAlgorithm.FOUR_WAY_RECURSION)
                        {
                            FourWay fW = new FourWay();
                            fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            FourWay fW = new FourWay();
                            fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            SpanFill sF = new SpanFill();
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            WalkBasedFill wF = new WalkBasedFill();
                            wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                        }
                    }
                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;
                }
                textureMip = null;
            }
            //PrintTime(times);
            return shape;
        }

        void PrintTime(List<long> times)
        {

            for (int i = 0; i < times.Count; i++)
            {
                print("Shape " + i + " : " + times[i]);
            }
        }
    }
}
