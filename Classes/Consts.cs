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
    }
}
