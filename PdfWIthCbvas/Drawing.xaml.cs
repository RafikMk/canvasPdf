using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace PdfWIthCbvas;

public partial class Drawing : ContentPage
{
	public Drawing()
	{
		InitializeComponent();
	}
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        SKPaint paint = new SKPaint
        {
            Color = SKColors.Red,
            StrokeWidth = 10
        };

        canvas.DrawCircle(100, 100, 50, paint);
    }

}