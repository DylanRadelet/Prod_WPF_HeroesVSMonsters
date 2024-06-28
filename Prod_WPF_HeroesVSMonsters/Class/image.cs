using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

public class GameImage
{
    public Image ImageControl { get; private set; }
    public string SourcePath { get; private set; }
    public bool IsVisible { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }
    public double X { get; private set; }
    public double Y { get; private set; }
    public int Index { get; private set; }

    public GameImage(string sourcePath, double width, double height, double x, double y, int index)
    {
        SourcePath = sourcePath;
        Width = width;
        Height = height;
        X = x;
        Y = y;
        Index = index;
        IsVisible = false;
        InitializeImage();
    }

    private void InitializeImage()
    {
        ImageControl = new Image
        {
            Width = Width,
            Height = Height,
            Source = new BitmapImage(new Uri(SourcePath, UriKind.RelativeOrAbsolute)),
            Visibility = Visibility.Hidden
        };

        Canvas.SetLeft(ImageControl, X);
        Canvas.SetTop(ImageControl, Y);
        Canvas.SetZIndex(ImageControl, 1001);
    }

    public void Show()
    {
        ImageControl.Visibility = Visibility.Visible;
        IsVisible = true;
    }

    public void Hide()
    {
        ImageControl.Visibility = Visibility.Hidden;
        IsVisible = false;
    }
}
