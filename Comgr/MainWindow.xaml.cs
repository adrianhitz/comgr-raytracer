using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Raytracing;

namespace Comgr {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            int width = 1000;
            int height = 1000;
            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
            image.Source = writeableBitmap;
            grid.Children.Add(image);

            Raytracer raytracer = new Raytracer(Camera.CornellBoxCamera(), Scene.CornellBox());
            var pixels = raytracer.CalculatePixelsByteArray(width, height);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 3, 0);
        }
    }
}
