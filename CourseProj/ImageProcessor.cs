using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CourseProj
{
    
    class ImageProcessor
    {
        public const int TRESHOLD = 500;
        public const int INFINITY = 10000000;
        public const int DIMENSION_COUNT = 10;
        public const double INNERTION = 0.5;
        public const double GBEST_INFLUENCE = 1.4;
        public const double PBEST_INFLUENCE = 0.6;

        class Particle
        {
          public  ImageProcessor parrent;
          public  int parentX, parentY;
          public  int[] dimensions;
          public  int dimensionsCount;
           
            public Particle(Particle e)
            {
                this.parrent = e.parrent;
                this.parentX = e.parentX;
                this.parentY = e.parentY;
                this.dimensions = e.dimensions;
                this.dimensionsCount = e.dimensionsCount;
            }
            
            public Particle(int dimensionsCount, int parentX, int parentY, ImageProcessor parrent)
            {
                this.dimensionsCount = dimensionsCount;
                dimensions = new int[dimensionsCount];
                this.parentX = parentX;
                this.parentY = parentY;
                this.parrent = parrent;
                for(int i = 0; i < dimensionsCount; i++)
                {
                    Random r = new Random();
                    dimensions[i] = r.Next(9);
                    if(dimensions[i] == 0)
                    {
                        for(int j = i; j < dimensionsCount; j++)
                        {
                            dimensions[j] = 0;
                        }

                        break;
                    }
                }
            }

            public double Homogeneity()
            {
                 int curX = parentX, curY = parentY;
                double res = 0f;
                for (int i = 0; i < dimensionsCount; i++)
                {
                    if (dimensions[i] == 0) break;
                    if (dimensions[i] == 1)
                    {
                        curY--;
                    }
                    if (dimensions[i] == 2)
                    {
                        curY--;
                        curX++;
                    }
                    if (dimensions[i] == 3)
                    {
                        curX++;
                    }
                    if (dimensions[i] == 4)
                    {
                        curY++;
                        curX++;
                    }
                    if (dimensions[i] == 5)
                    {
                        curY++;
                    }
                    if (dimensions[i] == 6)
                    {
                        curY++;
                        curX--;
                    }
                    if (dimensions[i] == 7)
                    {
                        curX--;
                    }
                    if (dimensions[i] == 8)
                    {
                        curY--;
                        curX--;
                    }
                    curX = Math.Max(curX, 0);
                    curY = Math.Max(curY, 0);
                    curX = Math.Min(curX, parrent.image.PixelWidth - 1);
                    curY = Math.Min(curY, parrent.image.PixelHeight - 1);
                    res += parrent.PixelHomogeneity(curX, curY);
                }

                res /= Length();
                return res;
            }

            public double Uniformity()
            {
                int curX = parentX, curY = parentY;
                double res = 0f, intense = parrent.PixelIntense(parentX, parentY);
                for (int i = 0; i < dimensionsCount; i++)
                {
                    if (dimensions[i] == 0) break;
                    if (dimensions[i] == 1)
                    {
                        curY--;
                    }
                    if (dimensions[i] == 2)
                    {
                        curY--;
                        curX++;
                    }
                    if (dimensions[i] == 3)
                    {
                        curX++;
                    }
                    if (dimensions[i] == 4)
                    {
                        curY++;
                        curX++;
                    }
                    if (dimensions[i] == 5)
                    {
                        curY++;
                    }
                    if (dimensions[i] == 6)
                    {
                        curY++;
                        curX--;
                    }
                    if (dimensions[i] == 7)
                    {
                        curX--;
                    }
                    if (dimensions[i] == 8)
                    {
                        curY--;
                        curX--;
                    }
                    curX = Math.Max(curX, 0);
                    curY = Math.Max(curY, 0);
                    curX = Math.Min(curX, parrent.image.PixelWidth - 1);
                    curY = Math.Min(curY, parrent.image.PixelHeight - 1);
                    res += Math.Abs(parrent.PixelIntense(curX, curY) - intense);
                }

                res /= Length();
                return res;
            }

            public double Length()
            {
                double res = 0;

                for (int i = 0; i < dimensionsCount; i++)
                {
                    if (dimensions[i] == 0) return res;
                    if (dimensions[i] % 2 == 0)
                    {
                        res += 1.414f;
                    }
                    else
                    {
                        res++;
                    }
                }
                return res;
            }
        }

        public BitmapSource image { get; set; }
        public int pixelSize = 4;
        public  int stride;
        public int size;
        private bool[,] marked;
        public byte[] pixels;
        private bool[,] kennyRes;
        public double accuracy;
        public float w;
        public float c1;
        public float c2;
        public float treshold;
        public float time;
        //public int TRESHOLD;

        public WriteableBitmap writableImage { get; set;}
        public ImageProcessor(string imagePath)
        {
            image = new BitmapImage(new Uri(imagePath));
            writableImage = new WriteableBitmap(image);
            stride = image.PixelWidth * pixelSize;
            size = image.PixelHeight * stride;
            marked = new bool[image.PixelHeight, image.PixelWidth];
            pixels = GetPixelData();
            Random r = new Random();
            w = (float)r.NextDouble();
            c1 = (float)r.NextDouble();
            c2 = 2.0f - c1;
            //TRESHOLD = trh;
            treshold = (float)r.NextDouble();
        }

        public void ToGreyscale()
        {
            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();

            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = image;

            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
            newFormatedBitmapSource.EndInit();

            image = (BitmapSource)newFormatedBitmapSource;
        }
       
        public byte[] GetPixelData()
        {
            
            byte[] pixels = new byte[size];
            image.CopyPixels(pixels, image.PixelWidth * pixelSize, 0);
            return pixels;
        }

        public int Index(int i, int j)
        {
            return j * stride + i * pixelSize;
        }
        public void Kenny()
        {
           
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int[] tmPyxel = new int[pixels.Length];
          

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = pixels[Index(i, j) + 2];
                    allPixG[i, j] = pixels[Index(i, j) + 1];
                    allPixB[i, j] = pixels[Index(i, j)];
                }
            }
            for (int i = 2; i < width - 2; i++)
            {
                for (int j = 2; j < height - 2; j++)
                {
                    int red = (
                              ((allPixR[i - 2, j - 2]) * 1 + (allPixR[i - 1, j - 2]) * 4 + (allPixR[i, j - 2]) * 7 + (allPixR[i + 1, j - 2]) * 4 + (allPixR[i + 2, j - 2])
                              + (allPixR[i - 2, j - 1]) * 4 + (allPixR[i - 1, j - 1]) * 16 + (allPixR[i, j - 1]) * 26 + (allPixR[i + 1, j - 1]) * 16 + (allPixR[i + 2, j - 1]) * 4
                              + (allPixR[i - 2, j]) * 7 + (allPixR[i - 1, j]) * 26 + (allPixR[i, j]) * 41 + (allPixR[i + 1, j]) * 26 + (allPixR[i + 2, j]) * 7
                              + (allPixR[i - 2, j + 1]) * 4 + (allPixR[i - 1, j + 1]) * 16 + (allPixR[i, j + 1]) * 26 + (allPixR[i + 1, j + 1]) * 16 + (allPixR[i + 2, j + 1]) * 4
                              + (allPixR[i - 2, j + 2]) * 1 + (allPixR[i - 1, j + 2]) * 4 + (allPixR[i, j + 2]) * 7 + (allPixR[i + 1, j + 2]) * 4 + (allPixR[i + 2, j + 2]) * 1) / 273
                              );

                    int green = (
                              ((allPixG[i - 2, j - 2]) * 1 + (allPixG[i - 1, j - 2]) * 4 + (allPixG[i, j - 2]) * 7 + (allPixG[i + 1, j - 2]) * 4 + (allPixG[i + 2, j - 2])
                              + (allPixG[i - 2, j - 1]) * 4 + (allPixG[i - 1, j - 1]) * 16 + (allPixG[i, j - 1]) * 26 + (allPixG[i + 1, j - 1]) * 16 + (allPixG[i + 2, j - 1]) * 4
                              + (allPixG[i - 2, j]) * 7 + (allPixG[i - 1, j]) * 26 + (allPixG[i, j]) * 41 + (allPixG[i + 1, j]) * 26 + (allPixG[i + 2, j]) * 7
                              + (allPixG[i - 2, j + 1]) * 4 + (allPixG[i - 1, j + 1]) * 16 + (allPixG[i, j + 1]) * 26 + (allPixG[i + 1, j + 1]) * 16 + (allPixG[i + 2, j + 1]) * 4
                              + (allPixG[i - 2, j + 2]) * 1 + (allPixG[i - 1, j + 2]) * 4 + (allPixG[i, j + 2]) * 7 + (allPixG[i + 1, j + 2]) * 4 + (allPixG[i + 2, j + 2]) * 1) / 273
                              );

                    int blue = (
                              ((allPixB[i - 2, j - 2]) * 1 + (allPixB[i - 1, j - 2]) * 4 + (allPixB[i, j - 2]) * 7 + (allPixB[i + 1, j - 2]) * 4 + (allPixB[i + 2, j - 2])
                              + (allPixB[i - 2, j - 1]) * 4 + (allPixB[i - 1, j - 1]) * 16 + (allPixB[i, j - 1]) * 26 + (allPixB[i + 1, j - 1]) * 16 + (allPixB[i + 2, j - 1]) * 4
                              + (allPixB[i - 2, j]) * 7 + (allPixB[i - 1, j]) * 26 + (allPixB[i, j]) * 41 + (allPixB[i + 1, j]) * 26 + (allPixB[i + 2, j]) * 7
                              + (allPixB[i - 2, j + 1]) * 4 + (allPixB[i - 1, j + 1]) * 16 + (allPixB[i, j + 1]) * 26 + (allPixB[i + 1, j + 1]) * 16 + (allPixB[i + 2, j + 1]) * 4
                              + (allPixB[i - 2, j + 2]) * 1 + (allPixB[i - 1, j + 2]) * 4 + (allPixB[i, j + 2]) * 7 + (allPixB[i + 1, j + 2]) * 4 + (allPixB[i + 2, j + 2]) * 1) / 273
                              );
                    tmPyxel[Index(i, j)] = blue;
                    tmPyxel[Index(i, j) + 1] = green;
                    tmPyxel[Index(i, j) + 2] = red;
                }
            }
            
            int[,] allPixRn = new int[width, height];
            int[,] allPixGn = new int[width, height];
            int[,] allPixBn = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixRn[i, j] = tmPyxel[Index(i, j) + 2];
                    allPixGn[i, j] = tmPyxel[Index(i, j) + 1];
                    allPixBn[i, j] = tmPyxel[Index(i, j)];
                }
            }


            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };
            int new_rx = 0, new_ry = 0;
            int new_gx = 0, new_gy = 0;
            int new_bx = 0, new_by = 0;
            int rc, gc, bc;
            int gradR, gradG, gradB;

            int[,] graidientR = new int[width, height];
            int[,] graidientG = new int[width, height];
            int[,] graidientB = new int[width, height];

            int atanR, atanG, atanB;

            int[,] tanR = new int[width, height];
            int[,] tanG = new int[width, height];
            int[,] tanB = new int[width, height];

          

            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {

                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rc = allPixRn[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixGn[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixBn[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }

                
                    gradR = (int)Math.Sqrt((new_rx * new_rx) + (new_ry * new_ry));
                    graidientR[i, j] = gradR;

                    gradG = (int)Math.Sqrt((new_gx * new_gx) + (new_gy * new_gy));
                    graidientG[i, j] = gradG;

                    gradB = (int)Math.Sqrt((new_bx * new_gx) + (new_by * new_by));
                    graidientB[i, j] = gradB;
                   
                    atanR = (int)((Math.Atan((double)new_ry / new_rx)) * (180 / Math.PI));
                    if ((atanR > 0 && atanR < 22.5) || (atanR > 157.5 && atanR < 180))
                    {
                        atanR = 0;
                    }
                    else if (atanR > 22.5 && atanR < 67.5)
                    {
                        atanR = 45;
                    }
                    else if (atanR > 67.5 && atanR < 112.5)
                    {
                        atanR = 90;
                    }
                    else if (atanR > 112.5 && atanR < 157.5)
                    {
                        atanR = 135;
                    }

                    if (atanR == 0)
                    {
                        tanR[i, j] = 0;
                    }
                    else if (atanR == 45)
                    {
                        tanR[i, j] = 1;
                    }
                    else if (atanR == 90)
                    {
                        tanR[i, j] = 2;
                    }
                    else if (atanR == 135)
                    {
                        tanR[i, j] = 3;
                    }

                    atanG = (int)((Math.Atan((double)new_gy / new_gx)) * (180 / Math.PI));
                    if ((atanG > 0 && atanG < 22.5) || (atanG > 157.5 && atanG < 180))
                    {
                        atanG = 0;
                    }
                    else if (atanG > 22.5 && atanG < 67.5)
                    {
                        atanG = 45;
                    }
                    else if (atanG > 67.5 && atanG < 112.5)
                    {
                        atanG = 90;
                    }
                    else if (atanG > 112.5 && atanG < 157.5)
                    {
                        atanG = 135;
                    }


                    if (atanG == 0)
                    {
                        tanG[i, j] = 0;
                    }
                    else if (atanG == 45)
                    {
                        tanG[i, j] = 1;
                    }
                    else if (atanG == 90)
                    {
                        tanG[i, j] = 2;
                    }
                    else if (atanG == 135)
                    {
                        tanG[i, j] = 3;
                    }
                   
                    atanB = (int)((Math.Atan((double)new_by / new_bx)) * (180 / Math.PI));
                    if ((atanB > 0 && atanB < 22.5) || (atanB > 157.5 && atanB < 180))
                    {
                        atanB = 0;
                    }
                    else if (atanB > 22.5 && atanB < 67.5)
                    {
                        atanB = 45;
                    }
                    else if (atanB > 67.5 && atanB < 112.5)
                    {
                        atanB = 90;
                    }
                    else if (atanB > 112.5 && atanB < 157.5)
                    {
                        atanB = 135;
                    }

                    if (atanB == 0)
                    {
                        tanB[i, j] = 0;
                    }
                    else if (atanB == 45)
                    {
                        tanB[i, j] = 1;
                    }
                    else if (atanB == 90)
                    {
                        tanB[i, j] = 2;
                    }
                    else if (atanB == 135)
                    {
                        tanB[i, j] = 3;
                    }
                  
                }
            }

            int[,] allPixRs = new int[width, height];
            int[,] allPixGs = new int[width, height];
            int[,] allPixBs = new int[width, height];

            for (int i = 2; i < width - 2; i++)
            {
                for (int j = 2; j < height - 2; j++)
                {

                   
                    if (tanR[i, j] == 0)
                    {
                        if (graidientR[i - 1, j] < graidientR[i, j] && graidientR[i + 1, j] < graidientR[i, j])
                        {
                            allPixRs[i, j] = graidientR[i, j];
                        }
                        else
                        {
                            allPixRs[i, j] = 0;
                        }
                    }
                    if (tanR[i, j] == 1)
                    {
                        if (graidientR[i - 1, j + 1] < graidientR[i, j] && graidientR[i + 1, j - 1] < graidientR[i, j])
                        {
                            allPixRs[i, j] = graidientR[i, j];
                        }
                        else
                        {
                            allPixRs[i, j] = 0;
                        }
                    }
                    if (tanR[i, j] == 2)
                    {
                        if (graidientR[i, j - 1] < graidientR[i, j] && graidientR[i, j + 1] < graidientR[i, j])
                        {
                            allPixRs[i, j] = graidientR[i, j];
                        }
                        else
                        {
                            allPixRs[i, j] = 0;
                        }
                    }
                    if (tanR[i, j] == 3)
                    {
                        if (graidientR[i - 1, j - 1] < graidientR[i, j] && graidientR[i + 1, j + 1] < graidientR[i, j])
                        {
                            allPixRs[i, j] = graidientR[i, j];
                        }
                        else
                        {
                            allPixRs[i, j] = 0;
                        }
                    }

                 
                    if (tanG[i, j] == 0)
                    {
                        if (graidientG[i - 1, j] < graidientG[i, j] && graidientG[i + 1, j] < graidientG[i, j])
                        {
                            allPixGs[i, j] = graidientG[i, j];
                        }
                        else
                        {
                            allPixGs[i, j] = 0;
                        }
                    }
                    if (tanG[i, j] == 1)
                    {
                        if (graidientG[i - 1, j + 1] < graidientG[i, j] && graidientG[i + 1, j - 1] < graidientG[i, j])
                        {
                            allPixGs[i, j] = graidientG[i, j];
                        }
                        else
                        {
                            allPixGs[i, j] = 0;
                        }
                    }
                    if (tanG[i, j] == 2)
                    {
                        if (graidientG[i, j - 1] < graidientG[i, j] && graidientG[i, j + 1] < graidientG[i, j])
                        {
                            allPixGs[i, j] = graidientG[i, j];
                        }
                        else
                        {
                            allPixGs[i, j] = 0;
                        }
                    }
                    if (tanG[i, j] == 3)
                    {
                        if (graidientG[i - 1, j - 1] < graidientG[i, j] && graidientG[i + 1, j + 1] < graidientG[i, j])
                        {
                            allPixGs[i, j] = graidientG[i, j];
                        }
                        else
                        {
                            allPixGs[i, j] = 0;
                        }
                    }

                   
                    if (tanB[i, j] == 0)
                    {
                        if (graidientB[i - 1, j] < graidientB[i, j] && graidientB[i + 1, j] < graidientB[i, j])
                        {
                            allPixBs[i, j] = graidientB[i, j];
                        }
                        else
                        {
                            allPixBs[i, j] = 0;
                        }
                    }
                    if (tanB[i, j] == 1)
                    {
                        if (graidientB[i - 1, j + 1] < graidientB[i, j] && graidientB[i + 1, j - 1] < graidientB[i, j])
                        {
                            allPixBs[i, j] = graidientB[i, j];
                        }
                        else
                        {
                            allPixBs[i, j] = 0;
                        }
                    }
                    if (tanB[i, j] == 2)
                    {
                        if (graidientB[i, j - 1] < graidientB[i, j] && graidientB[i, j + 1] < graidientB[i, j])
                        {
                            allPixBs[i, j] = graidientB[i, j];
                        }
                        else
                        {
                            allPixBs[i, j] = 0;
                        }
                    }
                    if (tanB[i, j] == 3)
                    {
                        if (graidientB[i - 1, j - 1] < graidientB[i, j] && graidientB[i + 1, j + 1] < graidientB[i, j])
                        {
                            allPixBs[i, j] = graidientB[i, j];
                        }
                        else
                        {
                            allPixBs[i, j] = 0;
                        }
                    }
                }
            }

            int threshold = (int)(treshold*100);
            int[,] allPixRf = new int[width, height];
            int[,] allPixGf = new int[width, height];
            int[,] allPixBf = new int[width, height];

            for (int i = 2; i < width - 2; i++)
            {
                for (int j = 2; j < height - 2; j++)
                {
                    if (allPixRs[i, j] > threshold)
                    {
                        allPixRf[i, j] = 1;
                    }
                    else
                    {
                        allPixRf[i, j] = 0;
                    }

                    if (allPixGs[i, j] > threshold)
                    {
                        allPixGf[i, j] = 1;
                    }
                    else
                    {
                        allPixGf[i, j] = 0;
                    }

                    if (allPixBs[i, j] > threshold)
                    {
                        allPixBf[i, j] = 1;
                    }
                    else
                    {
                        allPixBf[i, j] = 0;
                    }

                    byte[] colorData = { 0, 0, 0, 255 };
                    Int32Rect rect;

                    rect = new Int32Rect(i, j, 1, 1);

                    byte[] colorData2 = { 255, 255, 255, 255 };
                    kennyRes = new bool[width, height];

                    if (allPixRf[i, j] == 1 || allPixGf[i, j] == 1 || allPixBf[i, j] == 1)
                    {
                        kennyRes[i, j] = true;
                    }
                    else
                    {
                        kennyRes[i, j] = false;
                    }
                }
            }
           



        }


        public double PixelIntense(int x, int y)
        {
            int index = Index(x, y);
            if (index >= pixels.Length) return 0;
            return pixels[index] * 0.0722f + pixels[index + 1] * 0.7152f + pixels[index + 2] * 0.2126f;
        }
        public double PixelHomogeneity(int x, int y)
        {
            double res = 0;

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if(x + i >= 0 && x + i < image.PixelWidth && y + j >= 0 && y + j < image.PixelHeight)
                    res = Math.Max(res, Math.Abs(PixelIntense(x, y) - PixelIntense(x + i, y + j)));
                }
            }

            if (res > TRESHOLD) return res;
            else return 0;
                

        }

        public void RecalcHomohenity()
        {
              writableImage = new WriteableBitmap(image);
          
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

            int[,] allPixR = new int[width, height];
            int[,] allPixG = new int[width, height];
            int[,] allPixB = new int[width, height];

            int limit = 128 * 128;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    allPixR[i, j] = pixels[Index(i, j) + 2];
                    allPixG[i, j] = pixels[Index(i, j) + 1];
                    allPixB[i, j] = pixels[Index(i, j)];
                }
            }

            int new_rx = 0, new_ry = 0;
            int new_gx = 0, new_gy = 0;
            int new_bx = 0, new_by = 0;
            int rc, gc, bc;
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {

                    new_rx = 0;
                    new_ry = 0;
                    new_gx = 0;
                    new_gy = 0;
                    new_bx = 0;
                    new_by = 0;
                    rc = 0;
                    gc = 0;
                    bc = 0;

                    for (int wi = -1; wi < 2; wi++)
                    {
                        for (int hw = -1; hw < 2; hw++)
                        {
                            rc = allPixR[i + hw, j + wi];
                            new_rx += gx[wi + 1, hw + 1] * rc;
                            new_ry += gy[wi + 1, hw + 1] * rc;

                            gc = allPixG[i + hw, j + wi];
                            new_gx += gx[wi + 1, hw + 1] * gc;
                            new_gy += gy[wi + 1, hw + 1] * gc;

                            bc = allPixB[i + hw, j + wi];
                            new_bx += gx[wi + 1, hw + 1] * bc;
                            new_by += gy[wi + 1, hw + 1] * bc;
                        }
                    }

                    byte[] colorData = { 0, 0, 0, 255 };
                    Int32Rect rect;
                                     
                                   rect  = new Int32Rect(i, j, 1, 1);
                                 
                     byte[] colorData2 = { 255, 255, 255, 255 };

                                   
                    marked = new bool[width, height];
                    if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
                    {
                           writableImage.WritePixels(rect, colorData, 4, 0);
                           marked[i, j] = true;
                    }
                   
                    else
                    {
                        writableImage.WritePixels(rect, colorData2, 4, 0);
                        marked[i, j] = false;
                    }
                }
            }
 
        }

        private void RecalcDiefference()
        {
            double matches = 0;
            for(int i = 0; i < image.PixelWidth; i++)
            {
                for(int j = 0; j < image.PixelHeight; j++)
                {
                    if(marked[i, j] == kennyRes[i, j])
                    {
                        matches++;
                    }
                }
            }

            accuracy = matches / (image.Width * image.Height);
            
        }
        public void EdgeDetectPSO()
        {
            DateTime dateTime1, dateTime2;
            dateTime1 = DateTime.Now;
            
            writableImage = new WriteableBitmap(image);
            double[,] velocity = new double[10, DIMENSION_COUNT];
            for (int i = 0; i < 10; i++)
            {
                for(int j = 0; j < DIMENSION_COUNT; j++)
                {
                    velocity[i, j] = 50;
                }
            }
            RecalcHomohenity();

            for (int i = 0; i < image.PixelHeight / (DIMENSION_COUNT * DIMENSION_COUNT); i ++)
                {
                    for (int j = 0; j < image.PixelWidth / (DIMENSION_COUNT * DIMENSION_COUNT); j ++)
                    {
                        if (!marked[i, j])
                        {
                            Particle[] particles = new Particle[DIMENSION_COUNT];
                            Particle[] pBestPart = new Particle[DIMENSION_COUNT];
                            Particle gBestPart;
                            double[] objectiveFunction = new double[DIMENSION_COUNT];
                            double[] pBest = new double[DIMENSION_COUNT];
                            double gBest = -INFINITY;
                            for (int k = 0; k < DIMENSION_COUNT; k++)
                            {
                                objectiveFunction[k] = -INFINITY;
                                pBest[k] = -INFINITY;
                                particles[k] = new Particle(DIMENSION_COUNT, i, j, this);
                                pBestPart[k] = new Particle(particles[k]);
                            }

                            gBestPart = new Particle(particles[0]);

                            int curX = i, curY = j;
                            if (gBestPart.Length() > 1000)
                            {
                                for (int t = 0; t < gBestPart.dimensionsCount; t++)
                                {
                                    if (gBestPart.dimensions[t] == 0) break;
                                    if (gBestPart.dimensions[t] == 1)
                                    {
                                        curY--;
                                    }
                                    if (gBestPart.dimensions[t] == 2)
                                    {
                                        curY--;
                                        curX++;
                                    }
                                    if (gBestPart.dimensions[t] == 3)
                                    {
                                        curX++;
                                    }
                                    if (gBestPart.dimensions[t] == 4)
                                    {
                                        curY++;
                                        curX++;
                                    }
                                    if (gBestPart.dimensions[t] == 5)
                                    {
                                        curY++;
                                    }
                                    if (gBestPart.dimensions[t] == 6)
                                    {
                                        curY++;
                                        curX--;
                                    }
                                    if (gBestPart.dimensions[t] == 7)
                                    {
                                        curX--;
                                    }
                                    if (gBestPart.dimensions[t] == 8)
                                    {
                                        curY--;
                                        curX--;
                                    }
                                
                                }
                            }
                        }
                    }
                }
                dateTime2 = DateTime.Now;
            DateTime date = DateTime.Now;
            long a = (long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds;
                string tmp1 = "F:\\image";
                string tmp2 = a.ToString();
                tmp1 = string.Concat(tmp1, tmp2);
                tmp2 = ".jpg";
                tmp1 = string.Concat(tmp1, tmp2);
                CreateThumbnail(tmp1, writableImage.Clone());
                
                Kenny();
                
                RecalcDiefference();
                           
                time = (float)(dateTime2 - dateTime1).TotalSeconds;
                

        }



        void CreateThumbnail(string filename, BitmapSource image5)
        {
            if (filename != string.Empty)
            {
                using (FileStream stream5 = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder5 = new PngBitmapEncoder();
                    encoder5.Frames.Add(BitmapFrame.Create(image5));
                    encoder5.Save(stream5);
                    stream5.Close();
                }
            }
        }
    }
}
