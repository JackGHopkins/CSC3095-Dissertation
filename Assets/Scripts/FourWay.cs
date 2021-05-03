using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class FourWay
    {
        /*
         *  STACK BASED FOUR WAY RECURSION
         */
        public void FourWayRecursion(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            if (currentMipPosition <= pixelCheck.Length && currentMipPosition > 0)
            {
                // 1. If Node is "Inside" and not yet checked then Return add node to the list and mark as Checkd
                if (textureMip[currentMipPosition].Equals(colour) && !pixelCheck[currentMipPosition])
                {
                    // Mip's array count goes from left to right, bottom to top. Hence, we find the modulo for the X coord and divide for the Y coord.
                    shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

                    // Set pixel to Checked.
                    pixelCheck[currentMipPosition] = true;
                    return;
                }

                if (pixelCheck[currentMipPosition])
                    return;

                // 2. Set the node
                pixelCheck[currentMipPosition] = true;

                // 3. Perform Flood-fill one step to the south of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition - textureWidth, pixelCheck);

                // 4. Perform Flood-fill one step to the north of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition + textureWidth, pixelCheck);

                // 5. Perform Flood-fill one step to the west of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition + 1, pixelCheck);

                // 6. Perform Flood-fill one step to the east of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition - 1, pixelCheck);

                // 7. Return
                return;
            }
        }

        /*
         *  QUEUE BASED RECURSION
         */
        public void FourWayRecursion(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            if (currentMipPosition <= pixelCheck.Length && currentMipPosition > 0)
            {
                // 1. If Node is "Inside" and not yet checked then Return add node to the list and mark as Checkd
                if (textureMip[currentMipPosition].Equals(colour) && !pixelCheck[currentMipPosition])
                {
                    // Mip's array count goes from left to right, bottom to top. Hence, we find the modulo for the X coord and divide for the Y coord.
                    shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

                    // Set pixel to Checked.
                    pixelCheck[currentMipPosition] = true;
                    return;
                }

                if (pixelCheck[currentMipPosition])
                    return;

                // 2. Set the node
                pixelCheck[currentMipPosition] = true;

                // 3. Perform Flood-fill one step to the south of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition - textureWidth, pixelCheck);

                // 4. Perform Flood-fill one step to the north of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition + textureWidth, pixelCheck);

                // 5. Perform Flood-fill one step to the west of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition + 1, pixelCheck);

                // 6. Perform Flood-fill one step to the east of the node
                FourWayRecursion(shape, textureHeight, textureWidth, colour, textureMip, currentMipPosition - 1, pixelCheck);

                // 7. Return
                return;
            }
        }

        /*
         *  STACK FOUR WAY
         */
        public void FourWayLinear(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            Stack<Vector2> temp = new Stack<Vector2>();
            temp.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

            while (temp.Count > 0)
            {
                Vector2 a = temp.Pop();
                currentMipPosition = (int)(a.x + (a.y * textureWidth));
                if (a.x < textureWidth && a.x > 0 &&
                    a.y < textureHeight && a.y > 0)
                {
                    if (textureMip[currentMipPosition].Equals(colour))
                    {
                        // Add Vector2 to main Stack.
                        if (!pixelCheck[currentMipPosition])
                            shape.Push(new Vector2(a.x - 1, a.y));

                        temp.Push(new Vector2(a.x - 1, a.y));
                        temp.Push(new Vector2(a.x + 1, a.y));
                        temp.Push(new Vector2(a.x, a.y - 1));
                        temp.Push(new Vector2(a.x, a.y + 1));

                        pixelCheck[currentMipPosition] = true;
                    }
                }
            }
            return;
        }

        /*
         *  QUEUE FOUR WAY
         */
        public void FourWayLinear(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            Queue<Vector2> temp = new Queue<Vector2>();
            temp.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

            while (temp.Count > 0)
            {
                Vector2 a = temp.Dequeue();
                currentMipPosition = (int)(a.x + (a.y * textureWidth));
                if (a.x < textureWidth && a.x > 0 &&
                    a.y < textureHeight && a.y > 0)
                {
                    if (textureMip[currentMipPosition].Equals(colour) && !pixelCheck[currentMipPosition])
                    {
                        shape.Enqueue(new Vector2(a.x - 1, a.y));

                        temp.Enqueue(new Vector2(a.x - 1, a.y));
                        temp.Enqueue(new Vector2(a.x + 1, a.y));
                        temp.Enqueue(new Vector2(a.x, a.y - 1));
                        temp.Enqueue(new Vector2(a.x, a.y + 1));

                        pixelCheck[currentMipPosition] = true;
                    }
                }
            }
            return;
        }
    }
}
