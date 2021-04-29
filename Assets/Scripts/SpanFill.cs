using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class SpanFill
    {
        public Stack<Vector2> ScanLine(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, ref bool[] pixelCheck)
        {
            bool spanAbove = false;
            bool spanBelow = false;
            int x1;

            Stack<Vector2> temp = new Stack<Vector2>();
            temp.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

            while (temp.Count != 0)
            {
                Vector2 a = temp.Pop();
                x1 = (int)a.x;
                currentMipPosition = ((int)a.y * textureWidth);
                while (x1 >= 0 && !pixelCheck[currentMipPosition] && textureMip[x1 + currentMipPosition].Equals(colour))
                    x1--;

                x1++;

                
                // While x1 is not out of bounds and the pixel has not been checked and is the target colour
                while (x1 < textureWidth && !pixelCheck[x1 + currentMipPosition] && textureMip[x1 + currentMipPosition].Equals(colour))
                {
                    // Adding Vector to 
                    shape.Push(new Vector2(x1, a.y));
                    pixelCheck[x1 + ((int)a.y * textureWidth)] = true;

                    // If not SpanAbove, and y > 0, and the pixel hasn't been checked/added yet.
                    if (!spanAbove && a.y -1 == 0 && !pixelCheck[x1 + currentMipPosition - textureWidth] && textureMip[x1 + currentMipPosition - textureWidth].Equals(colour))
                    {
                        temp.Push(new Vector2(x1, a.y - 1));
                        spanAbove = true;
                    }
                    // Set spanAbove to false if there are no more pixels on span above pixel to be checked.
                    else if (spanAbove && a.y > 0 && pixelCheck[x1 + currentMipPosition - textureWidth] && textureMip[x1 + currentMipPosition - textureWidth].Equals(colour))
                        spanAbove = false;

                    // If not SpanBelow, and y > 0, and the pixel hasn't been checked/added yet.
                    if (!spanBelow && a.y < textureHeight - 1 && !pixelCheck[x1 + currentMipPosition - textureWidth] && textureMip[x1 + currentMipPosition + textureWidth].Equals(colour))
                    {
                        temp.Push(new Vector2(x1, a.y + 1));
                        spanBelow = true;
                    }
                    // Set spanBelow to false if there are no more pixels on span above pixel to be checked.
                    else if (!spanBelow && a.y < textureHeight - 1 && pixelCheck[x1 + currentMipPosition - textureWidth] && textureMip[x1 + currentMipPosition - textureWidth].Equals(colour))
                        spanBelow = false;

                    x1++;
                }
            }
            return shape;
        }


        public Queue<Vector2> ScanLine(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            int x1;

            Queue<Vector2> temp = new Queue<Vector2>();
            temp.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

            while (temp.Count != 0)
            {
                Vector2 a = temp.Dequeue();
                x1 = (int)a.x;
                currentMipPosition = ((int)a.y * textureWidth);
                while (x1 >= 0 && textureMip[x1 + currentMipPosition].Equals(colour))
                    x1--;

                x1++;

                bool spanAbove = false;
                bool spanBelow = false;

                // While x1 is not out of bounds and the pixel has not been checked and is the target colour
                while (x1 < textureWidth && !pixelCheck[x1 + ((int)a.y * textureWidth)] && textureMip[x1 + ((int)a.y * textureWidth)].Equals(colour))
                {
                    // Adding Vector to shape.
                    shape.Enqueue(new Vector2(x1, a.y));
                    pixelCheck[x1 + ((int)a.y * textureWidth)] = true;

                    // If not SpanAbove, and y > 0, and the pixel hasn't been checked/added yet.
                    if (!spanAbove && a.y > 0 && !pixelCheck[x1 + (((int)a.y - 1) * textureWidth)] && textureMip[x1 + (((int)a.y - 1) * textureWidth)].Equals(colour))
                    {
                        temp.Enqueue(new Vector2(x1, a.y - 1));
                        spanAbove = true;
                    }
                    // Set spanAbove to false if there are no more pixels on span above pixel to be checked.
                    else if (spanAbove && a.y - 1 == 0 && pixelCheck[x1 + (((int)a.y - 1) * textureWidth)] && !textureMip[x1 + (((int)a.y - 1) * textureWidth)].Equals(colour))
                        spanAbove = false;

                    // If not SpanBelow, and y > 0, and the pixel hasn't been checked/added yet.
                    if (!spanBelow && a.y < textureHeight - 1 && !pixelCheck[x1 + (((int)a.y + 1) * textureWidth)] && textureMip[x1 + (((int)a.y + 1) * textureWidth)].Equals(colour))
                    {
                        temp.Enqueue(new Vector2(x1, a.y + 1));
                        spanBelow = true;
                    }
                    /// Set spanBelow to false if there are no more pixels on span above pixel to be checked.
                    else if (spanBelow && a.y < textureHeight - 1 && pixelCheck[x1 + (((int)a.y + 1) * textureWidth)] && !textureMip[x1 + (((int)a.y + 1) * textureWidth)].Equals(colour))
                    {
                        spanBelow = false;
                    }
                    x1++;
                }
            }
            return shape;
        }
    }
}
