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
        public FrameworkElement parent { get; }

        public FloatMediaElement(FrameworkElement parent)
        {
            transform = new CompositeTransform();
            mediaElement = new MediaElement();
            this.parent = parent;
            initializePlayer();
        }

        private void initializePlayer()
        {
            mediaElement.Width = 400;
            mediaElement.Height = 300;
            parent.Children.Add(mediaElement);
        }
    }

}