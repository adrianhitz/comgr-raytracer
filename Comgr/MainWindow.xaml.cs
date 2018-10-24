using Raytracing;
using Raytracing.Pathtracing;
using Raytracing.Premade;
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

                // byte[] pixels = Lab03();
                // byte[] pixels = Lab04BVH();
                byte[] pixels = Lab04Textures();
                // byte[] pixels = Lab05();
                // byte[] pixels = Lab06();
                // byte[] pixels = DepthOfField();

                this.Dispatcher.Invoke(() => {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, imageResolution, imageResolution), pixels, imageResolution * 3, 0);
                    this.Activate();
                });
            });
            task.Start();
        }

        private static byte[] Lab03() {
            Raytracer raytracer = new Raytracer(Premade.Lab03.Camera(), Premade.Lab03.Scene());
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] Lab04BVH() {
            Raytracer raytracer = new Raytracer(Premade.Lab04BVH.Camera(), Premade.Lab04BVH.Scene(Scene.AccelerationStructure.BVH));
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
            Pathtracer pathtracer = new Pathtracer(Premade.Lab06.Camera(), Premade.Lab06.Scene()) {
                Samples = 512,
                RecursionDepth = 4
            };
            return pathtracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }

        private static byte[] DepthOfField() {
            Raytracer raytracer = new Raytracer(Premade.DepthOfField.Camera(), Premade.DepthOfField.Scene()) {
                SuperSampling = 512,
                ShadowSamples = 4
            };
            return raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
        }
    }
}
