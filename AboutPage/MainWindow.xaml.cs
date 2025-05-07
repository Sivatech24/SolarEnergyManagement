using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace AboutPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Ensure that the video source is set properly before playing
            BackgroundVideo.Source = new Uri(@"C:\Users\tech\Documents\Visual Studio Works\AboutPage\2249554-uhd_3840_2160_24fps.mp4");
            BackgroundVideo.Position = TimeSpan.Zero;
            // Start the video once the window is loaded
            BackgroundVideo.Play();
            
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open the link in the default browser
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void BackgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Loop the video
            BackgroundVideo.Position = TimeSpan.Zero; // Reset the video to the start
            BackgroundVideo.Play(); // Play it again
        }

    }
}
