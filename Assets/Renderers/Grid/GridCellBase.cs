using System.Collections;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class GridCellBase : UnitRenderer
    {
        protected Coroutine blinkerCR;

        override public void Highlight(bool blinking)
        {
            if (blinkerCR != null)
                StopCoroutine(blinkerCR);
        }

        override public void Dehighlight()
        {
            if (blinkerCR != null)
                StopCoroutine(blinkerCR);
        }
    }
}
