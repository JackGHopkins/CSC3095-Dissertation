using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class NeighbourChecking
    {
        bool[] pixelCheck;
        Color32[] textureMip;

        Texture2D texture;
        int textureWidth;
        int textureHeight;
        int currentMipPosition;

        // I think, if you go through each pixel that isn't black, and check its neighbouring pixels to see whether they are black. 
        // If they are black, the pixel in question adds to the shape. 


        /*
         *  STACK BASED
         */
        public Stack<Vector2> FFNeighbourChecking(Stack<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool perimeter)
        {
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.texture = texture;

            textureMip = texture.GetPixels32();

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            for (currentMipPosition = 0; currentMipPosition < textureMip.Length; currentMipPosition++)
            {
                if ((!colour.Equals(textureMip[currentMipPosition]) || !perimeter))
                {
                    // Checking if there is a North Pixel
                    if (currentMipPosition + textureWidth < textureMip.Length)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition + textureWidth]) && !pixelCheck[currentMipPosition + textureWidth])
                        {
                            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) + 1)));
                            pixelCheck[currentMipPosition + textureWidth] = true;
                        }
                    }

                    // Checking if there is a South Pixel
                    if (currentMipPosition - textureWidth >= 0)
                    {
                        //Debug.Log("South Pixel");
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition - textureWidth]) && !pixelCheck[currentMipPosition - textureWidth])
                        {
                            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) - 1)));
                            pixelCheck[currentMipPosition - textureWidth] = true;
                        }
                    }

                    // Checking if there is an East Pixel
                    if ((currentMipPosition + 1) % textureWidth != 0)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition + 1]) && !pixelCheck[currentMipPosition + 1])
                        {
                            shape.Push(new Vector2((currentMipPosition + 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                            pixelCheck[currentMipPosition + 1] = true;
                        }
                    }

                    // Checking if there is a West Pixel
                    if ((currentMipPosition) % textureWidth != 0)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition - 1]) && !pixelCheck[currentMipPosition - 1])
                        {
                            shape.Push(new Vector2((currentMipPosition - 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                            pixelCheck[currentMipPosition - 1] = true;
                        }
                    }
                    pixelCheck[currentMipPosition] = true;
                }
            }
            return shape;
        }

        /*
         *  QUEUE BASED
         */
        public Queue<Vector2> FFNeighbourChecking(Queue<Vector2> shape, Texture2D texture, int textureHeight, int textureWidth, Color32 colour, bool perimeter)
        {
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.texture = texture;

            textureMip = texture.GetPixels32();

            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureWidth * textureHeight];

            for (currentMipPosition = 0; currentMipPosition < textureMip.Length; currentMipPosition++)
            {
                if ((!colour.Equals(textureMip[currentMipPosition]) || !perimeter))
                {
                    // Checking if there is a North Pixel
                    if (currentMipPosition + textureWidth < textureMip.Length)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition + textureWidth]) && !pixelCheck[currentMipPosition + textureWidth])
                        {
                            shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) + 1)));
                            pixelCheck[currentMipPosition + textureWidth] = true;
                        }
                    }

                    // Checking if there is a South Pixel
                    if (currentMipPosition - textureWidth >= 0)
                    {
                        //Debug.Log("South Pixel");
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition - textureWidth]) && !pixelCheck[currentMipPosition - textureWidth])
                        {
                            shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) - 1)));
                            pixelCheck[currentMipPosition - textureWidth] = true;
                        }
                    }

                    // Checking if there is an East Pixel
                    if ((currentMipPosition + 1) % textureWidth != 0)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition + 1]) && !pixelCheck[currentMipPosition + 1])
                        {
                            shape.Enqueue(new Vector2((currentMipPosition + 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                            pixelCheck[currentMipPosition + 1] = true;
                        }
                    }

                    // Checking if there is a West Pixel
                    if ((currentMipPosition) % textureWidth != 0)
                    {
                        // Only with examine pixel if it has not been checked before.
                        if (colour.Equals(textureMip[currentMipPosition - 1]) && !pixelCheck[currentMipPosition - 1])
                        {
                            shape.Enqueue(new Vector2((currentMipPosition - 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                            pixelCheck[currentMipPosition - 1] = true;
                        }
                    }
                    pixelCheck[currentMipPosition] = true;
                }
            }
            return shape;
        }
    }
}
