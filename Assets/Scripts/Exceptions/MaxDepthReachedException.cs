namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class MaxDepthReachedException : System.Exception
    {
        public MaxDepthReachedException(string parentWheel, string childWheel) : base(FormatMessage(parentWheel, childWheel)) { }

        private static string FormatMessage(string parent, string child) =>
            $"Cannot add {child} to {parent}, because it exceeds the depth limit.";
    }
}