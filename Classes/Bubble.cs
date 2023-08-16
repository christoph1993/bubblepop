namespace bubblepop
{
    public class Bubble
    {
        public Bubble(PointF center, float radius)
        {
            BubbleCenter = center;
            BubbleRadius = radius;
            BubbleState = BubbleState.Floating;
        }

        #region Public Properties
        public PointF BubbleCenter { get; set; }
        public float BubbleRadius { get; set; }
        public BubbleState BubbleState { get; set; }
        public int BubblePopStep { get; private set; } = 0;

        public float PoppingBubbleRadius => BubbleRadius * (1 + BubblePopStep / 10f);
        #endregion

        #region Public Methods
        public void UpdateBubbleState()
        {
            switch (BubbleState)
            {
                case BubbleState.Floating:
                    float minX = BubbleCenter.X + BubbleRadius;
                    float maxX = BubbleCenter.X - BubbleRadius;
                    float minY = BubbleCenter.Y + BubbleRadius;
                    float maxY = BubbleCenter.Y - BubbleRadius;

                    if (minX < 0 || maxX > Consts.ScreenWidth || minY < 0 || maxY > Consts.ScreenHeight)
                        BubbleState = BubbleState.Gone;
                    break;
                case BubbleState.Popping:
                    if (BubblePopStep == Consts.MaxBubblePopStep)
                        BubbleState = BubbleState.Popped;
                    else
                        BubblePopStep++; 
                    break;
            }
        }

        public bool ContainsPoint(Point point)
        {
            var boundingBox = new Rect(
                BubbleCenter.X - BubbleRadius,
                BubbleCenter.Y - BubbleRadius,
                BubbleRadius * 2,
                BubbleRadius * 2);
            return boundingBox.Contains(point);
        }
        #endregion
    }
}
