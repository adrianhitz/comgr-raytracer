using Raytracing;
using Raytracing.Pathtracing;
using Raytracing.Premade;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Raytracing.Scene;

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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                // byte[] pixels = Lab03();
                // byte[] pixels = Lab04BVH();
                // byte[] pixels = Lab04Textures();
                // byte[] pixels = Lab05();
                byte[] pixels = Lab06();
                this.Dispatcher.Invoke(() => {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, imageResolution, imageResolution), pixels, imageResolution * 3, 0);
                    stopwatch.Stop();
                    this.Activate();
                    TimeSpan ts = stopwatch.Elapsed;
                    Debug.WriteLine("Done! Rendering took " + ts.ToString());
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

        private static byte[] Lab03() {
            Raytracer raytracer = new Raytracer(Premade.Lab03.Camera(), Premade.Lab03.Scene());
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab04BVH() {
            Raytracer raytracer = new Raytracer(Premade.Lab04BVH.Camera(), Premade.Lab04BVH.Scene(AccelerationStructure.BVH));
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab04Textures() {
            Raytracer raytracer = new Raytracer(Premade.Lab04Textures.Camera(), Premade.Lab04Textures.Scene());
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab05() {
            Raytracer raytracer = new Raytracer(Premade.Lab05.Camera(), Premade.Lab05.Scene(), 30, 30);
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab06() {
            Pathtracer pathtracer = new Pathtracer(Premade.CornellBox.Camera(), Premade.Lab06.Scene()) {
                Samples = 512,
                RecursionDepth = 4
            };
            return pathtracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }
    }
}
