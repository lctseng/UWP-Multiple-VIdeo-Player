using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Collections;
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

//空白頁項目範本收錄在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409



namespace Homework_1
{

    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public bool ctrlPressed { get; set; }

        ArrayList players;

        public MainPage()
        {
            this.InitializeComponent();
            players = new ArrayList();


            ctrlPressed = false;

            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                if (ctrl.HasFlag(CoreVirtualKeyStates.Down))
                {
                    ctrlPressed = true;
                }
            };
            Window.Current.CoreWindow.KeyUp += (s, e) =>
            {
                var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                if (ctrl.HasFlag(CoreVirtualKeyStates.None))
                {
                    ctrlPressed = false;
                }
            };
            
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ElementSoundPlayer.Play(ElementSoundKind.Invoke);
            Button b = (Button)sender;
            b.Foreground = new SolidColorBrush(Windows.UI.Colors.Blue);

        }
        private void TxtFilePath_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox tbPath = sender as TextBox;

                if (tbPath != null)
                {
                    LoadMediaFromString(tbPath.Text);
                }
            }
        }

        private void LoadMediaFromString(string path)
        {
            try
            {
                
                Uri pathUri = new Uri(path);
                FloatMediaElement player = new FloatMediaElement(this);
                player.mediaElement.Source = pathUri;
                player.mediaElement.Play();
                players.Add(player);

            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    // handle exception.
                    // For example: Log error or notify user problem with file
                }

            }
        }

        private async void Choose_File_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();


            await PlayFromFile(file);


        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    var file = items[0] as StorageFile;
                    await PlayFromFile(file);
                }
            }
        }

        private async Task<bool> PlayFromFile(StorageFile file)
        {
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                FloatMediaElement player = new FloatMediaElement(this);                
                player.mediaElement.SetSource(stream, file.ContentType);
                player.mediaElement.Play();
                players.Add(player);
                return true;
            }
            else
            {
                return false;
            }
        }

        public double maxPlayWidth()
        {
            return playAreaGrid.ActualWidth ;
        }

        public double maxPlayHeight()
        {
            return playAreaGrid.ActualHeight - 50;
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Link;
        }

        public static double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public Grid getPlayAreaGrid()
        {
            return playAreaGrid;
        }


    }

}
