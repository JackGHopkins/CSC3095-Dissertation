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
        Direction currentDirection, markDirection, mark2Direction;

        bool backtrack, findLoop;

        int? currentPixel, mark, mark2;
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


        //public Stack<Vector2> Painter(Stack<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, ref bool[] pixelCheck)
        //{
        //    this.currentMipPosition = currentMipPosition;
        //    this.textureWidth = textureWidth;
        //    this.textureHeight = textureHeight;
        //    this.colour = colour;
        //    this.textureMip = textureMip;

        //    currentPixel = currentMipPosition;
        //    currentDirection = Direction.RIGHT;

        //    finished = false;

        //    shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));

        //    while (CheckFrontPixel(pixelCheck))
        //        MoveForward(shape);

        //    MainLoop(shape, pixelCheck);
        //    while (!finished)
        //    {
        //        // PAINT:
        //        MoveForward(shape);
        //        Navigate(shape, pixelCheck);
        //    }
        //    return shape;
        //}


        //void MainLoop(Stack<Vector2> shape, bool[] pixelCheck)
        //{
        //    MoveForward(shape);
        //    if (CheckRightPixel(pixelCheck))
        //    {
        //        if (backtrack && !findLoop && CheckFrontPixel(pixelCheck) && CheckLeftPixel(pixelCheck))
        //            findLoop = true;

        //        TurnRight();
        //    }
        //}

        //void Navigate(Stack<Vector2> shape, bool[] pixelCheck)
        //{
        //    CaseFinder(pixelCheck);

        //    if (count != 4)
        //    {
        //        do
        //        {
        //            TurnRight();
        //        } while (CheckFrontPixel());
        //        do
        //        {
        //            TurnLeft();
        //            finished = true;
        //            return;
        //        } while (!CheckFrontPixel());
        //    }

        //    switch (count)
        //    {
        //        case  1:
        //            if (backtrack)
        //                findLoop = true;
        //            else if (findLoop)
        //                if (mark == null)
        //                    mark = currentMipPosition;
        //            else if (CheckFrontLeftPixel(pixelCheck) && CheckBackLeftPixel(pixelCheck))
        //                {
        //                    mark = null;
        //                    return; // go to paint;
        //                }
        //            break;
        //        case 2:
        //            if (!CheckBackPixel(pixelCheck))
        //            {
        //                if (CheckFrontLeftPixel(pixelCheck))
        //                {
        //                    mark = null;
        //                    return; // go to paint;
        //                }
        //            }
        //            else if (mark == null)
        //            {
        //                mark = currentMipPosition;
        //                markDirection = currentDirection;
        //                mark2 = null;
        //                findLoop = false;
        //                backtrack = false;
        //            } 
        //            else
        //            {
        //                if (mark2 == null)
        //                {
        //                    if (currentMipPosition == mark)
        //                    {
        //                        if (currentDirection == markDirection)
        //                        {
        //                            mark = null;
        //                            TurnAround();
        //                            return; // go to paint
        //                        }
        //                        else
        //                        {
        //                            backtrack = true;
        //                            findLoop = false;
        //                            currentDirection = markDirection;
        //                        }
        //                    }
        //                    else if (findLoop)
        //                    {
        //                        mark2 = currentMipPosition;
        //                        mark2Direction = currentDirection;
        //                    }
        //                }
        //                else
        //                {
        //                    if (currentMipPosition == mark)
        //                    {
        //                        currentMipPosition = (int)mark2;
        //                        currentDirection = mark2Direction;
        //                        mark = null;
        //                        mark2 = null;
        //                        backtrack = false;
        //                        TurnAround();
        //                        return; // got to paint
        //                    }
        //                    else if (currentMipPosition == mark2)
        //                    {
        //                        mark = currentMipPosition;
        //                        currentDirection = mark2Direction;
        //                        markDirection = mark2Direction;
        //                        mark2 = null;
        //                    }
        //                }
        //            }
        //            break;
        //        case 3:
        //            mark = null;
        //            return; // jump to paint
        //        case 4:
        //            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        //            finished = true;
        //            break;

        //    }

        //}

        //void MoveForward(Stack<Vector2> shape)
        //{
        //    switch (currentDirection)
        //    {
        //        case Direction.UP:
        //            currentMipPosition += textureWidth;
        //            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        //            break;
        //        case Direction.DOWN:
        //            currentMipPosition -= textureWidth;
        //            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        //            break;
        //        case Direction.LEFT:
        //            currentMipPosition -= 1;
        //            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        //            break;
        //        case Direction.RIGHT:
        //            currentMipPosition += 1;
        //            shape.Push(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
        //            break;
        //    }
        //}

        /*
         * 
         *  QUEUE
         * 
         */

        public Queue<Vector2> Painter(Queue<Vector2> shape, int textureHeight, int textureWidth, Color32 colour, Color32[] textureMip, int currentMipPosition, ref bool[] pixelCheck)
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

            //shape.Enqueue(new Vector2(currentMipPosition % textureWidth, Mathf.Floor(currentMipPosition / textureHeight)));
            //pixelFilled[currentMipPosition] = true;
            //pixelCheck[currentMipPosition] = true;

            //while (CheckFrontPixel())
            //    MoveForward(shape, ref pixelCheck);

            while (!finished)
            {
                if (mainLoop)
                    MainLoop(shape, ref pixelCheck);
                mainLoop = true;
                if (paint)
                    MoveForward(shape, ref pixelCheck);
                Navigate(shape, ref pixelCheck);
            }
            return shape;
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
                        if (mark == null)
                        {
                            mark = currentMipPosition;
                        }
                    }
                    else if (CheckFrontLeftPixel() && CheckBackLeftPixel())/// check this
                    {
                        mark = null;
                        mainLoop = false;
                        paint = true;
                        return; // go to paint;
                    }
                    //else if (!CheckFrontLeftPixel() && !CheckFrontRightPixel() && mark == null)
                    //{
                    //    // if front left and front right pixel are filled then place  mark and set paint to false.
                    //    mark = currentMipPosition;
                    //    mainLoop = false;
                    //    paint = false;
                    //    while((CheckBackPixel() && count == 2) || (Check)
                    //}

                    break;
                case 2:
                    if (!CheckBackPixel())
                    {
                        if (CheckFrontLeftPixel())
                        {
                            mark = null;
                            mainLoop = false;
                            paint = true;
                            return; // go to paint;
                        }
                    }
                    else if (mark == null)
                    {
                        mark = currentMipPosition;
                        markDirection = currentDirection;
                        mark2 = null;
                        findLoop = false;
                        backtrack = false;
                    }
                    else
                    {
                        if (mark2 == null)
                        {
                            if (currentMipPosition == mark)
                            {
                                if (currentDirection == markDirection)
                                {
                                    mark = null;
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
                                mark = null;
                                mark2 = null;
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
                                mark2 = null;
                            }
                        }
                    }
                    break;
                case 3:
                    mark = null;
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
         *      CHECKING PIXELS
         *  
         */

        // Checking if the pixel in Front is empty and inside the shape.
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

        // Check if pixel behind is inside the shape.
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

        // Checking Right Pixel is inside the shape.
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

        // Checking Left Pixel is inside the shape.
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

        // Check if pixel Front-Left is inside the shape.
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

        // Check if pixel back-left is inside the shape.
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
