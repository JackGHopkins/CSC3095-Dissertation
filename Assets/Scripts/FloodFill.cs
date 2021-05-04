using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

namespace Assets.Scripts
{
    class FloodFill : MonoBehaviour
    {
        bool[] pixelCheck;
        Color32[] textureMip;
        int currentMipPosition;
        int rounds;

        // This method is to find the first pixel of that colour in the shape.
        public Stack<Vector2> FFStack(Stack<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, FloodFillAlgorithm algorithm)
        {
            textureMip = texture.GetPixels32();
            currentMipPosition = 0;
            rounds = 0;


            Stopwatch stopWatch = new Stopwatch();
            List<long> times = new List<long>();


            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                for (int i = 0; i < 1000; i++)
                {
                    stopWatch.Start();
                    nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
                    stopWatch.Stop();
                }
                times.Add(stopWatch.ElapsedMilliseconds);
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

                            bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            print(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            FourWay fW = new FourWay();
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];

                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            SpanFill sF = new SpanFill();
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];

                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            WalkBasedFill wF = new WalkBasedFill();
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                    }

                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;

                }
                textureMip = null;
            }

            print("Shape rounds: " + rounds);
            print("Shape recursions: " + times.Count);
            print("Shape Points (Pixels): " + shape.Count());

            PrintTime(times);
            return shape;
        }

            public Queue<Vector2> FFQueue(Queue<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, FloodFillAlgorithm algorithm)
        {
            textureMip = texture.GetPixels32();
            currentMipPosition = 0;
            rounds = 0;


            Stopwatch stopWatch = new Stopwatch();
            List<long> times = new List<long>();


            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (algorithm == FloodFillAlgorithm.PERIMETER_FILL)
            {
                PerimeterFill nB = new PerimeterFill();
                for (int i = 0; i < 1000; i++) {
                    stopWatch.Start();
                    nB.PerimeterFind(shape, textureHeight, textureWidth, colour, textureMip, pixelCheck);
                    stopWatch.Stop();
                }
                times.Add(stopWatch.ElapsedMilliseconds);
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

                            bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            print(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }

                        else if (algorithm == FloodFillAlgorithm.FOUR_WAY_LINEAR)
                        {
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            FourWay fW = new FourWay();
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                        else if (algorithm == FloodFillAlgorithm.SPAN_FILL)
                        {
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];
                            
                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            SpanFill sF = new SpanFill();
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                        else if (algorithm == FloodFillAlgorithm.WALK_BASED_FILL)
                        {
                            bool[] pixelTemp = new bool[textureHeight * textureWidth];

                            for (int i = 0; i < pixelTemp.Length; ++i)
                            {
                                pixelTemp[i] = pixelCheck[i];
                            }
                            int tempMipPos = currentMipPosition;
                            WalkBasedFill wF = new WalkBasedFill();
                            for (int i = 0; i < 1000; i++)
                            {
                                for (int j = 0; j < pixelTemp.Length; ++j)
                                {
                                    pixelCheck[j] = pixelTemp[j];
                                }
                                currentMipPosition = tempMipPos;
                                stopWatch.Start();
                                wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, pixelCheck);
                                stopWatch.Stop();
                                shape.Clear();
                            }
                            times.Add(stopWatch.ElapsedMilliseconds);
                            stopWatch.Reset();
                        }
                    }

                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;

                }
                textureMip = null;
            }


            print("Shape rounds: " + rounds);
            print("Shape recursions: " + times.Count);
            print("Shape Points (Pixels): " + shape.Count());

            PrintTime(times);
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
