using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [RequireComponent(typeof(UnityEngine.Renderer))]

    public class GridCellWorld : GridCellBase
    {
        protected TextMesh label;
        protected float blinkPower = 0;
        protected Material mat;

        public void Initialize(float x, float y, Transform parent)
        {
            mat = GetComponent<UnityEngine.Renderer>().material;
            label = GetComponentInChildren<TextMesh>();
            transform.SetParent(parent);

            transform.localPosition = new Vector3(x, y);
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;
        }

        override public void Highlight(bool blinking)
        {
            base.Highlight(blinking);
            blinkPower = 0.8f;
            mat.SetFloat("_Emission", blinkPower);
            blinkerCR = StartCoroutine(Fade(blinking));
        }

        protected IEnumerator Fade(bool looping)
        {
            yield return new WaitForSeconds(0.1f);
            while (blinkPower > 0)
            {
                blinkPower -= Time.deltaTime * 2;
                mat.SetFloat("_Emission", blinkPower);

                if (looping && blinkPower <= 0)
                {
                    blinkPower += 1;
                }

                yield return new WaitForEndOfFrame();
            }

            blinkPower = 0;

            yield return null;
        }

        override public void Dehighlight()
        {
            base.Dehighlight();
            blinkPower = 0;
            mat.SetFloat("_Emission", blinkPower);
        }

        override public void SetContent<T>(T content)
        {
            if (typeof(T).Equals(typeof(string)) && label)
            {
                label.text = content as string;
                return;
            }

            Debug.LogWarning($"no suitable content container found for type {typeof(T)}.");
        }
    }
}
