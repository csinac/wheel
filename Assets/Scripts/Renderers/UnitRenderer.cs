namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class UnitRenderer: UnityEngine.MonoBehaviour
    {
        virtual public void Highlight(bool blinking)
        {
            UnityEngine.Debug.LogWarning($"Highlight unit {gameObject.name}. Override this function for the custom highlighting effect of your unit renderer.");
        }
        virtual public void Dehighlight()
        {
            UnityEngine.Debug.LogWarning($"Dehighlight unit {gameObject.name}. Override this function for disabling the custom highlight effect of your unit renderer.");
        }

        public void Toggle(bool state)
        {
            UnityEngine.Debug.LogWarning($"Toggle unit {gameObject.name}. Override this function with the \"new\" keyword  for a hide / unhide effect.");
        }
        abstract public void SetContent<T>(T content);
    }
}