using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VindLocatieWeekend
{
    class Chart
    {
        List<Vector> cities = new List<Vector>();

        Chart() { }

        public static Chart GenerateNew()
        {
            return new Chart() {
                cities = new List<Vector>() {
                    coord2km(Menen),
                    coord2km(Wevelgem),
                    coord2km(Wulvergem),
                    coord2km(Merelbeke)
                }
            };
        }

        internal BitmapSource Draw()
        {
            double afstand = Vector.Distance(cities[0] * (1 / scaling), cities[1] * (1 / scaling));
            afstand = Vector.Distance(cities[0] * (1 / scaling), cities[2] * (1 / scaling));
            afstand = Vector.Distance(cities[0] * (1 / scaling), cities[3] * (1 / scaling));
            afstand = Vector.Distance(cities[1] * (1 / scaling), cities[2] * (1 / scaling));
            afstand = Vector.Distance(cities[1] * (1 / scaling), cities[3] * (1 / scaling));
            afstand = Vector.Distance(cities[2] * (1 / scaling), cities[3] * (1 / scaling));
            // draw using byte array
            int width = MainWindow.width, height = MainWindow.height;

            WriteableBitmap wbitmap = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
            byte[,,] pixels = new byte[width, height, 4];

            int colorbands = 9;
            Palette colorsGreen = ColorBrewerPalette.Greens(colorbands);
            Palette colorsRed = ColorBrewerPalette.Reds(colorbands);
            double baseDistance = 52;


            // Change the pixels here
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector loc = new Vector(i / scaling / 10.0, j / scaling / 10.0);
                    double totalDist = cities.Sum(c => Vector.Distance(c, loc));
                    double average = totalDist / cities.Count;
                    double diff = average - baseDistance;
                    diff *= .6;
                    Color32 col;
                    if (cities.Any(c => Vector.Distance(c, loc) < 0.5))
                        col = new Color32(0, 0, 255, 255);
                    else if (cities.Any(c => Vector.Distance(c, loc) < 0.8))
                        col = new Color32(255, 0, 0, 255);
                    else if (diff < -colorbands - 1)
                        col = new Color32(255, 255, 255, 0);
                    else if (diff < -1)
                        col = colorsRed.colors[colorbands +(int)diff];
                    else if (diff <= 1)
                        col = new Color32(0, 0, 0, 255);
                    else if (diff < colorbands + 1)
                        col = colorsGreen.colors[colorbands - (int)diff];
                    else
                        col = new Color32(255, 255, 255, 0);

                    pixels[i, j, 0] = col.B;
                    pixels[i, j, 1] = col.G;
                    pixels[i, j, 2] = col.R;
                    pixels[i, j, 3] = col.A > 127 ? (byte)127 : col.A;
                }
            }


            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[height * width * 4];
            int index = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int i = 0; i < 4; i++)
                        pixels1d[index++] = pixels[col, row, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            System.Windows.Int32Rect rect = new System.Windows.Int32Rect(0, 0, width, height);
            int stride = 4 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0, 0);
            //BitmapSource source = BitmapSource.Create(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgr32, null, pixels1d, stride);

            return wbitmap;
        }


        public static Vector Menen { get { return new Vector(50.797821, 3.114734); } }
        public static Vector Wevelgem { get { return new Vector(50.803068, 3.157326); } }
        public static Vector Wulvergem { get { return new Vector(50.767467, 2.850780); } }
        public static Vector Merelbeke { get { return new Vector(51.016830, 3.747549); } }


        //const double scaling = 0.743;
        const double scaling = 1.008;
        static Vector offset = new Vector(29, -3);

        public static Vector coord2km(Vector coord)
        {
            //return scaling * new Vector((coord.y - 2.8) * Math.Cos(coord.x) * 111 + offset.x, -(coord.x - 51.2) * 111 + offset.y);
            return scaling * new Vector((coord.y - 2.8) * 70.3 + offset.x, -(coord.x - 51.2) * 111.2 + offset.y);
        }
    }
}
