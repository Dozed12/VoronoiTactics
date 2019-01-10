using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Graphics
{

    //Useful point struct
    private struct Point
    {
        public short x;
        public short y;
        public Point(short aX, short aY) { x = aX; y = aY; }
        public Point(int aX, int aY) : this((short)aX, (short)aY) { }
    }

    //Draws a line on the bitmap using Bresenham
    public static Texture2D Bresenham(Texture2D bitmap, int x0, int y0, int x1, int y1, Color color)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;
        for (; ; )
        {
            bitmap.SetPixel(x0, y0, color);
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
        return bitmap;
    }

    //Draws a border on the bitmap
    public static Texture2D Border(Texture2D bitmap, Color color)
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
    public static Texture2D FloodFill4(Texture2D aTex, int aX, int aY, Color aFillColor)
    {
        int w = aTex.width;
        int h = aTex.height;
        Color[] colors = aTex.GetPixels();
        Color refCol = colors[aX + aY * w];
        Queue<Point> nodes = new Queue<Point>();
        nodes.Enqueue(new Point(aX, aY));
        while (nodes.Count > 0)
        {
            Point current = nodes.Dequeue();
            for (int i = current.x; i < w; i++)
            {
                Color C = colors[i + current.y * w];
                if (C != refCol || C == aFillColor)
                    break;
                colors[i + current.y * w] = aFillColor;
                if (current.y + 1 < h)
                {
                    C = colors[i + current.y * w + w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if (current.y - 1 >= 0)
                {
                    C = colors[i + current.y * w - w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
            for (int i = current.x - 1; i >= 0; i--)
            {
                Color C = colors[i + current.y * w];
                if (C != refCol || C == aFillColor)
                    break;
                colors[i + current.y * w] = aFillColor;
                if (current.y + 1 < h)
                {
                    C = colors[i + current.y * w + w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y + 1));
                }
                if (current.y - 1 >= 0)
                {
                    C = colors[i + current.y * w - w];
                    if (C == refCol && C != aFillColor)
                        nodes.Enqueue(new Point(i, current.y - 1));
                }
            }
        }
        aTex.SetPixels(colors);
        return aTex;
    }

    //Scan-line flood fill, much faster
    public static Texture2D FloodFillLine(Texture2D bmp, int x, int y, Color replacementColor)
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

                if (!spanLeft && temp.x > 0 && bmp.GetPixel(temp.x - 1, y1) == targetColor)
                {
                    pixels.Push(new Point(temp.x - 1, y1));
                    spanLeft = true;
                }
                else if (spanLeft && temp.x - 1 >= 0 && bmp.GetPixel(temp.x - 1, y1) != targetColor)
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.x < bmp.width - 1 && bmp.GetPixel(temp.x + 1, y1) == targetColor)
                {
                    pixels.Push(new Point(temp.x + 1, y1));
                    spanRight = true;
                }
                else if (spanRight && temp.x < bmp.width - 1 && bmp.GetPixel(temp.x + 1, y1) != targetColor)
                {
                    spanRight = false;
                }
                y1++;
            }

        }

        return bmp;

    }

}
