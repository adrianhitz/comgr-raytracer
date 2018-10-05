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

            int size = 1000;
            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr24, null);
            image.Source = writeableBitmap;
            grid.Children.Add(image);

            Raytracer raytracer = new Raytracer(Camera.CornellBoxCamera(), Scene.CornellBox()) {
                RecursionDepth = 2
            };
            var pixels = raytracer.CalculatePixelsByteArray(size, size);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, size, size), pixels, size * 3, 0);
        }
    }
}
