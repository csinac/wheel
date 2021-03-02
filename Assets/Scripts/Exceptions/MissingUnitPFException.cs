namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class MissingUnitPFException : System.Exception
    {
        public MissingUnitPFException(string name, System.Type type): base(FormatMessage(name, type)) { }

        private static string FormatMessage(string name, System.Type type)
            => $"Unit Prefab missing. Assign a unit prefab of type {type} to the {name} property.";
    }
}
