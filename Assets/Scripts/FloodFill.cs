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

        Texture2D texture;
        int textureWidth;
        int textureHeight;
        int currentMipPosition;
        int rounds;

        // This method is to find the first pixel of that colour in the shape.
        public Stack<Vector2> FFStack(Stack<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool recursion, bool fourWay, bool spanFill, bool neighbourChecking, bool walkFill)
        {
            this.texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;

            textureMip = texture.GetPixels32();
            currentMipPosition = 0;

            Stopwatch stopWatch = new Stopwatch();
            List<long> times = new List<long>();

            stopWatch.Start();

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (neighbourChecking)
            {
                NeighbourChecking nB = new NeighbourChecking();
                nB.FFNeighbourChecking(shape, textureHeight, textureWidth, colour, textureMip);
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
                        if (fourWay)
                        {
                            FourWay fW = new FourWay();
                            if (recursion)
                            {
                                fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                            }
                            else
                            {
                                fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                            }
                        }
                        else if (spanFill)
                        {
                            SpanFill sF = new SpanFill();
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                        } 
                        else if (walkFill)
                        {
                            WalkBasedFilling wF = new WalkBasedFilling();
                            //wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                        }
                    }

                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;

                }
                textureMip = null;
            }
            stopWatch.Stop();
            times.Add(stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();

            PrintTime(times);
            return shape;
        }

        public Queue<Vector2> FFQueue(Queue<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool recursion, bool fourWay, bool spanFill, bool neighbourChecking, bool walkFill)
        {
            this.texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;

            textureMip = texture.GetPixels32();
            currentMipPosition = 0;
            rounds = 0;


            Stopwatch stopWatch = new Stopwatch();
            List<long> times = new List<long>();

            stopWatch.Start();

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            if (neighbourChecking)
            {
                NeighbourChecking nB = new NeighbourChecking();
                nB.FFNeighbourChecking(shape, textureHeight, textureWidth, colour, textureMip);
                rounds++;
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
                        if (fourWay)
                        {
                            FourWay fW = new FourWay();
                            if (recursion)
                            {
                                fW.FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                            }
                            else
                            {
                                fW.FourWayLinear(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                                rounds++;
                            }
                        }
                        else if (spanFill)
                        {
                            SpanFill sF = new SpanFill();
                            sF.ScanLine(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                            rounds++;
                        }
                        else if (walkFill)
                        {
                            WalkBasedFilling wF = new WalkBasedFilling();
                            //wF.Painter(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                        }
                    }

                    // Pixel has now been checked.
                    if (!pixelCheck[pixelCount])
                        pixelCheck[pixelCount] = true;

                    currentMipPosition++;

                }
                textureMip = null;
            }
            stopWatch.Stop();
            times.Add(stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();

            print("Shape recursions: " + rounds);
            print("Shape Points (Pixels): " + shape.Count());

            //PrintTime(times);
            return shape;
        }

        void PrintTime(List<long> times)
        {

            for (int i = 0; i < times.Count; i++)
            {
                print(times[i]);
            }
        }
    }
}
