using System;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace WatermarkImages
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { "/Users/ra/Downloads/demo/fotos", "/Users/ra/Downloads/demo/vadeball-logo.jpg", "center", "20", "100" };
            // /Users/ra/Downloads/demo/fotos /Users/ra/Downloads/demo/vadeball-logo.jpg center 20 100
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Select directory and watermark file to use:" +
                        "\n  => mono WathermarkImages.exe FOTOS_DIR FILE_WATERMAKR POSITION SIZE MARGIN" +
                        "\n\n" +
                        "Help:" +
                        "\n * POSITION => Center, TopLeft, TopCenter, TopRight, BottomLeft, BottomCenter, BottomRight" +
                        "\n * SIZE => 10 to 70" +
                        "\n * MARGIN => pixel to margen laterial. Only form Left and Right postions." +
                        "\n\n");
                }

                var path = "";
                if (args.Length > 0)
                {
                    path = args[0];
                    Console.WriteLine("Directory: " + args[0]);
                }
                else
                {
                    Console.WriteLine("Select the directory: ");
                    path = Console.ReadLine();
                }

                var fileWatermark = "";
                if (args.Length > 1)
                {
                    fileWatermark = args[1];
                }
                else
                {
                    Console.WriteLine("Select Watermark file: ");
                    fileWatermark = Console.ReadLine();
                }

                var position = "center";

                if (args.Length > 2)
                    position = args[2].ToLower();
                else
                {
                    Console.WriteLine("What postion (center,topleft,topcenter,topright,bottomleft,bottomcenter,bottomright)? ");
                    position = Console.ReadLine();
                }

                var size = 0;
                try
                {
                    if (args.Length > 3)
                        size = Convert.ToInt32(args[3]);
                    else
                    {
                        Console.WriteLine("Select a size in % from 10 to 70: ");
                        size = Convert.ToInt32(Console.ReadLine());
                    }
                }
                catch //(Exception ex)
                {
                    Console.WriteLine("Check Size value.");
                }


                var margin = 0;
                try
                {
                    if (args.Length > 4)
                        margin = Convert.ToInt32(args[4]);
                    else
                    {
                        Console.WriteLine("Select a margin, 0 by defatul: ");
                        size = Convert.ToInt32(Console.ReadLine());
                    }
                }
                catch //(Exception ex)
                {
                    Console.WriteLine("Check Margin value.");
                }

                var files = Directory.GetFiles(path);
                if (files.Length == 0)
                {
                    Console.WriteLine("This folther don't have images.");
                }
                else
                {
                    Console.WriteLine("Total fotos to process: " + files.Length);
                    /*  Console.WriteLine("Write 'y' to continue. ");
                      if (Console.ReadLine() != "y")
                      {
                          return;
                      }*/
                }
                var watermark = new Bitmap(Image.FromFile(fileWatermark));

                for (var f = 0; f < files.Length; f++)
                    WatermarkAdd(files[f], watermark, position, size, margin);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        private static void WatermarkAdd(string file, Bitmap watermark, string position, int size, int margin)
        {
            var x = 0;
            var y = 0;

            var imagen = Image.FromFile(file);

            if (size < 10)
                size = 10;
            if (size > 70)
                size = 70;

            var w = 0;
            var h = 0;

            switch (position)
            {
                default:
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, (imagen.Width - w) / 2, (imagen.Height - h) / 2, w, h);
                    }
                    break;
                case "topleft":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        gr.DrawImage(watermark, x + margin, y + margin, imagen.Width * size / 100, imagen.Height * size / 100);
                    }
                    break;
                case "topcenter":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, (imagen.Width - w) / 2, y + margin, w, h);
                    }
                    break;
                case "topright":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, imagen.Width - w - margin, y + margin, w, h);
                    }
                    break;
                case "bottomleft":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, x + margin, imagen.Height - h - margin, w, h);
                    }
                    break;
                case "bottomcenter":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, (imagen.Width - w) / 2, imagen.Height - h - margin, w, h);
                    }
                    break;
                case "bottomright":
                    using (Graphics gr = Graphics.FromImage(imagen))
                    {
                        w = imagen.Width * size / 100;
                        h = imagen.Height * size / 100;
                        gr.DrawImage(watermark, imagen.Width - w - margin, imagen.Height - h - margin, w, h);
                    }
                    break;
            }

            SaveImage(imagen, file);

        }

        private static void SaveImage(Image image, string file)
        {
            string extension = Path.GetExtension(file);
            var fileName = Path.GetFileName(file);
            var dirName = Path.GetDirectoryName(file);
            var fileout = Path.Combine(dirName, "wwm");
            if (!Directory.Exists(fileout))
                Directory.CreateDirectory(fileout);
            fileout = Path.Combine(fileout, fileName);

            switch (extension.ToLower())
            {
                case ".bmp":
                    image.Save(fileout, ImageFormat.Bmp);
                    break;
                case ".exif":
                    image.Save(fileout, ImageFormat.Exif);
                    break;
                case ".gif":
                    image.Save(fileout, ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                    image.Save(fileout, ImageFormat.Jpeg);
                    break;
                case ".png":
                    image.Save(fileout, ImageFormat.Png);
                    break;
                case ".tif":
                case ".tiff":
                    image.Save(fileout, ImageFormat.Tiff);
                    break;
                default:
                    throw new NotSupportedException(
                        "Unknown file extension " + extension);
            }
        }
    }
}
