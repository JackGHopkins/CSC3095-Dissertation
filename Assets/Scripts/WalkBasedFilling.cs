using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Pixel
    {
        public int x { get; set; }
        public int y { get; set; }
        public Pixel(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    enum Direction
    {
        UP, DOWN, LEFT, RIGHT,
    }
    class WalkBasedFilling
    {
        int mark, mark2;
        Direction currentDirection, markDirection, mark2Direction;
        bool backtrack, findLoop;
        int count;

        int currentMipPosition;
        int textureWidth;
        int textureHeight;
        public Stack<Vector2> Painter(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, ref bool[] pixelCheck)
        {
            this.currentMipPosition = currentMipPosition;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            currentDirection = Direction.RIGHT;

            if (currentDirection == Direction.DOWN && currentMipPosition < textureMip.Length - textureWidth)
                while (textureMip[currentMipPosition + textureWidth].Equals(colour) && !pixelCheck[currentMipPosition + textureWidth])
                    MoveForward();

            if (currentDirection == Direction.UP && currentMipPosition > textureWidth)
                while (textureMip[currentMipPosition - textureWidth].Equals(colour) && !pixelCheck[currentMipPosition - textureWidth])
                    MoveForward();

            if (currentDirection == Direction.RIGHT)
                while (textureMip[currentMipPosition + 1].Equals(colour) && !pixelCheck[currentMipPosition + 1])
                    MoveForward();

            if (currentDirection == Direction.LEFT)
                while (textureMip[currentMipPosition - 1].Equals(colour) && !pixelCheck[currentMipPosition - 1])
                    MoveForward();



            return shape;
        }

        void MoveForward()
        {
            switch (currentDirection)
            {
                case Direction.UP:
                    currentMipPosition =+ textureWidth;
                    break;
                case Direction.DOWN:
                    currentMipPosition =- textureWidth;
                    break;
                case Direction.LEFT:
                    currentMipPosition =- 1;
                    break;
                case Direction.RIGHT:
                    currentMipPosition =+ 1;
                    break;
            }
        }

        void TurnRight()
        {
            switch (currentDirection)
            {
                case Direction.UP:
                    currentDirection = Direction.RIGHT;
                    break;
                case Direction.DOWN:
                    currentDirection = Direction.LEFT;
                    break;
                case Direction.LEFT:
                    currentDirection = Direction.UP;
                    break;
                case Direction.RIGHT:
                    currentDirection = Direction.DOWN;
                    break;
            }
        }
    }
}
