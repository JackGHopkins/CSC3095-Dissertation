using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /*
     * 
     *  BRUTE FORCE: Iterates through every pixel, adding them to the array
     * 
     */
    class BruteForce
    {
        public void UpdateShape(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip)
        {
            for (int currentMipPosition = 0; currentMipPosition < textureMip.Length; currentMipPosition++)
                if (textureMip[currentMipPosition].Equals(colour))
                    shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        }  
        public void UpdateShape(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip)
        {
            for (int currentMipPosition = 0; currentMipPosition < textureMip.Length; currentMipPosition++)
                if (textureMip[currentMipPosition].Equals(colour))
                    shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        }
    }
}
