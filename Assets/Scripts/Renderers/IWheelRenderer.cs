using RectangleTrainer.WheelOfPseudoFortune.Model;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public interface IWheelRenderer
    {
        bool ResetView();
        void SetWheel(WheelBase wheel);
        void DestroyWheel();
        void RenderResult(object sender, RollResult roll);
    }
}