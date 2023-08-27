using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using SkiaSharp.Extended.Svg;

namespace bubblepop
{
    public static class GenerateBubble
    {
        public static bool HaveGeneratedBubbles => Path.Exists(Path.Join(Consts.BubbleImagePath, Consts.BubblePoppedImageFile));

        public static void GenerateBubbleRing(int childBubbles = 7)
        {
            (int width, int height) = (-1, -1);
            
            var outImagePath = Path.Join(FileSystem.AppDataDirectory, Consts.BubblePoppedImageFile);
            var inImagePath = Path.Join(AppContext.BaseDirectory, Consts.BubbleImageFile);
            var increment = 360 / (float)childBubbles;
            var scale = childBubbles / 100.0f;
            using var stream = new MemoryStream();

            SKRect bounds = SKRect.Create(Consts.DefaultImageWidth, Consts.DefaultImageHeight);
            string bubbleSvgString = File.ReadAllText(inImagePath);
            var bubbleXml = XDocument.Parse(bubbleSvgString);
            var bubblePaths = bubbleXml.Document.Root.Descendants().Where(p => p.Name.LocalName == "path").ToList();
            var bubbleSkiaPaths = bubblePaths.Select(p => SKPath.ParseSvgPathData(p.Attribute("d").Value)).ToList();
            var svg = new SkiaSharp.Extended.Svg.SKSvg();
            svg.Load(inImagePath);
            if (svg == null)
                throw new NullReferenceException(nameof(svg));

            width = (int)svg.CanvasSize.Width;
            height = (int)svg.CanvasSize.Width;

            var radius = width / 2;

            var scaleX = (int)Math.Ceiling((double)(width * scale));
            var scaleY = (int)Math.Ceiling((double)(height * scale));

            using (var bitmap = SKBitmap.FromImage(SKImage.FromPicture(svg.Picture, new SKSizeI(width, height))))
            using (var resizedBitmap = bitmap.Resize(new SKImageInfo(scaleX, scaleY), SKFilterQuality.High))
            using (var canvas = SKSvgCanvas.Create(bounds, stream))
            {
                {
                    for (int i = 0; i < childBubbles; i++)
                    {
                        float offsetRadians = i * increment * (float)(Math.PI / 180);
                        float xOffset = radius * (float)Math.Cos(offsetRadians);
                        float yOffset = radius * (float)Math.Sin(offsetRadians);
                    }
                }
                canvas.Flush();
            }

            stream.Position = 0;

            using (var reader = new StreamReader(stream))
            {
                var xml = reader.ReadToEnd();
                var xdoc = XDocument.Parse(xml);
                File.WriteAllText(outImagePath, xdoc.ToString());
            }
        }
    }
}
