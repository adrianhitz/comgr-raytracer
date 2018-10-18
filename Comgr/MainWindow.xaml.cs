using Raytracing;
using Raytracing.Premade;
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
        public MainWindow() {
            InitializeComponent();

            int imageResolution = 400;
            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(imageResolution, imageResolution, 96, 96, PixelFormats.Bgr24, null);
            image.Source = writeableBitmap;
            grid.Children.Add(image);
            Task task = new Task(() => {
                Raytracer raytracer = new Raytracer(Premade.CornellBox.Camera(), Premade.CornellBox.Scene()) {
                    AASamples = 20,
                    ShadowSamples = 20
                };
                byte[] pixels = raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
                this.Dispatcher.Invoke(() => {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, imageResolution, imageResolution), pixels, imageResolution * 3, 0);
                    this.Activate();
                });
            });
            task.Start();
        }
    }
}
