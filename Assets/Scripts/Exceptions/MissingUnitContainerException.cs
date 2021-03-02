namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class MissingUnitContainerException : System.Exception
    {
        public MissingUnitContainerException(string name) : base(FormatMessage(name)) { }

        private static string FormatMessage(string name) =>
            $"Unit Container missing. Make sure the object \"{name}\" has a child game object for containing wheel units and that it is assigned to 'unitContainer'";
    }
}