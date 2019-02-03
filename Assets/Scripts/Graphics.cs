using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

public static class Graphics
{

    //Basic integer based point
    public struct Point
    {
        public int x;
        public int y;
        public Point(int nx, int ny)
        {
            x = nx;
            y = ny;
        }
    }

    //Array of pixels with facilitators to use with Unity SetPixels
    public class PixelMatrix
    {

        public Color[] pixels;
        public int width;
        public int height;

        public PixelMatrix(Texture2D texture)
        {

            this.pixels = texture.GetPixels();
            this.width = texture.width;
            this.height = texture.height;

        }

        public PixelMatrix(int width, int height, Color clear)
        {

            //Initialize
            pixels = new Color[width * height];
            this.width = width;
            this.height = height;

            //Clear with color
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = clear;
            }

        }

        public Color GetPixel(int x, int y)
        {

            if (x < 0)
                x = 0;
            else if (x > width - 1)
                x = width - 1;

            if (y < 0)
                y = 0;
            else if (y > height - 1)
                y = height - 1;

            return pixels[x * width + y];
        }

        public void SetPixel(int x, int y, Color cl)
        {

            //Check if outside
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;

            pixels[x * width + y] = cl;

        }

        public void SetPixel(int x, int y, Color cl, bool safe)
        {

            //Use safe=true if we know for sure there wont be out of bounds
            if (!safe)
            {
                //Check if outside
                if (x < 0 || x >= width || y < 0 || y >= height)
                    return;
            }

            pixels[x * width + y] = cl;

        }

    }

    //Draw a filled box
    public static PixelMatrix FilledBox(int width, int height, Color color)
    {

        //New Decal
        PixelMatrix decal = new PixelMatrix(width, height, new Color(0, 0, 0, 0));

        //Box perimeter
        decal = Border(decal, color);

        //Fill box
        decal = FloodFillLine(decal, width / 2, height / 2, color);

        return decal;

    }

    //Draw a filled circle
    public static PixelMatrix FilledCircle(int radius, Color color)
    {

        //New Decal
        int size = radius * 2 + 1;
        PixelMatrix decal = new PixelMatrix(size, size, new Color(0, 0, 0, 0));

        //Circle perimeter
        decal = BresenhamCircle(decal, radius, radius, radius, color);

        //Fill circle
        decal = FloodFillLine(decal, radius, radius, color);

        return decal;

    }

    //Draws a line on the bitmap using Bresenham
    public static PixelMatrix BresenhamLine(PixelMatrix bitmap, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;

        do
        {
            bitmap.SetPixel(x0, y0, color);
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dy)
            {
                err += dx;
                y0 += sy;
            }
        } while (true);

        return bitmap;
    }

    //Draws a line on the bitmap using Bresenham and thickness
    public static PixelMatrix BresenhamLineThick(PixelMatrix bitmap, int x0, int y0, int x1, int y1, Color color, int thickness)
    {

        //Prepare filled circle
        PixelMatrix decal = FilledCircle(thickness, color);

        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;

        do
        {
            bitmap = Decal(bitmap, decal, x0, y0);
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        } while (true);

        return bitmap;
    }

    //Draws a circle on the bitmap using Bresenham
    public static PixelMatrix BresenhamCircle(PixelMatrix bitmap, int centerX, int centerY, int radius, Color color)
    {
        int d = (5 - radius * 4) / 4;
        int x = 0;
        int y = radius;

        do
        {
            bitmap.SetPixel(centerX + x, centerY + y, color);
            bitmap.SetPixel(centerX + x, centerY - y, color);
            bitmap.SetPixel(centerX - x, centerY + y, color);
            bitmap.SetPixel(centerX - x, centerY - y, color);
            bitmap.SetPixel(centerX + y, centerY + x, color);
            bitmap.SetPixel(centerX + y, centerY - x, color);
            bitmap.SetPixel(centerX - y, centerY + x, color);
            bitmap.SetPixel(centerX - y, centerY - x, color);
            if (d < 0)
            {
                d += 2 * x + 1;
            }
            else
            {
                d += 2 * (x - y) + 1;
                y--;
            }
            x++;
        } while (x <= y);

        return bitmap;

    }

    //Draws a border on the bitmap
    public static PixelMatrix Border(PixelMatrix bitmap, Color color)
    {
        for (int i = 0; i < bitmap.height; i++)
        {
            bitmap.SetPixel(0, i, color);
            bitmap.SetPixel(bitmap.width - 1, i, color);
        }
        for (int i = 0; i < bitmap.width; i++)
        {
            bitmap.SetPixel(i, 0, color);
            bitmap.SetPixel(i, bitmap.height - 1, color);
        }
        return bitmap;
    }

    //4 Direction flood fill, slower
    public static PixelMatrix FloodFill4(PixelMatrix aTex, int aX, int aY, Color aFillColor)
    {
        int w = aTex.width;
        int h = aTex.height;
        Color[] colors = aTex.pixels;
        Color refCol = colors[aX + aY * w];
        Queue<VPoint> nodes = new Queue<VPoint>();
        nodes.Enqueue(new VPoint(aX, aY));
        while (nodes.Count > 0)
        {
            VPoint current = nodes.Dequeue();
            for (int i = (int)current.X; i < w; i++)
            {
                Color C = colors[i + (int)current.Y * w];
                if (C != refCol || C == aFillColor)
                    break;
                colors[i + (int)current.Y * w] = aFillColor;
                if (current.Y + 1 < h)
                {
                    C = colors[i + (int)current.Y * w + w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new VPoint(i, current.Y + 1));
                }
                if (current.Y - 1 >= 0)
                {
                    C = colors[i + (int)current.Y * w - w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new VPoint(i, current.Y - 1));
                }
            }
            for (int i = (int)current.X - 1; i >= 0; i--)
            {
                Color C = colors[i + (int)current.Y * w];
                if (C != refCol || C == aFillColor)
                    break;
                colors[i + (int)current.Y * w] = aFillColor;
                if (current.Y + 1 < h)
                {
                    C = colors[i + (int)current.Y * w + w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new VPoint(i, current.Y + 1));
                }
                if (current.Y - 1 >= 0)
                {
                    C = colors[i + (int)current.Y * w - w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new VPoint(i, current.Y - 1));
                }
            }
        }
        aTex.pixels = colors;
        return aTex;
    }

    //Scan-line flood fill, much faster
    //https://lodev.org/cgtutor/floodfill.html#Scanline_Floodfill_Algorithm_With_Stack
    public static PixelMatrix FloodFillLine(PixelMatrix bmp, int x, int y, Color replacementColor)
    {

        Point pt = new Point(x, y);

        Color targetColor = bmp.GetPixel(pt.x, pt.y);
        if (targetColor == replacementColor)
        {
            return bmp;
        }

        Stack<Point> pixels = new Stack<Point>();

        pixels.Push(pt);
        while (pixels.Count != 0)
        {
            Point temp = pixels.Pop();
            int y1 = temp.y;

            while (y1 >= 0 && bmp.GetPixel(temp.x, y1) == targetColor)
            {
                y1--;
            }
            y1++;
            bool spanLeft = false;
            bool spanRight = false;

            while (y1 < bmp.height && bmp.GetPixel(temp.x, y1) == targetColor)
            {
                bmp.SetPixel(temp.x, y1, replacementColor);

                Color clm1 = bmp.GetPixel(temp.x - 1, y1);
                Color clp1 = bmp.GetPixel(temp.x + 1, y1);

                if (!spanLeft && temp.x > 0 && clm1 == targetColor)
                {
                    pixels.Push(new Point(temp.x - 1, y1));
                    spanLeft = true;
                }
                else if (spanLeft && temp.x - 1 >= 0 && clm1 != targetColor)
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.x < bmp.width - 1 && clp1 == targetColor)
                {
                    pixels.Push(new Point(temp.x + 1, y1));
                    spanRight = true;
                }
                else if (spanRight && temp.x < bmp.width - 1 && clp1 != targetColor)
                {
                    spanRight = false;
                }
                y1++;
            }

        }

        return bmp;

    }

    //Fill Polygon
    //Looks much faster and quite promissing
    //http://alienryderflex.com/polygon_fill/
    public static PixelMatrix FillPolygon(PixelMatrix original, List<Geometry.Vector2X> polygon, Color cl)
    {

        int pixelX, pixelY, swap;
        int i, j, nodes;
        int[] nodeX = new int[polygon.Count];

        //Limits of polygon
        int IMAGE_TOP = original.height + 1, IMAGE_BOT = -1, IMAGE_RIGHT = -1, IMAGE_LEFT = original.width + 1;

        for (int p = 0; p < polygon.Count; p++)
        {
            if (polygon[p].value.x > IMAGE_RIGHT)
                IMAGE_RIGHT = (int)polygon[p].value.x;
            if (polygon[p].value.x < IMAGE_LEFT)
                IMAGE_LEFT = (int)polygon[p].value.x;
            if (polygon[p].value.y < IMAGE_TOP)
                IMAGE_TOP = (int)polygon[p].value.y;
            if (polygon[p].value.y > IMAGE_BOT)
                IMAGE_BOT = (int)polygon[p].value.y;
        }

        //  Loop through the rows of the image.
        for (pixelY = IMAGE_TOP; pixelY < IMAGE_BOT; pixelY++)
        {

            //  Build a list of nodes.
            nodes = 0; j = polygon.Count - 1;
            for (i = 0; i < polygon.Count; i++)
            {
                if (polygon[i].value.y < (float)pixelY && polygon[j].value.y >= (float)pixelY || polygon[j].value.y < (float)pixelY && polygon[i].value.y >= (float)pixelY)
                {
                    nodeX[nodes++] = (int)(polygon[i].value.x + (pixelY - polygon[i].value.y) / (polygon[j].value.y - polygon[i].value.y) * (polygon[j].value.x - polygon[i].value.x));
                }
                j = i;
            }

            //  Sort the nodes, via a simple “Bubble” sort.
            i = 0;
            while (i < nodes - 1)
            {
                if (nodeX[i] > nodeX[i + 1])
                {
                    swap = nodeX[i]; nodeX[i] = nodeX[i + 1]; nodeX[i + 1] = swap; if (i != 0) i--;
                }
                else
                {
                    i++;
                }
            }

            //  Fill the pixels between node pairs.
            for (i = 0; i < nodes; i += 2)
            {
                if (nodeX[i] >= IMAGE_RIGHT)
                    break;
                if (nodeX[i + 1] > IMAGE_LEFT)
                {
                    if (nodeX[i] < IMAGE_LEFT)
                        nodeX[i] = IMAGE_LEFT;
                    if (nodeX[i + 1] > IMAGE_RIGHT)
                        nodeX[i + 1] = IMAGE_RIGHT;
                    for (pixelX = nodeX[i]; pixelX < nodeX[i + 1]; pixelX++)
                        original.SetPixel((int)pixelX, (int)pixelY, cl);
                }
            }
        }

        return original;
    }

    //Rotate an image by increments of 90 degrees
    public static PixelMatrix RotateSq(PixelMatrix original, int rotates)
    {

        //No more rotates
        if (rotates == 0)
            return original;

        //Setup new rotated
        int side = Mathf.Max(original.width, original.height);
        PixelMatrix rotated = new PixelMatrix(side, side, Color.clear);

        //Rotate 90º
        for (int i = 0; i < original.width; i++)
        {
            int finalI = original.width - i - 1;
            for (int j = 0; j < original.height; j++)
            {
                rotated.SetPixel(j, finalI, original.GetPixel(i, j));
            }
        }

        //Call new rotation
        rotated = RotateSq(rotated, rotates - 1);

        return rotated;
    }

    //Add a decal to an image
    public static PixelMatrix Decal(PixelMatrix original, PixelMatrix decal, int x, int y)
    {

        //Offsets to apply
        int widthOffset = -decal.width / 2 + x;
        int heightOffset = -decal.height / 2 + y;

        for (int i = 0; i < decal.width; i++)
        {

            //X offset
            int finalX = i + widthOffset;

            //Skip column if out of original
            if (finalX < 0 || finalX > original.width - 1)
                continue;

            for (int j = 0; j < decal.height; j++)
            {

                //Get color
                Color cl = decal.GetPixel(i, j);

                //Skip transparent pixels
                //TODO Blend with transparency?
                if (cl.a < 0.7)
                    continue;

                //Y offset                
                int finalY = j + heightOffset;

                //Apply color
                original.SetPixel(finalX, finalY, cl);

            }
        }

        return original;
    }

}
