using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace RysowanieKB2202
{
    public class GraphicsDrawable : IDrawable
    {
        public float Line1StartX { get; set; }
        public float Line1StartY { get; set; }
        public float Line1EndX { get; set; }
        public float Line1EndY { get; set; }

        public float Line2AngleDegrees { get; set; }
        public float Line2Length { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 5;

            canvas.DrawLine(Line1StartX, Line1StartY, Line1EndX, Line1EndY);

            float angleRadians = (float)((Line2AngleDegrees - 90) * Math.PI / 180.0);

            float line2EndX = Line1EndX + Line2Length * (float)Math.Cos(angleRadians);
            float line2EndY = Line1EndY + Line2Length * (float)Math.Sin(angleRadians);


            canvas.StrokeColor = Colors.Green;

            canvas.DrawLine(Line1EndX, Line1EndY, line2EndX, line2EndY);

            float staticAngle = 200;


            float randomAngle = (float)((staticAngle - 90 - (360 - Line2AngleDegrees)) * Math.PI / 180.0);
            float line3EndX = Line1EndX + Line2Length * (float)Math.Cos(randomAngle);
            float line3EndY = Line1EndY + Line2Length * (float)Math.Sin(randomAngle);

            canvas.StrokeColor = Colors.LightBlue;

            canvas.DrawLine(Line1EndX, Line1EndY, line3EndX, line3EndY);
        }
    }

    public partial class MainPage : ContentPage
    {
        GraphicsDrawable drawable;
        double compassAngle = 0; 

        public MainPage()
        {
            InitializeComponent();
            ToggleCompass();

            drawable = new GraphicsDrawable
            {
                Line1StartX = 150,
                Line1StartY = 50,
                Line1EndX = 150,
                Line1EndY = 150,
                Line2AngleDegrees = 0,
                Line2Length = 100
            };
            grafika.Drawable = drawable;
        }

        private void ToggleCompass()
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    Compass.Default.ReadingChanged += Compass_ReadingChanged;
                    Compass.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Compass.Default.Stop();
                    Compass.Default.ReadingChanged -= Compass_ReadingChanged;
                }
            }
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            CompassLabel.TextColor = Colors.Green;
            CompassLabel.Text = e.Reading.HeadingMagneticNorth+"";
            compassAngle = e.Reading.HeadingMagneticNorth;
            drawable.Line2AngleDegrees = (float)compassAngle;
            grafika.Invalidate();
        }
    }
}
