using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBased : MonoBehaviour
{
    bool[] pixelCheck;
    Color32[] textureMip;

    Texture2D texture;
    int textureWidth;
    int textureHeight;
    int currentMipPosition;

    public List<Vector2> FloodFill(Texture2D texture, int textureHeight, int textureWidth, Color32 colour)
    {
        this.texture = texture;
        List<Vector2> perimeter = new List<Vector2>();

        currentMipPosition = 0;

        textureMip = texture.GetPixels32();

        this.textureWidth = textureWidth;
        this.textureHeight = textureHeight;

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
            if (texture.GetPixel(currentMipPosition % textureWidth, currentMipPosition / textureWidth) == colour)
            {
                //perimeter.Add(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                StackBased4Point(perimeter, colour, currentMipPosition);
            }

            // Pixel has now been checked.
            if (!pixelCheck[pixelCount])
                pixelCheck[pixelCount] = true;

            currentMipPosition++;

        }
        textureMip = null;
        return perimeter;
    }

    // Recursive function
    void StackBased4Point(List<Vector2> perimeter, Color32 colour, int currentMipPosition)
    {
        if (currentMipPosition >= pixelCheck.Length || currentMipPosition < 0)
            return;


        // 1. If Node is not "Inside" then Return and add node to the list.
        if (this.texture.GetPixel(currentMipPosition % textureWidth, currentMipPosition / textureWidth) != colour)
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

    // I think, if you go through each pixel that isn't black, and check its neighbouring pixels to see whether they are black. 
    // If they are black, the pixel in question adds to the perimeter. 
    public List<Vector2> Finlay(Texture2D texture, int textureHeight, int textureWidth, Color32 colour)
    {
        this.textureWidth = textureWidth;
        this.textureHeight = textureHeight;
        this.texture = texture;

        List<Vector2> perimeter = new List<Vector2>();

        textureMip = texture.GetPixels32();

        // Array to corrospond to whether or not that pixel in the Mip has been checked or not.
        pixelCheck = new bool[textureWidth * textureHeight];

        for (currentMipPosition = 0; currentMipPosition < textureMip.Length; currentMipPosition++)
        {
            if (!colour.Equals(textureMip[currentMipPosition]))
            {
                // Checking if there is a North Pixel
                if (currentMipPosition + textureWidth < textureMip.Length)
                {
                    // Only with examine pixel if it has not been checked before.
                    if (colour.Equals(textureMip[currentMipPosition + textureWidth]) && !pixelCheck[currentMipPosition + textureWidth])
                    {
                        perimeter.Add(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) + 1)));
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
                        perimeter.Add(new Vector2(currentMipPosition % textureWidth, Mathf.Floor((currentMipPosition / textureHeight) - 1)));
                        pixelCheck[currentMipPosition - textureWidth] = true;
                    }
                }

                // Checking if there is an East Pixel
                if ((currentMipPosition + 1) % textureWidth != 0)
                {
                    // Only with examine pixel if it has not been checked before.
                    if (colour.Equals(textureMip[currentMipPosition + 1]) && !pixelCheck[currentMipPosition + 1])
                    {
                        perimeter.Add(new Vector2((currentMipPosition + 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                        pixelCheck[currentMipPosition + 1] = true;
                    }
                }

                // Checking if there is a West Pixel
                if ((currentMipPosition) % textureWidth != 0)
                {
                    // Only with examine pixel if it has not been checked before.
                    if (colour.Equals(textureMip[currentMipPosition - 1]) && !pixelCheck[currentMipPosition - 1])
                    {
                        perimeter.Add(new Vector2((currentMipPosition - 1) % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                        pixelCheck[currentMipPosition - 1] = true;
                    }
                }
                pixelCheck[currentMipPosition] = true;
            }
        }

        return perimeter;
    }


}
