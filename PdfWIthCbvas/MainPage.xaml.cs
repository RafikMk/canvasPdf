using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using PdfiumViewer;

namespace PdfWIthCbvas;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
        var panGestureRecognizer = new PanGestureRecognizer();
        panGestureRecognizer.PanUpdated += OnStackLayoutPanUpdated;
        stackLayout1.GestureRecognizers.Add(panGestureRecognizer);
        // Content = grid;
    }
    private void OnStackLayoutPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var position = e.TotalX + e.TotalY;
      Debug.WriteLine("X= "+e.TotalX.ToString()+" Y ="+ e.TotalY.ToString());
        // Do something with the touch position...
    }
    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        //	if (count == 1)
        //CounterBtn.Text = $"Clicked {count} time";
        //else
        //	CounterBtn.Text = $"Clicked {count} times";

        //	SemanticScreenReader.Announce(CounterBtn.Text);

    }
    //private double currentScale = 1;

   


    private async void aaab(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Pdf,
            PickerTitle = "Select a PDF file"
        });

        if (result != null)
        {

            var pdfDocument = PdfDocument.Load(result.FullPath);
            List<ImageSource> images = new List<ImageSource>();
            
            for (int page = 0; page < pdfDocument.PageCount; page++)
            {
                using (var img = pdfDocument.Render(page, 300, 300, false))
                {
                    using (var stream = new MemoryStream())
                    {
                        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        var imageData = stream.ToArray(); // Convert the memory stream to a byte array
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageData)); // Create a new memory stream from the byte array

                        images.Add(imageSource);
                    }
                }
            }
            imageStackLayout.Children.Clear();

            for (int i = 0; i < images.Count; i++)
            {
                Grid grid = new Grid();

                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = GridLength.Star;
                grid.RowDefinitions.Add(rowDef);

                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = GridLength.Star;
                grid.ColumnDefinitions.Add(colDef);

                Image image = new Image();
                image.SetBinding(Image.ScaleProperty, new Binding("Value", source: slider));
                image.Source = images[i];
                image.Aspect = Aspect.AspectFit;
                Grid.SetRow(image, 0);
                Grid.SetColumn(image, 0);
                grid.Children.Add(image);

                StackLayout stackLayout2 = new StackLayout { BackgroundColor = Colors.Transparent };
                //    stackLayout2.BackgroundColor = Color.Transparent;
                //   stackLayout2.Name = "stackLayout2";
                AbsoluteLayout.SetLayoutFlags(stackLayout2, AbsoluteLayoutFlags.PositionProportional);
                Grid.SetRow(stackLayout2, 0);
                Grid.SetColumn(stackLayout2, 0);
                grid.Children.Add(stackLayout2);

                /* Ajoutez ici les éléments de la deuxième StackLayout 

                Image signatureImg = new Image();
                signatureImg.Name = "SignatureImg";
                signatureImg.BackgroundColor = Color.Red;
                stackLayout2.Children.Add(signatureImg);
                */

                StackLayout stackLayout1 = new StackLayout { BackgroundColor = Colors.Transparent };
                //  stackLayout1.BackgroundColor = Color.Transparent;
                // stackLayout1.Name = "stackLayout1";

              


                AbsoluteLayout.SetLayoutFlags(stackLayout1, AbsoluteLayoutFlags.PositionProportional);
                Grid.SetRow(stackLayout1, 0);
                Grid.SetColumn(stackLayout1, 0);
                grid.Children.Add(stackLayout1);

                var drawingView = new DrawingView { BackgroundColor = Colors.Transparent };
                drawingView.ClassId = "DrawingView22";
              
                //drawingView.DrawingLineCompleted += DrawingView_DrawingLineCompleted;
                // drawingView.BackgroundColor = Color.Transparent;
                drawingView.LineWidth = 2;
                drawingView.HorizontalOptions = LayoutOptions.FillAndExpand;
                drawingView.VerticalOptions = LayoutOptions.FillAndExpand;
                stackLayout1.Children.Add(drawingView);


                //  var stackLayout = new StackLayout();
                // stackLayout.Orientation = StackOrientation.Vertical;             //   stackLayout.BackgroundColor = Color.Yellow;
            //    var imageView = new Image();
            //    imageView.Source = images[i];
             //   imageView.SetBinding(Image.ScaleProperty,new Binding("Value", source:slider));
              // var drawingView = new DrawingView();
             //   drawingView.IsMultiLineModeEnabled = false;
              //  drawingView.LineWidth = 2;
              //  drawingView.WidthRequest = 400;
              //  drawingView.HeightRequest = 400;

              //  stackLayout.Children.Add(imageView);
              //  stackLayout.Children.Add(drawingView);

                imageStackLayout.Children.Add(grid);



            }
        }
    }

    private async void DrawingView_DrawingLineCompleted(object sender, CommunityToolkit.Maui.Core.DrawingLineCompletedEventArgs e)
    {

       var stream = await DrawingView.GetImageStream(500, 500);
      
        SignatureImg.Source = ImageSource.FromStream( () => stream);
    }
    /*
    private void DragGestureRecognizer_DragStarting_1(object sender, DragStartingEventArgs e)
    {
        // Récupère l'élément en cours de glissement (l'image)
    //   Image image = (Image)sender;
        var image = (sender as Element)?.Parent as Image;
       // Console.WriteLine("eeee" + image.Source);
        Debug.WriteLine("eeee" + image.Source);
        // Ajoute les données de l'image en tant que données à transférer lors du glissement
        e.Data.Properties.Add("image", image.Source);
    }
    private void DropGestureRecognizer_Drop_1(object sender, DropEventArgs e)
    {
        Console.WriteLine("e " + e);
        var data = e.Data.Properties["image"].ToString();
        var image = (sender as Element)?.Parent as Image;
       // image.Source = ImageSource.FromUri(new Uri("https://aka.ms/campus.jpg")) ;
    }*/
    private void DragGestureRecognizer_DragStarting_1(object sender, DragStartingEventArgs e)
    {
        var label = (sender as Element)?.Parent as Label;
        e.Data.Properties.Add("Text", label.Text);
    }

    private void DropGestureRecognizer_Drop_1(object sender, DropEventArgs e)
    {
        Console.WriteLine("zzz" + e);
        var data = e.Data.Properties["Text"].ToString();
        var frame = (sender as Element)?.Parent as Frame;
        frame.Content = new Label
        {
            Text = data
        };
    }



}







