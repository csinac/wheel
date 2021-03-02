namespace RectangleTrainer.WheelOfPseudoFortune.Exceptions
{
    public class WrongParameterCountException : System.Exception
    {
        public WrongParameterCountException(int count, params System.Type[] types) : base(FormatMessage(count, types)) { }

        private static string FormatMessage(int count, params System.Type[] types)
        {
            //var st = new StackTrace();
            //var sf = st.GetFrame(1);

            //return sf.GetMethod().Name;
            return $"Parameter count does not match the styling requirements (needs {count}: index (int), arc in degrees (float)";
        }
    }
}
