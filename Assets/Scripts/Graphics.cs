using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/Zalgo2462/VoronoiLib
using VoronoiLib;
using VoronoiLib.Structures;

//Array of pixels with facilitators to use with Unity SetPixels
public class PixelMatrix{

    public Color[] pixels;
    public int width;
    public int height;

    public PixelMatrix(int width, int height){
        pixels = new Color[width*height];
        this.width = width;
        this.height = height;
    }

    public Color GetPixel(int x, int y){
        return pixels[x * width + y];
    }

    public void SetPixel(int x, int y, Color cl){

        if(x < 0 || x >= width || y < 0 || y >= height)
            return;

        pixels[x * width + y] = cl;
    }

}

public static class Graphics
{

    //Draws a line on the bitmap using Bresenham
    public static PixelMatrix Bresenham(PixelMatrix bitmap, int x0, int y0, int x1, int y1, Color color)
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
    public static PixelMatrix FloodFillLine(PixelMatrix bmp, int x, int y, Color replacementColor)
    {

        VPoint pt = new VPoint(x, y);

        Color targetColor = bmp.GetPixel((int)pt.X, (int)pt.Y);
        if (targetColor == replacementColor)
        {
            return bmp;
        }

        Stack<VPoint> pixels = new Stack<VPoint>();

        pixels.Push(pt);
        while (pixels.Count != 0)
        {
            VPoint temp = pixels.Pop();
            int y1 = (int)temp.Y;
            while (y1 >= 0 && bmp.GetPixel((int)temp.X, y1) == targetColor)
            {
                y1--;
            }
            y1++;
            bool spanLeft = false;
            bool spanRight = false;
            while (y1 < bmp.height && bmp.GetPixel((int)temp.X, y1) == targetColor)
            {
                bmp.SetPixel((int)temp.X, y1, replacementColor);

                if (!spanLeft && temp.X > 0 && bmp.GetPixel((int)temp.X - 1, y1) == targetColor)
                {
                    pixels.Push(new VPoint(temp.X - 1, y1));
                    spanLeft = true;
                }
                else if (spanLeft && temp.X - 1 >= 0 && bmp.GetPixel((int)temp.X - 1, y1) != targetColor)
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.X < bmp.width - 1 && bmp.GetPixel((int)temp.X + 1, y1) == targetColor)
                {
                    pixels.Push(new VPoint(temp.X + 1, y1));
                    spanRight = true;
                }
                else if (spanRight && temp.X < bmp.width - 1 && bmp.GetPixel((int)temp.X + 1, y1) != targetColor)
                {
                    spanRight = false;
                }
                y1++;
            }

        }

        return bmp;

    }

}
