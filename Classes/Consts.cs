using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;

namespace bubblepop
{
    public static class Consts
    {
#if WINDOWS
        public static readonly int ScreenHeight = (int)(new MauiWinUIWindow()).Bounds.Height;
        public static readonly int ScreenWidth = (int)(new MauiWinUIWindow()).Bounds.Width;
#else
        public static readonly int ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
        public static readonly int ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;
#endif

        public static readonly float DefaultBubbleRadius = 20f;
        public static readonly int MaxBubblePopStep = 5;
        public static readonly int DefaultImageWidth = 128;
        public static readonly int DefaultImageHeight = 128;

        public static readonly string BubbleImagePath = "Resources\\Bubble";
        public static readonly string BubbleImageFile = "bubble-fill.svg";
        public static readonly string BubblePoppedImageFile = "bubble-popped.svg";

        public static readonly string SVGTransformRegexPattern = @"(\W+)\(.*\)$";
    }
}
