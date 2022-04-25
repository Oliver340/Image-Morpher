using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace _4932_Asn1_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPoint = new Point();
        Line line1 = new Line();
        Line line2 = new Line();
        SolidColorBrush redBrush = new SolidColorBrush();
        Line? selectedLine = null;
        List<PairedLines> pairedLines = new List<PairedLines>();

        public MainWindow()
        {
            InitializeComponent();
        }


        // CANVAS 1 CLICK
        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas1);
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                drawDrag(canvas1, line1, e);
            }
        }

        private void canvas1_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkLength(canvas1, e))
            {
                return;
            }
            PairedLines pair = new PairedLines();
            pair.setLeft(drawLine(canvas1, canvas1, e));
            pair.setRight(drawLine(canvas1, canvas2, e));
            pairedLines.Add(pair);
            canvas1.Children.Remove(line1);
        }


        // CANVAS 2 CLICK
        private void canvas2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas2);
        }

        private void canvas2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                drawDrag(canvas2, line2, e);
            }
        }

        private void canvas2_MouseUp(object sender, MouseEventArgs e)
        {
            if (checkLength(canvas2, e))
            {
                return;
            }
            PairedLines pair = new PairedLines();
            pair.setLeft(drawLine(canvas2, canvas1, e));
            pair.setRight(drawLine(canvas2, canvas2, e));
            pairedLines.Add(pair);
            canvas2.Children.Remove(line2);
        }

        // HELPER FUNCTIONS
        private void drawDrag(Canvas currentCanvas, Line drawingLine, MouseEventArgs e)
        {
            currentCanvas.Children.Remove(drawingLine);
            if (checkLength(currentCanvas, e))
            {
                return;
            }
            redBrush.Color = Colors.Red;
            drawingLine.Stroke = redBrush;

            drawingLine.X1 = startPoint.X;
            drawingLine.Y1 = startPoint.Y;
            drawingLine.X2 = e.GetPosition(currentCanvas).X;
            drawingLine.Y2 = e.GetPosition(currentCanvas).Y;

            currentCanvas.Children.Add(drawingLine);
        }

        private bool checkLength(Canvas currentCanvas, MouseEventArgs e)
        {
            double xlength = e.GetPosition(currentCanvas).X - startPoint.X;
            double ylength = e.GetPosition(currentCanvas).Y - startPoint.Y;
            if (Math.Sqrt(xlength * xlength + ylength * ylength) < 10 || selectedLine != null)
            {
                return true;
            }
            return false;
        }

        private Line drawLine(Canvas currentCanvas, Canvas selectedCanvas, MouseEventArgs e)
        {
            Line line = new Line();
            line.Stroke = redBrush;
            line.StrokeThickness = 2;
            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = e.GetPosition(currentCanvas).X;
            line.Y2 = e.GetPosition(currentCanvas).Y;

            selectedCanvas.Children.Add(line);
            line.MouseDown += new MouseButtonEventHandler(line_MouseDown);
            line.MouseMove += new MouseEventHandler(line_MouseMove);

            return line;
        }

        // LINE SELECT
        Ellipse ellipse1;
        Ellipse ellipse2;
        Canvas currentCanvas;
        Point lineOriginalPos1;
        Point lineOriginalPos2;
        Point clickStartPos;
        private void line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentCanvas = (Canvas)VisualTreeHelper.GetParent((Line)sender);
            if (selectedLine == ((Line)sender))
            {
                ((Line)sender).Stroke = Brushes.Red;
                selectedLine = null;
                currentCanvas.Children.Remove(ellipse1);
                currentCanvas.Children.Remove(ellipse2);
            } else
            {
                ((Line)sender).Stroke = Brushes.Blue;
                selectedLine = (Line)sender;
                addEllipse();
            }
        }

        private void addEllipse()
        {
            ellipse1 = new Ellipse();
            ellipse1.Fill = Brushes.Black;
            ellipse1.Width = 5;
            ellipse1.Height = 5;
            ellipse1.Margin = new Thickness(selectedLine.X1 - 2.5, selectedLine.Y1 - 2.5, 0, 0);
            ellipse2 = new Ellipse();
            ellipse2.Fill = Brushes.Green;
            ellipse2.Width = 5;
            ellipse2.Height = 5;
            ellipse2.Margin = new Thickness(selectedLine.X2 - 2.5, selectedLine.Y2 - 2.5, 0, 0);

            if (VisualTreeHelper.GetParent(selectedLine) == canvas1)
            {
                canvas1.Children.Add(ellipse1);
                canvas1.Children.Add(ellipse2);
            }
            else
            {
                canvas2.Children.Add(ellipse1);
                canvas2.Children.Add(ellipse2);
            }
            ellipse1.MouseMove += new MouseEventHandler(ellipse1_MouseMove);
            ellipse2.MouseMove += new MouseEventHandler(ellipse2_MouseMove);
        }
        private void ellipse1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedLine != null && e.LeftButton == MouseButtonState.Pressed)
            {
                moveEllipse(ellipse1, currentCanvas, e);
            }
        }
        private void ellipse2_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedLine != null && e.LeftButton == MouseButtonState.Pressed)
            {
                moveEllipse(ellipse2, currentCanvas, e);
            }
        }

        private void moveEllipse(Ellipse ellipse, Canvas canvas, MouseEventArgs e)
        {
            if (ellipse == ellipse1)
            {
                selectedLine.X1 = e.GetPosition(canvas).X;
                selectedLine.Y1 = e.GetPosition(canvas).Y;
                ellipse.Margin = new Thickness(selectedLine.X1 - 2.5, selectedLine.Y1 - 2.5, 0, 0);
            } else if (ellipse == ellipse2)
            {
                selectedLine.X2 = e.GetPosition(canvas).X;
                selectedLine.Y2 = e.GetPosition(canvas).Y;
                ellipse.Margin = new Thickness(selectedLine.X2 - 2.5, selectedLine.Y2 - 2.5, 0, 0);
            }
        }

        private void line_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedLine != null && e.LeftButton == MouseButtonState.Pressed)
            {
                clickStartPos = e.GetPosition(currentCanvas);
                lineOriginalPos1 = new Point(selectedLine.X1, selectedLine.Y1);
                lineOriginalPos2 = new Point(selectedLine.X2, selectedLine.Y2);
                currentCanvas = (Canvas)VisualTreeHelper.GetParent((Line)sender);
                DragDrop.DoDragDrop(selectedLine, selectedLine, DragDropEffects.Move);
            }
        }

        private void canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(currentCanvas);
            selectedLine.X1 = lineOriginalPos1.X + (dropPosition.X - clickStartPos.X);
            selectedLine.X2 = lineOriginalPos2.X + (dropPosition.X - clickStartPos.X);
            selectedLine.Y1 = lineOriginalPos1.Y + (dropPosition.Y - clickStartPos.Y);
            selectedLine.Y2 = lineOriginalPos2.Y + (dropPosition.Y - clickStartPos.Y);
            ellipse1.Margin = new Thickness(selectedLine.X1 - 2.5, selectedLine.Y1 - 2.5, 0, 0);
            ellipse2.Margin = new Thickness(selectedLine.X2 - 2.5, selectedLine.Y2 - 2.5, 0, 0);
        }



        // LOAD BUTTONS
        static double xResolution = 400;
        static double yResolution = 400;
        private void btn1Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image1.Source = new BitmapImage(new Uri(op.FileName));
                image1.Width = xResolution;
                image1.Height = yResolution;
                BitmapSource bs = image1.Source as BitmapSource;
                ScaleTransform st = new ScaleTransform(xResolution / bs.PixelWidth, yResolution / bs.PixelHeight);
                image1.Source = new TransformedBitmap(image1.Source as BitmapSource, st);
            }
        }

        private void btn2Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image2.Source = new BitmapImage(new Uri(op.FileName));
                image2.Width = xResolution;
                image2.Height = yResolution;
                BitmapSource bs = image2.Source as BitmapSource;
                ScaleTransform st = new ScaleTransform(xResolution / bs.PixelWidth, yResolution / bs.PixelHeight);
                image2.Source = new TransformedBitmap(image2.Source as BitmapSource, st);
            }
        }

        // Morph Button
        MorphWindow morphWindow;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            morphWindow = new MorphWindow();
            morphWindow.Show();
            morphWindow.morphImage.Source = image1.Source;

            if (int.TryParse(inputFrames.Text, out int value))
            {
                int numFrames = Int32.Parse(inputFrames.Text);
                generateBitMaps(numFrames);
            }
        }

        private Vector2 performWeights(Vector2 T, int numLines, List<PairedLines> linePairs)
        {
            double[] weights = new double[numLines];
            Vector2[] deltaXs = new Vector2[numLines];
            Vector2[] tPrimes = new Vector2[numLines];
            for (int i = 0; i < numLines; i++)
            {
                tPrimes[i] = linePairs[i].calcXPrime(T);
                weights[i] = linePairs[i].calcWeight();
                deltaXs[i] = linePairs[i].calcDeltaX();
            }
            double x = 0;
            double y = 0;
            double totalWeights = 0;
            for (int i = 0; i < numLines; i++)
            {
                x += weights[i] * deltaXs[i].X;
                y += weights[i] * deltaXs[i].Y;
                totalWeights += weights[i];
            }
            Vector2 newT = new Vector2((float)(T.X + (x / totalWeights)), (float)(T.Y + (y / totalWeights)));
            return newT;
        }

        private byte[] goThoughPixels(List<PairedLines> linePairs, BitmapSource bmp)
        {
            int height = bmp.PixelHeight;
            int width = bmp.PixelWidth;
            int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);
            byte[] bits = new byte[height * stride];
            bmp.CopyPixels(bits, stride, 0);
            Pixel[] pixels = new Pixel[bits.Length / 4];
            int count = 0;
            for (int i = 0; i < bits.Length; i += 4)
            {
                pixels[count++] = new Pixel(bits[i], bits[i + 1], bits[i + 2], bits[i + 3]);
            }
            byte[] morphedPixels = new byte[bits.Length];
            int count2 = 0;


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //Vector2 location = linePairs[0].calcXPrime(new Vector2(j, i));
                    Vector2 location = performWeights(new Vector2(j, i), linePairs.Count(), linePairs);
                    location.X = (int)Math.Clamp(location.X, 0, width - 1);
                    location.Y = (int)Math.Clamp(location.Y, 0, height - 1);
                    count = (int)((location.X) + (location.Y * width));
                    morphedPixels[count2++] = pixels[count].B;
                    morphedPixels[count2++] = pixels[count].G;
                    morphedPixels[count2++] = pixels[count].R;
                    morphedPixels[count2++] = pixels[count].A;
                }
            }
            return morphedPixels;
        }

        private void generateBitMaps(int numFrames)
        {
            var bmp1 = image1.Source as BitmapSource;
            var bmp2 = image2.Source as BitmapSource;

            List<byte[]> forwardFrameBMP = new List<byte[]>();
            List<List<PairedLines>> forwardFramesPairedLines = calculateForwardPairedLines(numFrames);
            List<byte[]> reverseFrameBMP = new List<byte[]>();
            List<List<PairedLines>> reverseFramesPairedLines = calculateReversePairedLines(numFrames);
            
            for (int i = 0; i < numFrames; i++)
            {
                forwardFrameBMP.Add(goThoughPixels(forwardFramesPairedLines[i], bmp1));
                reverseFrameBMP.Add(goThoughPixels(reverseFramesPairedLines[i], bmp2));
            }

            List<byte[]> crossDissolvedBMP = new List<byte[]>();
            for (int i = 0; i < numFrames; i++)
            {
                crossDissolvedBMP.Add(getCrossDissolvePixels(forwardFrameBMP[i], reverseFrameBMP[i], i, numFrames));
            }

            morphWindow.writeToImage(crossDissolvedBMP[0]);
            morphWindow.initFrameData(crossDissolvedBMP, numFrames);
        }

        private byte[] getCrossDissolvePixels(byte[] leftImage, byte[] rightImage, int frameNo, int numFrames)
        {
            byte[] crossDissolvedPixels = new byte[leftImage.Length];
            double percentageLeft = 1 - ((double)frameNo / (numFrames - 1));
            double percentageRight = (double)frameNo / (numFrames - 1);
            for (int i = 0; i < leftImage.Length; i++)
            {
                crossDissolvedPixels[i] = (byte)(leftImage[i] * percentageLeft + rightImage[i] * percentageRight);
            }
            return crossDissolvedPixels;
        }

        private List<List<PairedLines>> calculateForwardPairedLines(int numOfFrames)
        {
            List<List<PairedLines>> framesPairedLines = new List<List<PairedLines>>();
            for (int i = 0; i < numOfFrames; i++)
            {
                List<PairedLines> linePairs = new List<PairedLines>();
                for (int j = 0; j < pairedLines.Count; j++)
                {
                    Line change = calculateChangeInLine(pairedLines[j].leftLine, pairedLines[j].rightLine, numOfFrames);
                    linePairs.Add(new PairedLines());
                    linePairs[j].leftLine = pairedLines[j].leftLine;
                    linePairs[j].rightLine = getNextFrame(change, pairedLines[j].leftLine, i);

                }
                framesPairedLines.Add(linePairs);
            }
            return framesPairedLines;
        }
        private List<List<PairedLines>> calculateReversePairedLines(int numOfFrames)
        {
            List<List<PairedLines>> framesPairedLines = new List<List<PairedLines>>();
            for (int i = 0; i < numOfFrames; i++)
            {
                List<PairedLines> linePairs = new List<PairedLines>();
                for (int j = 0; j < pairedLines.Count; j++)
                {
                    Line change = calculateChangeInLine(pairedLines[j].rightLine, pairedLines[j].leftLine, numOfFrames);
                    linePairs.Add(new PairedLines());
                    linePairs[j].leftLine = pairedLines[j].rightLine;
                    linePairs[j].rightLine = getNextFrame(change, pairedLines[j].rightLine, i);

                }
                framesPairedLines.Insert(0, linePairs);
            }
            return framesPairedLines;
        }

        private Line calculateChangeInLine(Line start, Line end, int numFrames)
        {
            Line distance = new Line();
            distance.X1 = end.X1 - start.X1;
            distance.Y1 = end.Y1 - start.Y1;
            distance.X2 = end.X2 - start.X2;
            distance.Y2 = end.Y2 - start.Y2;
            int step = numFrames - 1;
            distance.X1 /= step;
            distance.Y1 /= step;
            distance.X2 /= step;
            distance.Y2 /= step;
            return distance;
        }

        private Line getNextFrame(Line distance, Line start, int frameNo)
        {
            Line newLine = new Line();
            newLine.X1 = start.X1;
            newLine.Y1 = start.Y1;
            newLine.X2 = start.X2;
            newLine.Y2 = start.Y2;
            for (int i = 0; i < frameNo; i++)
            {
                newLine = addLines(newLine, distance);
            }
            return newLine;
        }

        private Line addLines(Line first, Line second)
        {
            first.X1 += second.X1;
            first.X2 += second.X2;
            first.Y1 += second.Y1;
            first.Y2 += second.Y2;
            return first;
        }
    }
}
