using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBased : MonoBehaviour
{
    bool[] pixelCheck;
    Color32[] textureMip;

    int textureWidth;
    int textureHeight;
    int currentMipPosition;

    public List<Vector2> FloodFill(Texture2D texture, int textureHeight, int textureWidth, Color32[] colours)
    {
        List<Vector2> perimeter = new List<Vector2>();

        currentMipPosition = 0;

        textureMip = texture.GetPixels32();

        this.textureWidth = textureWidth;
        this.textureHeight = textureHeight;

        foreach (Color32 currentColour in colours)
        {
            currentMipPosition = 0;
            // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
            pixelCheck = new bool[textureMip.Length];

            for (int pixelCount = 0; pixelCount < textureMip.Length; pixelCount++)
            {
                // Go to next loop if pixel has been checked.
                if (pixelCheck[pixelCount])
                {
                    currentMipPosition++;
                    continue;
                }

                // Start flood fill if colours are the same.
                if (textureMip[currentMipPosition].Equals(currentColour))
                {
                    //perimeter.Add(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                    StackBased4Point(perimeter, currentColour, currentMipPosition);
                }

                // Pixel has now been checked.
                if (!pixelCheck[pixelCount])
                    pixelCheck[pixelCount] = true;

                currentMipPosition++;
            }
        }
        return perimeter;
    }

    // Recursive function
    void StackBased4Point(List<Vector2> perimeter, Color32 colour, int currentMipPosition)
    {
        if (currentMipPosition >= textureMip.Length || currentMipPosition < 0)
            return;


        // 1. If Node is not "Inside" then Return and add node to the list.
        if (!textureMip[currentMipPosition].Equals(colour))
        {
            // Mip's array count goes from left to right, bottom to top. Hence, we find the modulo for the X coord and divide for the Y coord.
            perimeter.Add(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

            Debug.Log("message");

            // Set pixel to Checked.
            pixelCheck[currentMipPosition] = true;
            return;
        }

        if (pixelCheck[currentMipPosition])
            return;

        // 2. Set the node
        pixelCheck[currentMipPosition] = true;

        // 3. Perform Flood-fill one step to the south of the node
        StackBased4Point(perimeter, colour, currentMipPosition - textureWidth);

        // 4. Perform Flood-fill one step to the north of the node
        StackBased4Point(perimeter, colour, currentMipPosition + textureWidth);

        // 5. Perform Flood-fill one step to the west of the node
        StackBased4Point(perimeter, colour, currentMipPosition + 1);

        // 6. Perform Flood-fill one step to the east of the node
        StackBased4Point(perimeter, colour, currentMipPosition - 1);

        // 7. Return
        return;
    }
}
