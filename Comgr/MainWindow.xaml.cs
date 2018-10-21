using Raytracing;
using Raytracing.Pathtracing;
using Raytracing.Premade;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Comgr {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private const int imageResolution = 800;

        public MainWindow() {
            InitializeComponent();

            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(imageResolution, imageResolution, 96, 96, PixelFormats.Bgr24, null);
            image.Source = writeableBitmap;
            grid.Children.Add(image);
            Task task = new Task(() => {
                byte[] pixels = RaytracingImage();
                this.Dispatcher.Invoke(() => {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, imageResolution, imageResolution), pixels, imageResolution * 3, 0);
                    this.Activate();
                    Debug.Write("Done!");
                });
            });
            task.Start();
        }

        private static byte[] RaytracingImage() {
            Raytracer raytracer = new Raytracer(Premade.CornellBox.Camera(), Premade.CornellBox.Scene()) {
                SuperSampling = 1,
                ShadowSamples = 1
            };
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] PathtracingImage() {
            Pathtracer pathtracer = new Pathtracer(Premade.CornellBox.Camera(), Premade.PathCornellBox.Scene()) {
                Samples = 128,
                RecursionDepth = 4
            };
            return pathtracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab03() {
            Raytracer raytracer = new Raytracer(Premade.Lab03.Camera(), Premade.Lab03.Scene()) {
                SuperSampling = 20,
                ShadowSamples = 1
            };
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
    }
    }
}
