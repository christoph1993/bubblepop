using Microsoft.Maui.Controls;

namespace bubblepop.Views;

public class BubbleView : GraphicsView
{
	private readonly Random _random = new Random();
	private readonly double _bubbleProbability = 0.075;

    public BubbleView()
	{
		Drawable = new BubbleGraphics();
		Dispatcher.StartTimer(TimeSpan.FromMilliseconds(30), Loop);

		WidthRequest = Consts.ScreenWidth;
		HeightRequest = Consts.ScreenHeight;

		var tap = new TapGestureRecognizer();
		tap.Tapped += OnBubbleTap;

		GestureRecognizers.Add(tap);
	}

	public bool Loop() 
	{
		if (_random.NextDouble() < _bubbleProbability)
			((BubbleGraphics)Drawable).AddBubble();
        ((BubbleGraphics)Drawable).UpdateBubbles();
        Invalidate();
		return true;
	}

	void OnBubbleTap(object sender, TappedEventArgs e)
	{
		var point = e.GetPosition(this);
		if (!point.HasValue)
			return;
		((BubbleGraphics)Drawable).PopBubble(point.Value);
	}
}