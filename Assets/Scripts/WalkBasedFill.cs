using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    enum Direction
    {
        UP, DOWN, LEFT, RIGHT,
    }
    class WalkBasedFill
    {
        Direction currentDirection, markDirection, mark2Direction;

        bool backtrack, findLoop;

        int currentPixel = 0, mark = -1, mark2 = -1;
        int count;

        int currentMipPosition;
        int textureWidth;
        int textureHeight;
        Color32 colour;
        Color32[] textureMip;

        bool finished;
        bool mainLoop = false;
        bool paint = false;
        bool[] pixelFilled;


        public void Painter(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            this.currentMipPosition = currentMipPosition;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.colour = colour;
            this.textureMip = textureMip;

            currentPixel = currentMipPosition;
            currentDirection = Direction.RIGHT;

            finished = false;
            pixelFilled = new bool[pixelCheck.Length];

            while (!finished)
            {
                if (mainLoop)
                    MainLoop(shape, ref pixelCheck);
                mainLoop = true;
                if (paint)
                    MoveForward(shape, ref pixelCheck);
                Navigate(shape, ref pixelCheck);
            }
            return;
        }

        void MainLoop(Stack<Vector2> shape, ref bool[] pixelCheck)
        {
            MoveForward(shape, ref pixelCheck);
            if (CheckRightPixel())
            {
                if (backtrack && !findLoop && (CheckFrontPixel() || CheckLeftPixel()))
                    findLoop = true;

                TurnRight();
                paint = true;
            }
            else
            {
                paint = false;
            }
        }

        void Navigate(Stack<Vector2> shape, ref bool[] pixelCheck)
        {
            CaseFinder();


            if (count != 4)
            {
                do
                {
                    TurnRight();
                } while (CheckFrontPixel());
                do
                {
                    TurnLeft();
                } while (!CheckFrontPixel());
            }

            switch (count)
            {
                case 1:
                    if (backtrack)
                    {
                        findLoop = true;
                    }
                    else if (findLoop)
                    {
                        if (mark == -1)
                        {
                            mark = currentMipPosition;
                        }
                    }
                    else if (CheckFrontLeftPixel() && CheckBackLeftPixel())/// check this
                    {
                        mark = -1;
                        mainLoop = false;
                        paint = true;
                        return; // go to paint;
                    }
                    else if (!CheckFrontLeftPixel() && !CheckFrontRightPixel() && mark == -1)
                    {
                        // if front left and front right pixel are filled then place  mark and set paint to false.
                        mark = currentMipPosition;
                        mainLoop = false;
                        paint = false;
                        do
                        {
                            MoveForward(shape, ref pixelCheck);
                            CaseFinder();

                            if (count != 2)
                            {
                                TurnAround();
                                break;
                            }
                        }
                        while ((CheckFrontPixel() && count == 2));
                    }

                    break;
                case 2:
                    if (!CheckBackPixel())
                    {
                        if (CheckFrontLeftPixel())
                        {
                            mark = -1;
                            mainLoop = false;
                            paint = true;
                            return; // go to paint;
                        }
                        else if (!CheckFrontLeftPixel() && !CheckFrontRightPixel() && mark == -1)
                        {
                            // if front left and front right pixel are filled then place  mark and set paint to false.
                            mark = currentMipPosition;
                            mainLoop = false;
                            paint = false;
                            do
                            {
                                MoveForward(shape, ref pixelCheck);
                                CaseFinder();

                                if (count != 2)
                                {
                                    TurnAround();
                                    break;
                                }
                            }
                            while ((CheckFrontPixel() && count == 2));
                        }
                    }
                    else if (mark == -1)
                    {
                        mark = currentMipPosition;
                        markDirection = currentDirection;
                        mark2 = -1;
                        findLoop = false;
                        backtrack = false;
                    }
                    else
                    {
                        if (mark2 == -1)
                        {
                            if (currentMipPosition == mark)
                            {
                                if (currentDirection == markDirection)
                                {
                                    mark = -1;
                                    TurnAround();
                                    mainLoop = false;
                                    paint = true;
                                    return; // go to paint
                                }
                                else
                                {
                                    backtrack = true;
                                    findLoop = false;
                                    currentDirection = markDirection;
                                }
                            }
                            else if (findLoop)
                            {
                                mark2 = currentMipPosition;
                                mark2Direction = currentDirection;
                            }
                        }
                        else
                        {
                            if (currentMipPosition == mark)
                            {
                                currentMipPosition = (int)mark2;
                                currentDirection = mark2Direction;
                                mark = -1;
                                mark2 = -1;
                                backtrack = false;
                                TurnAround();
                                mainLoop = false;
                                paint = true;
                                return; // got to paint
                            }
                            else if (currentMipPosition == mark2)
                            {
                                mark = currentMipPosition;
                                currentDirection = mark2Direction;
                                markDirection = mark2Direction;
                                mark2 = -1;
                            }
                        }
                    }
                    break;
                case 3:
                    mark = -1;
                    mainLoop = false;
                    paint = true;
                    return; // jump to paint
                case 4:
                    shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                    pixelFilled[currentMipPosition] = true;
                    pixelCheck[currentMipPosition] = true;
                    finished = true; // Break Main Loop
                    break;
            }
        }

        void MoveForward(Stack<Vector2> shape, ref bool[] pixelCheck)
        {
            if (paint)
            {
                shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                pixelFilled[currentMipPosition] = true;
                pixelCheck[currentMipPosition] = true;
            }

            switch (currentDirection)
            {
                case Direction.UP:
                    currentMipPosition += textureWidth;
                    break;
                case Direction.DOWN:
                    currentMipPosition -= textureWidth;
                    break;
                case Direction.LEFT:
                    currentMipPosition -= 1;
                    break;
                case Direction.RIGHT:
                    currentMipPosition += 1;
                    break;
            }
        }

        /*
         * 
         *  QUEUE
         * 
         */

        public void Painter(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, bool[] pixelCheck)
        {
            this.currentMipPosition = currentMipPosition;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.colour = colour;
            this.textureMip = textureMip;

            currentPixel = currentMipPosition;
            currentDirection = Direction.RIGHT;

            finished = false;
            pixelFilled = new bool[pixelCheck.Length];

            while (!finished)
            {
                if (mainLoop)
                    MainLoop(shape, ref pixelCheck);
                mainLoop = true;
                if (paint)
                    MoveForward(shape, ref pixelCheck);
                Navigate(shape, ref pixelCheck);
            }
            return;
        }

        void MainLoop(Queue<Vector2> shape, ref bool[] pixelCheck)
        {
            MoveForward(shape, ref pixelCheck);
            if (CheckRightPixel())
            {
                if (backtrack && !findLoop && (CheckFrontPixel() || CheckLeftPixel()))
                    findLoop = true;

                TurnRight();
                paint = true;
            } 
            else
            {
                paint = false;
            }
        }

        void Navigate(Queue<Vector2> shape, ref bool[] pixelCheck)
        {
            CaseFinder();


            if (count != 4)
            {
                do
                {
                    TurnRight();
                } while (CheckFrontPixel());
                do
                {
                    TurnLeft();
                } while (!CheckFrontPixel());
            }

            switch (count)
            {
                case 1:
                    if (backtrack)
                    {
                        findLoop = true;
                    }
                    else if (findLoop)
                    {
                        if (mark == -1)
                        {
                            mark = currentMipPosition;
                        }
                    }
                    else if (CheckFrontLeftPixel() && CheckBackLeftPixel())/// check this
                    {
                        mark = -1;
                        mainLoop = false;
                        paint = true;
                        return; // go to paint;
                    }
                    else if (!CheckFrontLeftPixel() && !CheckFrontRightPixel() && mark == -1)
                    {
                        // if front left and front right pixel are filled then place  mark and set paint to false.
                        mark = currentMipPosition;
                        mainLoop = false;
                        paint = false;
                        do
                        {
                            MoveForward(shape, ref pixelCheck);
                            CaseFinder();

                            if (count != 2)
                            {
                                TurnAround();
                                break;
                            }
                        }
                        while ((CheckFrontPixel() && count == 2));
                    }

                    break;
                case 2:
                    if (!CheckBackPixel())
                    {
                        if (CheckFrontLeftPixel())
                        {
                            mark = -1;
                            mainLoop = false;
                            paint = true;
                            return; // go to paint;
                        }
                        else if (!CheckFrontLeftPixel() && !CheckFrontRightPixel() && mark == -1)
                        {
                            // if front left and front right pixel are filled then place  mark and set paint to false.
                            mark = currentMipPosition;
                            mainLoop = false;
                            paint = false;
                            do
                            {
                                MoveForward(shape, ref pixelCheck);
                                CaseFinder();

                                if (count != 2)
                                {
                                    TurnAround();
                                    break;
                                }
                            }
                            while ((CheckFrontPixel() && count == 2));
                        }
                    }
                    else if (mark == -1)
                    {
                        mark = currentMipPosition;
                        markDirection = currentDirection;
                        mark2 = -1;
                        findLoop = false;
                        backtrack = false;
                    }
                    else
                    {
                        if (mark2 == -1)
                        {
                            if (currentMipPosition == mark)
                            {
                                if (currentDirection == markDirection)
                                {
                                    mark = -1;
                                    TurnAround();
                                    mainLoop = false;
                                    paint = true;
                                    return; // go to paint
                                }
                                else
                                {
                                    backtrack = true;
                                    findLoop = false;
                                    currentDirection = markDirection;
                                }
                            }
                            else if (findLoop)
                            {
                                mark2 = currentMipPosition;
                                mark2Direction = currentDirection;
                            }
                        }
                        else
                        {
                            if (currentMipPosition == mark)
                            {
                                currentMipPosition = (int)mark2;
                                currentDirection = mark2Direction;
                                mark = -1;
                                mark2 = -1;
                                backtrack = false;
                                TurnAround();
                                mainLoop = false;
                                paint = true;
                                return; // got to paint
                            }
                            else if (currentMipPosition == mark2)
                            {
                                mark = currentMipPosition;
                                currentDirection = mark2Direction;
                                markDirection = mark2Direction;
                                mark2 = -1;
                            }
                        }
                    }
                    break;
                case 3:
                    mark = -1;
                    mainLoop = false;
                    paint = true;
                    return; // jump to paint
                case 4:
                    shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                    pixelFilled[currentMipPosition] = true;
                    pixelCheck[currentMipPosition] = true;
                    finished = true; // Break Main Loop
                    break;
            }
        }

        void MoveForward(Queue<Vector2> shape, ref bool[] pixelCheck)
        {
            if (paint)
            {
                shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
                pixelFilled[currentMipPosition] = true;
                pixelCheck[currentMipPosition] = true;
            }

            switch (currentDirection)
            {     
                case Direction.UP:
                    currentMipPosition += textureWidth;
                    break;
                case Direction.DOWN:
                    currentMipPosition -= textureWidth;
                    break;
                case Direction.LEFT:
                    currentMipPosition -= 1;
                    break;
                case Direction.RIGHT:
                    currentMipPosition += 1;
                    break;
            }
        }

        /*
         * 
         *      TURNING CURRENT DIRECTION
         * 
         */
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

        void TurnLeft()
        {
            switch (currentDirection)
            {
                case Direction.UP:
                    currentDirection = Direction.LEFT;
                    break;
                case Direction.DOWN:
                    currentDirection = Direction.RIGHT;
                    break;
                case Direction.LEFT:
                    currentDirection = Direction.DOWN;
                    break;
                case Direction.RIGHT:
                    currentDirection = Direction.UP;
                    break;
            }
        }

        void TurnAround()
        {
            switch (currentDirection)
            {
                case Direction.UP:
                    currentDirection = Direction.DOWN;
                    break;
                case Direction.DOWN:
                    currentDirection = Direction.UP;
                    break;
                case Direction.LEFT:
                    currentDirection = Direction.RIGHT;
                    break;
                case Direction.RIGHT:
                    currentDirection = Direction.LEFT;
                    break;
            }
        }

        /*
         *  
         *      CHECKING PIXELS (Directions are relative to currentDirection)
         *  
         */

        bool CheckFrontPixel()
        {
            if (currentDirection == Direction.UP && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.DOWN && currentMipPosition >= textureWidth)
                if (textureMip[currentMipPosition - textureWidth].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition + 1].Equals(colour) && !pixelFilled[currentMipPosition + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition - 1].Equals(colour) && !pixelFilled[currentMipPosition - 1])
                    return true;
                else
                    return false;

            return false;

        }

        bool CheckBackPixel()
        {
            if (currentDirection == Direction.DOWN && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.UP && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition + 1].Equals(colour) && !pixelFilled[currentMipPosition + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition - 1].Equals(colour) && !pixelFilled[currentMipPosition - 1])
                    return true;
                else
                    return false;

            return false;

        }

        bool CheckRightPixel()
        {
            if (currentDirection == Direction.LEFT && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.UP && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition + 1].Equals(colour) && !pixelFilled[currentMipPosition + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.DOWN && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition - 1].Equals(colour) && !pixelFilled[currentMipPosition - 1])
                    return true;
                else
                    return false;

            return false;
        }

        bool CheckLeftPixel()
        {
            if (currentDirection == Direction.RIGHT && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.DOWN && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition + 1].Equals(colour) && !pixelFilled[currentMipPosition + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.UP && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition - 1].Equals(colour) && !pixelFilled[currentMipPosition - 1])
                    return true;
                else
                    return false;

            return false;
        }

        bool CheckFrontLeftPixel()
        {
            if (currentDirection == Direction.UP && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth - 1].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth - 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.DOWN && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth + 1].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition + textureWidth + 1].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition - textureWidth - 1].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth - 1])
                    return true;
                else
                    return false;

            return false;
        }

        bool CheckFrontRightPixel()
        {
            if (currentDirection == Direction.UP && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth + 1].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.DOWN && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth - 1].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth - 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition - textureWidth + 1].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition + textureWidth - 1].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth - 1])
                    return true;
                else
                    return false;

            return false;
        }

        bool CheckBackLeftPixel()
        {
            if (currentDirection == Direction.DOWN && currentMipPosition < textureMip.Length - textureWidth)
                if (textureMip[currentMipPosition + textureWidth + 1].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth + 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.UP && currentMipPosition > textureWidth)
                if (textureMip[currentMipPosition - textureWidth - 1].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth - 1])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.LEFT && currentMipPosition % textureWidth != textureWidth - 1)
                if (textureMip[currentMipPosition - textureWidth + 1 ].Equals(colour) && !pixelFilled[currentMipPosition - textureWidth + 1 ])
                    return true;
                else
                    return false;

            else if (currentDirection == Direction.RIGHT && currentMipPosition % textureWidth != 0)
                if (textureMip[currentMipPosition + textureWidth - 1 ].Equals(colour) && !pixelFilled[currentMipPosition + textureWidth - 1 ])
                    return true;
                else
                    return false;

            return false;

        }


        // Finds the number of surrounding pixels NOT available.
        void CaseFinder()
        {
            count = 0;
            // Check north pixel
            if (currentMipPosition < textureMip.Length - textureWidth)
                if (!(textureMip[currentMipPosition + textureWidth].Equals(colour)) || pixelFilled[currentMipPosition + textureWidth])
                    count++;
            // Check south pixel
            if (currentMipPosition > textureWidth)
                if (!(textureMip[currentMipPosition - textureWidth].Equals(colour)) || pixelFilled[currentMipPosition - textureWidth])
                    count++;
            // Check east pixel
            if (currentMipPosition < textureMip.Length)
                if (!(textureMip[currentMipPosition + 1].Equals(colour)) || pixelFilled[currentMipPosition + 1])
                    count++;
            // Check west pixel
            if (currentMipPosition > 0)
                if (!(textureMip[currentMipPosition - 1].Equals(colour)) || pixelFilled[currentMipPosition - 1])
                    count++;
        }
    }
}
