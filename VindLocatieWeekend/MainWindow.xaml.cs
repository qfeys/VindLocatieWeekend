using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VindLocatieWeekend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Chart chart;
        Canvas canvas;

        public const int width = 1200, height = 800;

        public MainWindow()
        {
            InitializeComponent();
            canvas = new Canvas();
            Content = canvas;
            chart = Chart.GenerateNew();

            // Create an Image to display the bitmap.
            //var source = ConvertWriteableBitmapToBitmapImage(chart.Draw());
            var source = chart.Draw();
            Image image = new Image {
                Stretch = Stretch.None,
                Width = width,
                Margin = new Thickness(0),
                Source = source,
                
            };

            BitmapImage kaart = new BitmapImage(new Uri("kaart.png", UriKind.Relative));
            double qdskfjlhuqe = kaart.Width;
            Image image_kaart = new Image {
                Source = kaart
            };


            SaveImageToFile(source);

            canvas.Children.Add(image_kaart);
            canvas.Children.Add(image);
        }

        private void Buttonclick(object sender, KeyEventArgs e)
        {
            
        }

        public BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }

        public static void SaveImageToFile(BitmapSource img)
        {
            using (var fileStream = new FileStream(".\\image.png", FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(img));
                encoder.Save(fileStream);
            }
        }
    }
}
