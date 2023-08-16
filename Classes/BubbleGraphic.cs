using Microsoft.Maui.Graphics.Text;

namespace bubblepop
{
    public partial class BubbleGraphics : View, IDrawable
    {
        private static Random _random = new Random();
        private static double offSet = (double)Consts.ScreenHeight * 0.005;

        private bool isUpdating = false;


        #region Private Properties
        private List<Bubble> Bubbles = new();
        #endregion

        #region Initialisers
        public BubbleGraphics()
        {
            
        }
        #endregion
        
        #region Private Methods
        //private void AnimateBounce()
        //{
        //    if (animationProgress <= 1f)
        //    {
        //        double yOffset = (double)Math.Sin(animationProgress * Math.PI) * 50; // Adjust the amplitude of the bounce
        //        BubbleCenter = new PointF(BubbleCenter.X, originalY + (float)yOffset); // Update the bubble's Y coordinate

        //        animationProgress += 0.02f; // Increase the animation progress
        //    }
        //    else
        //    {
        //        isAnimating = false;
        //    }
        //}

        //private void DrawAnimatedBubble(ICanvas canvas)
        //{
        //    float scaleFactor = 1f - animationProgress;
        //    var bubbleRect = new Rect(
        //        BubbleCenter.X - BubbleRadius * scaleFactor,
        //        BubbleCenter.Y - BubbleRadius * scaleFactor,
        //        BubbleRadius * 2 * scaleFactor,
        //        BubbleRadius * 2 * scaleFactor
        //    );

        //    animationProgress += 0.02f; // Increase the animation progress
        //    if (animationProgress >= 1f)
        //    {
        //        isAnimating = false;
        //        animationProgress = 0f;
        //    }
        //}

        //private void OnBubbleTapped(object sender, TouchEventArgs e)
        //{
        //    if (!isAnimating)
        //    {
        //        BubbleRadius *= 0.2f;
        //        BubbleCenter = new PointF(BubbleCenter.X + BubbleRadius * 0.5f, BubbleCenter.Y + BubbleRadius * 0.5f);
        //        isAnimating = true;
        //    }
        //}

        private double DistanceSquared(Bubble b, Point p) => (p.X - b.BubbleCenter.X) * (p.X - b.BubbleCenter.X) + (p.Y - b.BubbleCenter.Y) * (p.Y - b.BubbleCenter.Y);
        #endregion

        #region Public Methods
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.ResetState();

            canvas.StrokeColor = Colors.DeepSkyBlue;
            canvas.StrokeSize = 2;
            canvas.FillColor = Colors.LightSkyBlue;

            foreach (var bubble in Bubbles)
            {
                if (bubble.BubbleState == BubbleState.Floating)
                {
                    canvas.DrawCircle(bubble.BubbleCenter, bubble.BubbleRadius);
                    canvas.FillCircle(bubble.BubbleCenter, bubble.BubbleRadius);
                }
                else if (bubble.BubbleState == BubbleState.Popping)
                {
                    canvas.DrawCircle(bubble.BubbleCenter, bubble.PoppingBubbleRadius);
                    canvas.FillCircle(bubble.BubbleCenter, bubble.PoppingBubbleRadius);
                }
                
            }
        }

        public void Bounce() { }

        public void AddBubble()
        {
            if (isUpdating)
                return;
            var spawn = _random.Next(Consts.ScreenWidth);
            PointF center = new PointF(spawn, Consts.ScreenHeight - (spawn % 100));
            var bubble = new Bubble(center, Consts.DefaultBubbleRadius);
            Bubbles.Add(bubble);
        }

        public void PopBubble(Point point)
        {
            var _bubbles = Bubbles.Where(b => b.ContainsPoint(point)).ToList();
            foreach(var bubble in _bubbles)
                bubble.BubbleState = BubbleState.Popping;
        }

        public void UpdateBubbles(bool ascending = true)
        {
            isUpdating = true;
            List<Bubble> removeBubbles = new();
            
            int yOffset = (ascending ? -1 : 1) * (int)(Math.Round(offSet, 0, MidpointRounding.AwayFromZero)); // Change in Y coordinate
            foreach(var bubble in Bubbles)
            {
                if (bubble.BubbleState == BubbleState.Floating)
                    bubble.BubbleCenter = new PointF(bubble.BubbleCenter.X, bubble.BubbleCenter.Y + yOffset);
                bubble.UpdateBubbleState();

                if (bubble.BubbleState == BubbleState.Gone || bubble.BubbleState == BubbleState.Popped)
                    removeBubbles.Add(bubble);
            }
            Bubbles.RemoveAll(b => removeBubbles.Contains(b));
            isUpdating = false;
        }
        #endregion
    }
}