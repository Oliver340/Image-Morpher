using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _4932_Asn1_WPF
{
    /// <summary>
    /// Interaction logic for MorphWindow.xaml
    /// </summary>
    public partial class MorphWindow : Window
    {
        public MorphWindow()
        {
            InitializeComponent();
        }

        int frameIndex = 0;
        List<byte[]> frameBMP;
        int totalFrames;
        public void initFrameData(List<byte[]> fBMP, int tF)
        {
            frameBMP = fBMP;
            totalFrames = tF;
        }
        // Previous Frame Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            frameIndex--;
            if (frameIndex < 0)
            {
                frameIndex = totalFrames - 1;
            }
            writeToImage(frameBMP[frameIndex]);
        }

        // Next Frame Button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            frameIndex++;
            if (frameIndex >= totalFrames)
            {
                frameIndex = 0;
            }
            writeToImage(frameBMP[frameIndex]);
        }

        async private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            frameIndex = 0;
            while (frameIndex < totalFrames)
            {
                await Task.Delay(50);
                writeToImage(frameBMP[frameIndex++]);

            }
        }

        public void writeToImage(byte[] morphedPixels)
        {
            var bmp = morphImage.Source as BitmapSource;
            int height = bmp.PixelHeight;
            int width = bmp.PixelWidth;
            int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);
            WriteableBitmap bmp2 = new WriteableBitmap(bmp);
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            bmp2.WritePixels(rect, morphedPixels, stride, 0);
            morphImage.Source = bmp2;
        }
    }
}
