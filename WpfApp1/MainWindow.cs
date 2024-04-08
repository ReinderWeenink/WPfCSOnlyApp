using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shell;
using Orientation = System.Windows.Controls.Orientation;
using Button = System.Windows.Controls.Button;
using System.Windows.Media.Imaging;
using Forms = System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace WpfApp1
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
        }
        public static Window SetupWindow()
        {
            BitmapImage myBitmapImage = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            string iconFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "images.png");
            if (File.Exists(iconFilePath))
            {
                myBitmapImage.UriSource = new Uri(iconFilePath);
            }
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();
            //set image source
            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Source = myBitmapImage,
                Margin = new Thickness(2, 5, 2, 2),
                //Height = SystemInformation.CaptionButtonSize.Height,
                Width = SystemInformation.CaptionButtonSize.Width,
            };
            //Setup window

            Window window = new Window();
            window.ResizeMode = ResizeMode.CanMinimize;
            window.ResizeMode = ResizeMode.CanResize;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;
            window.ShowInTaskbar = true;
            window.Icon = BitmapFrame.Create(new Uri(iconFilePath));
            window.ContextMenu = new ContextMenu ();

            WindowChrome windowChrome = new WindowChrome
            {
                UseAeroCaptionButtons = false
            };
            WindowChrome.SetWindowChrome(window, windowChrome);

            Grid baseGrid = new Grid();
            //add 2 row 1 for titlebar 1 for rest
            baseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.0, GridUnitType.Auto) });
            baseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.0, GridUnitType.Star) });
            //SSetup grid for windowheader. 1 column for title and icon, 1 column for buttons.
            Grid windowHeader = new Grid { Height = SystemParameters.WindowNonClientFrameThickness.Top, Margin = new Thickness(0, -1, 0, 0) };
            windowHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
            windowHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Auto) });
            //windowheader is first row of basegrid
            Grid.SetRow(windowHeader, 0); // row for title  etc.
            //Setup panel for title and image
            StackPanel titlePanel = new StackPanel { Orientation = Orientation.Horizontal };//panel for image and title
            
            
            titlePanel.Children.Add(image);
            WindowChrome.SetIsHitTestVisibleInChrome(image, true);

            //Titlepanel is in the first columen of windowheader
            Grid.SetColumn(titlePanel, 0);
            windowHeader.Children.Add(titlePanel);
            //buttons
            StackPanel buttonsPanel = new StackPanel
            {
                HorizontalAlignment =  System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch,
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, -1, 0),
            };
            Grid.SetColumn(buttonsPanel, 1);
            windowHeader.Children.Add(buttonsPanel);
            Button minimizeButton = SetButton("minimizeButton", "－", true, Visibility.Visible);
            minimizeButton.Click += new RoutedEventHandler(MininizeWindow);
            buttonsPanel.Children.Add(minimizeButton);

            Grid maximizeRestoreGrid = new Grid
            {
                Margin = new Thickness(1, 0, 1, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            };
            Button restoreButton = SetButton("restoreButton", "❐", true, Visibility.Collapsed);
            Button maximizeButton = SetButton("maximizeButton", "⬜", true, Visibility.Visible);
            restoreButton.Click += new RoutedEventHandler(RestoreWindow);
            maximizeButton.Click += new RoutedEventHandler(MaximizeWindow);

            maximizeRestoreGrid.Children.Add(restoreButton);
            maximizeRestoreGrid.Children.Add(maximizeButton);
            buttonsPanel.Children.Add(maximizeRestoreGrid);

            Button closeButton = SetButton("closeButton", "🗙", true, Visibility.Visible);
            closeButton.Click += new RoutedEventHandler(CloseWindow);
            buttonsPanel.Children.Add(closeButton);

            Grid.SetColumn(buttonsPanel, 1);
            baseGrid.Children.Add(windowHeader);
            window.Content = baseGrid;
            return window;

            void CloseWindow(object sender, RoutedEventArgs e)
            {
                window.Close();
            }
            void MininizeWindow(object sender, RoutedEventArgs e)
            {
                window.WindowState = WindowState.Minimized;
            }
            void MaximizeWindow(object sender, RoutedEventArgs e)
            {
                window.WindowState = WindowState.Maximized;
                maximizeButton.Visibility = Visibility.Collapsed;
                restoreButton.Visibility = Visibility.Visible;

            }
            void RestoreWindow(object sender, RoutedEventArgs e)
            {
                window.WindowState = WindowState.Normal;
                maximizeButton.Visibility = Visibility.Visible;
                restoreButton.Visibility = Visibility.Collapsed;
            }
        }
        private static Button SetButton(string name, string content, bool hitTestVisibleInChrome, Visibility visibility)
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            
            Button button = new Button
            {
                Name = name,
                Content = content,
                Visibility = visibility,
                Height = SystemInformation.CaptionButtonSize.Height,
                Width = SystemInformation.CaptionButtonSize.Width,
                BorderThickness = new Thickness(0),
                Background = solidColorBrush
            };
            WindowChrome.SetIsHitTestVisibleInChrome(button, hitTestVisibleInChrome);
            return button;
        }
    }
}