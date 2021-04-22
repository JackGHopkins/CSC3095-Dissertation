using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // This method is to find the first pixel of that colour in the shape.
        public Stack<Vector2> FFStack(Stack<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool recursion, bool fourWay, bool spanFill)
        {
            this.texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;

            textureMip = texture.GetPixels32();
            currentMipPosition = 0;

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

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
                            fW.FourPointRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                        else
                            fW.FF4Way(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                    } 
                    else if (spanFill)
                    {
                        SpanFill sF = new SpanFill();
                        sF.FFSpanFill(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                    }
                }

                // Pixel has now been checked.
                if (!pixelCheck[pixelCount])
                    pixelCheck[pixelCount] = true;

                currentMipPosition++;

            }
            textureMip = null;
            return shape;
        }

        public Queue<Vector2> FFQueue(Queue<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool recursion, bool fourWay, bool spanFill)
        {
            this.texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;

            textureMip = texture.GetPixels32();
            currentMipPosition = 0;

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

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
                            fW.FourPointRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                        else
                            fW.FF4Way(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                    }
                    else if (spanFill)
                    {
                        SpanFill sF = new SpanFill();
                        sF.FFSpanFill(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition, ref pixelCheck);
                    }
                }

                // Pixel has now been checked.
                if (!pixelCheck[pixelCount])
                    pixelCheck[pixelCount] = true;

                currentMipPosition++;

            }
            textureMip = null;
            return shape;
        }
    }
}
