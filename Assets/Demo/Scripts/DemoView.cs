using UnityEngine;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune
{
    public abstract class DemoView : DemoBase
    {
        [SerializeField] protected WheelRenderer[] wheelRenderers = default;

        protected void SetRendererWheels()
        {
            foreach (WheelRenderer renderer in wheelRenderers)
                if (renderer) renderer.SetWheel(wheel);
        }

        public void Reload()
        {
            foreach (WheelRenderer renderer in wheelRenderers)
                if (renderer) renderer.ResetView();
        }
    }
}