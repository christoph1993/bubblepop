using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bubblepop
{
    public static class Utils
    {
        public static SKMatrix CreateRotationMatrix(this string rotation)
        {

            return SKMatrix.Identity;
        }

        public static double Scale(this double initial, double min, double max, double bound) => 1 + (initial - min) * (bound - 1) / (max - min);
        public static float Scale(this float initial, float min, float max, float bound) => 1 + (initial - min) * (bound - 1) / (max - min);
    }
}
