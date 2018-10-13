using Raytracing;
using Raytracing.Premade;
using System.Threading;
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
            
            int imageResolution = 800;
            Image image = new Image();
            WriteableBitmap writeableBitmap = new WriteableBitmap(imageResolution, imageResolution, 96, 96, PixelFormats.Bgr24, null);
            image.Source = writeableBitmap;
            grid.Children.Add(image);
            Raytracer raytracer = new Raytracer(Premade.CornellBox.Camera(), Premade.CornellBox.Scene());
            byte[] pixels = null;
            Thread t = new Thread(() => {
                pixels = raytracer.CalculatePixelsByteArray(imageResolution, imageResolution);
                this.Dispatcher.Invoke(() => {
                    writeableBitmap.WritePixels(new Int32Rect(0, 0, imageResolution, imageResolution), pixels, imageResolution * 3, 0);
                    this.Activate();
                });
            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
