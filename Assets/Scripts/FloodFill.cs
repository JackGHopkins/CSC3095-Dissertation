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
                //for (int i = 0; i < 1000; i++)
                //{
                //    stopWatch.Start();
                Profiler.BeginSample("My Sample");
                UnityEngine.Debug.Log("This code is being profiled");
                bF.UpdateShape(shape, textureHeight, textureWidth, colour, textureMip);
                Profiler.EndSample();
                //    stopWatch.Stop();
                //}

            }
            else if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                //for (int i = 0; i < pixelTemp.Length; ++i)
                //{
                //    pixelTemp[i] = pixelCheck[i];
                //}
                //int tempMipPos = currentMipPosition;
                //for (int i = 0; i < 1000; i++)
                //{
                //    for (int j = 0; j < pixelTemp.Length; ++j)
                //    {
                //        pixelCheck[j] = pixelTemp[j];
                //    }
                //    currentMipPosition = tempMipPos;
                //    stopWatch.Start();
                    nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
                //    stopWatch.Stop();
                //}
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
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            //    stopWatch.Stop();
                            //}
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            FourWay fW = new FourWay();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            //    stopWatch.Stop();
                            //}
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            SpanFill sF = new SpanFill();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            //    stopWatch.Stop();
                            //}
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            WalkBasedFill wF = new WalkBasedFill();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            //    stopWatch.Stop();
                            //}
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
                //for (int i = 0; i < 1000; i++)
                //{

                Profiler.BeginSample("My Sample");
                UnityEngine.Debug.Log("This code is being profiled");
                    bF.UpdateShape(shape, textureHeight, textureWidth, colour, textureMip);
 

                Profiler.EndSample();
                //}

            }
            else if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                //for (int i = 0; i < pixelTemp.Length; ++i)
                //{
                //    pixelTemp[i] = pixelCheck[i];
                //}
                //int tempMipPos = currentMipPosition;
                //for (int i = 0; i < 1000; i++)
                //{
                //    for (int j = 0; j < pixelTemp.Length; ++j)
                //    {
                //        pixelCheck[j] = pixelTemp[j];
                //    }
                //    currentMipPosition = tempMipPos;
                //    stopWatch.Start();
                    nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
                //    stopWatch.Stop();
                //}
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
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            //    stopWatch.Stop();
                            //}
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            FourWay fW = new FourWay();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                                Profiler.BeginSample("My Sample");
                                UnityEngine.Debug.Log("This code is being profiled");
                                fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
  
                                Profiler.EndSample();
                            //}
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            SpanFill sF = new SpanFill();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            print(Profiler.GetTotalAllocatedMemoryLong());
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            print(Profiler.GetTotalAllocatedMemoryLong());
                            //    stopWatch.Stop();
                            //}
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            WalkBasedFill wF = new WalkBasedFill();
                            //bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            //for (int i = 0; i < pixelTemp.Length; ++i)
                            //{
                            //    pixelTemp[i] = pixelCheck[i];
                            //}
                            //int tempMipPos = currentMipPosition;
                            //for (int i = 0; i < 1000; i++)
                            //{
                            //    for (int j = 0; j < pixelTemp.Length; ++j)
                            //    {
                            //        pixelCheck[j] = pixelTemp[j];
                            //    }
                            //    currentMipPosition = tempMipPos;
                            //    stopWatch.Start();
                            Profiler.BeginSample("My Sample");
                            UnityEngine.Debug.Log("This code is being profiled");
                            wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                            Profiler.EndSample();
                            print(Profiler.GetTotalAllocatedMemoryLong());
                            //stopWatch.Stop();
                            // }
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
