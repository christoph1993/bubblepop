using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace bubblepop
{
    public struct SKSvgElement
    {
        public SKPath Path;
        public SKPaint Fill;
        public SKPaint Stroke;
        public SKMatrix Transform;

        public static SKSvgElement ParseXElement(XElement p)
        {
            SKMatrix _matrix = SKMatrix.Identity;
            var fillAttribute = p.Attribute("fill")?.Value;
            var fill = string.IsNullOrEmpty(fillAttribute) || fillAttribute == "none" || !SKColor.TryParse(fillAttribute, out SKColor fillColour) ? SKColor.Empty : fillColour;
            var fillPaint = fill == SKColor.Empty ? null : new SKPaint { Color = fill };

            var strokeAttribute = p.Attribute("stroke")?.Value;
            var stroke = string.IsNullOrEmpty(strokeAttribute) || strokeAttribute == "none" || !SKColor.TryParse(strokeAttribute, out SKColor strokeColour) ? SKColor.Empty : strokeColour;
            var strokeWidth = float.TryParse(p.Attribute("stroke-width")?.Value, out float _width) ? _width : 0f;
            var strokePaint = stroke == SKColor.Empty ? null : new SKPaint
            {
                IsStroke = true,
                StrokeWidth = strokeWidth,
                Color = stroke
            };
            var transform = p.Attribute("transform")?.Value;
            if (transform != null)
            {
                var matches = Regex.Match(transform, Consts.SVGTransformRegexPattern);
                if (matches.Success)
                {
                    _matrix = matches.Groups[1].Value switch
                    {
                        "rotate" => matches.Groups[1].Value.CreateRotationMatrix(),
                        _ => SKMatrix.Identity
                    };
                }
            }

            var path = SKPath.ParseSvgPathData(p.Attribute("d")?.Value);

            return new SKSvgElement
            {
                Path = path,
                Fill = fillPaint,
                Stroke = strokePaint,
                Transform = _matrix
            };
        }
    }
}
