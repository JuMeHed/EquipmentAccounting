using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;

namespace EquipmentAccounting.Classes
{
    internal class QRCodeGenerator
    {
        public static BitmapSource GenerateQRCode(string content)
        {
            try
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 128,
                        Height = 128
                    }
                };

                using (var bitmap = writer.Write(content))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Position = 0;
                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();
                        return bitmapImage;
                    }
                }
            }
            catch { return null; }
          
        }
        public static void PrintQRCode(BitmapSource QrCodeImage)
        {
            if (QrCodeImage != null)
            {
                PrintDialog printDialog = new PrintDialog();

                // Настройка параметров печати
                printDialog.PrintTicket.PageMediaSize = new PageMediaSize(5 * 96, 5 * 96); // Размер 5x5 дюймов, 96 DPI

                // Открыть диалоговое окно выбора принтера
                bool? print = printDialog.ShowDialog();
                if (print == true)
                {
                    // Создание визуального элемента для печати
                    DrawingVisual visual = new DrawingVisual();
                    using (DrawingContext drawingContext = visual.RenderOpen())
                    {
                        // Рисуем QR-код
                        drawingContext.DrawImage(QrCodeImage, new Rect(0, 0, QrCodeImage.PixelWidth, QrCodeImage.PixelHeight));
                    }

                    // Печать
                    printDialog.PrintVisual(visual, "QR Code Print");
                }
            }
            else
            {
                MessageBox.Show("QR-код не сгенерирован.");
            }
        }
    }
}
