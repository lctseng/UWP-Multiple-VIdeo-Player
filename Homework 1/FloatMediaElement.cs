using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Homework_1
{
    public class FloatMediaElement
    {

        public MediaElement mediaElement { get; }
        public CompositeTransform transform { get; }
        public Grid parent { get; }
        public MainPage page { get; }

        public FloatMediaElement(MainPage page)
        {
            this.page = page;
            parent = page.getPlayAreaGrid();

            transform = new CompositeTransform();
            mediaElement = new MediaElement();
            
            initializePlayer();
        }

        private void initializePlayer()
        {
            mediaElement.Width = 400;
            mediaElement.Height = 300;


            mediaElement.AutoPlay = true;
            mediaElement.Width = 400;
            mediaElement.Height = 300;
            mediaElement.AreTransportControlsEnabled = true;
            mediaElement.ManipulationMode = ManipulationModes.All;
            mediaElement.ManipulationDelta += playerManipulationDelta;
            mediaElement.ManipulationStarted += playerManipulationStarted;
            mediaElement.ManipulationCompleted += playerManipulationCompleted;
            mediaElement.HorizontalAlignment = HorizontalAlignment.Left;
            mediaElement.VerticalAlignment = VerticalAlignment.Top;
            mediaElement.IsLooping = true;
            mediaElement.TransportControls.IsZoomEnabled = mediaElement.TransportControls.IsZoomButtonVisible = false;
            mediaElement.TransportControls.IsPlaybackRateEnabled = mediaElement.TransportControls.IsPlaybackRateButtonVisible = true;
            mediaElement.TransportControls.IsFullWindowEnabled = mediaElement.TransportControls.IsFullWindowButtonVisible = false;


            parent.Children.Add(mediaElement);

        }

        private void playerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            ManipulationDelta md = e.Delta;
            Point trans = md.Translation;
            if (page.ctrlPressed)
            {
                // Scale
                if (transform.ScaleX + trans.X / 100.0 < 0.1)
                {
                    transform.ScaleX = 0.1;
                }
                else if (trans.X > 0.0 && mediaElement.Margin.Left + mediaElement.ActualWidth > page.maxPlayWidth())
                {
                    // do not scale X
                }
                else
                {
                    transform.ScaleX += trans.X / 100.0;
                }
                if (transform.ScaleY + trans.Y / 100.0 < 0.1)
                {
                    transform.ScaleY = 0.1;
                }
                else if (trans.Y > 0.0 && mediaElement.Margin.Top + mediaElement.ActualHeight > page.maxPlayHeight())
                {
                    // do not scale Y
                }
                else
                {
                    transform.ScaleY += trans.Y / 100.0;
                }

            }
            else
            {
                // Translate
                transform.TranslateX = Clamp(transform.TranslateX + trans.X, 0.0, page.maxPlayWidth() - mediaElement.ActualWidth);
                transform.TranslateY = Clamp(transform.TranslateY + trans.Y, 0.0, page.maxPlayHeight() - mediaElement.ActualHeight);

            }
            updateSizeByTranform();
            e.Handled = true;
        }

        private void updateSizeByTranform()
        {
            // Scale
            mediaElement.Width = transform.ScaleX * 400;
            mediaElement.Height = transform.ScaleY * 300;
            // Translate
            var newMargin = mediaElement.Margin;
            newMargin.Left = transform.TranslateX;
            newMargin.Top = transform.TranslateY;
            newMargin.Right = page.maxPlayWidth() - newMargin.Left - mediaElement.ActualWidth;
            newMargin.Bottom = page.maxPlayHeight() - newMargin.Top - mediaElement.ActualHeight;
            mediaElement.Margin = newMargin;
            //commandBarText.Text = "btm:" + newMargin.Bottom + ", top:" + newMargin.Top;
        }

        private void playerManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            mediaElement.Opacity = 0.4;

        }

        private void playerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            mediaElement.Opacity = 1.0;
        }

        public static double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }

}