namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class ZeroChanceException : System.Exception
    {
        public ZeroChanceException() : base("Wheel cannot have zero chance") { }
    }
}