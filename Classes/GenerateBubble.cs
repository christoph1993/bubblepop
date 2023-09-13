using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using SkiaSharp.Extended.Svg;
using SkiaSharp.Views.Maui;

namespace bubblepop
{
    public static class GenerateBubble
    {
        public static bool HaveGeneratedBubbles => Path.Exists(Path.Join(Consts.BubbleImagePath, Consts.BubblePoppedImageFile));

        public static void GenerateBubbleRing(int childBubbles = 10)
        {
            (int width, int height) = (-1, -1);
            
            var outImagePath = Path.Join(FileSystem.AppDataDirectory, Consts.BubblePoppedImageFile);
            var inImagePath = Path.Join(AppContext.BaseDirectory, Consts.BubbleImageFile);
            var increment = 360 / (float)childBubbles;
            var scale = childBubbles / 100.0f;

            string bubbleSvgString = File.ReadAllText(inImagePath);
            var bubbleXml = XDocument.Parse(bubbleSvgString);
            var bubblePaths = bubbleXml.Document.Root.Descendants().Where(p => p.Name.LocalName == "path").ToList();
            var bubbleSkiaPaths = bubblePaths.Select(p => SKSvgElement.ParseXElement(p)).ToList();
            var _svg = new SkiaSharp.Extended.Svg.SKSvg();
            _svg.Load(inImagePath);
            if (_svg == null)
                throw new NullReferenceException(nameof(_svg));

            width = (int)_svg.CanvasSize.Width;
            height = (int)_svg.CanvasSize.Width;

            var radius = width / 2;

            List<XElement> xPaths = new();

            var scaleX = (int)(Math.Ceiling((double)(width * scale)).Scale(width * scale, width, Consts.DefaultImageWidth / 2));
            var scaleY = (int)Math.Ceiling((double)(height * scale)).Scale(height * scale, height, Consts.DefaultImageHeight / 2);
            for (int i = 0; i < childBubbles; i++)
            {
                float offsetRadians = i * increment * (float)(Math.PI / 180);
                float xOffset = (radius * (float)Math.Cos(offsetRadians)).Scale(0, radius, Consts.DefaultImageWidth / 2);
                float yOffset = (radius * (float)Math.Sin(offsetRadians)).Scale(0, radius, Consts.DefaultImageHeight / 2);
                bubbleSkiaPaths.ForEach(path =>
                {
                    path.Path.Offset(xOffset, yOffset);
                    path.Path.Transform(SKMatrix.CreateScale(scaleX, scaleY));
                    if (path.Transform != SKMatrix.Identity)
                        path.Path.Transform(path.Transform);
                    path.Path.Simplify();
                    var xPath = new XElement("path", new XAttribute("d", path.Path.ToSvgPathData()));
                    if (path.Fill != null)
                        xPath.Add(new XAttribute("fill", path.Fill.Color.ToMauiColor().ToArgbHex()));
                    if (path.Stroke != null)
                    {
                        xPath.Add(new XAttribute("stroke", path.Stroke.Color.ToMauiColor().ToArgbHex()));
                        xPath.Add(new XAttribute("stroke-width", path.Stroke.StrokeWidth.ToString()));
                    }
                    xPaths.Add(xPath);
                });
            }

            XNamespace svg = "http://www.w3.org/2000/svg";
            XNamespace xlink = "http://www.w3.org/1999/xlink";
            var xSvg = new XElement(
                svg + "svg",
                new XAttribute("xmlns", svg.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "xlink", xlink.NamespaceName),
                new XAttribute("viewBox", "-360 -360 1024 1024"),
                new XAttribute("width", "1024"),
                new XAttribute("height", "1024"));
            xSvg.Add(xPaths.ToArray());

            var outSvg = xSvg.ToString();
            outSvg = outSvg.Replace("xmlns=\"\"", String.Empty);

            File.WriteAllText(outImagePath, outSvg);
        }
    }
}
