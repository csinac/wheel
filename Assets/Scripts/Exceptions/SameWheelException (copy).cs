namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class SameWheelException : System.Exception
    {
        public SameWheelException(string wheel1, string wheel2) : base(FormatMessage(wheel1, wheel2)) { }

        private static string FormatMessage(string wheel1, string wheel2) =>
            $"{wheel1} and {wheel2} are already on the same wheel.";
    }
}