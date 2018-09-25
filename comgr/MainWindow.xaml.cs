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

namespace comgr {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            int width = 400;
            int height = 400;
            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null); // TODO change to Bgr24
            image.Source = writeableBitmap; ;
            grid.Children.Add(image);

            Raytracer raytracer = new Raytracer(Camera.CornellBoxCamera(), Scene.CornellBox(), width, height);
            var pixels = raytracer.CalculatePixelsByteArray();
            writeableBitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
        }
    }
}
