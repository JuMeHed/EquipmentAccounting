using System.IO;
using System.Windows.Media.Imaging;
using ZXing;

namespace EquipmentAccounting.Classes
{
    internal class QRCodeGenerator
    {
        public static BitmapSource GenerateQRCode(string content)
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
    }
}
