using System.Collections;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class DiskSliceBase: UnitRenderer
    {
        public abstract void SetFontSize(int size);
        public abstract void SetTextOffset(float offset);
        public abstract void SetContentScale(float scale);
        public abstract void SetContentRadialOffset(float offset);
        public abstract void SetContentRotation(float rotation);

        #region Toggle
        [SerializeField] AnimationCurve toggleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] float toggleTime = 0.5f;
        protected bool toggleState = true;
        Coroutine toggleCR;

        new public void Toggle(bool state)
        {
            if (toggleState == state)
                return;

            toggleState = state;

            if (toggleCR != null)
                StopCoroutine(toggleCR);

            toggleCR = StartCoroutine(ToggleAnimation(state ? 1 : 0));
        }

        protected IEnumerator ToggleAnimation(float target)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.25f));

            float start = transform.localScale.x;
            for(float f = 0; f < 1f; f += Time.deltaTime / toggleTime)
            {
                float s = Mathf.LerpUnclamped(start, target, toggleCurve.Evaluate(f));
                transform.localScale = new Vector3(s, s, s);
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = new Vector3(target, target, target);
            toggleCR = null;
            yield return null;
        }
        #endregion
    }
}
