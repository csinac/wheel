using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class GridCellUI: GridCellBase
    {
        [SerializeField] Color normalTint;
        [SerializeField] Color highlightTint;

        protected Text label;
        protected RectTransform rt;
        protected float blinkPower;
        protected Image image;

        public void Initialize(float x, float y, Transform parent, Vector2 padding)
        {
            image = GetComponent<Image>();
            label = GetComponentInChildren<Text>();
            rt = GetComponent<RectTransform>();
            rt.SetParent(parent);

            rt.anchoredPosition = new Vector2(x, y);
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;

            image.color = normalTint;
        }

        override public void Highlight(bool blinking)
        {
            base.Highlight(blinking);
            blinkPower = 0.8f;
            image.color = highlightTint;
            blinkerCR = StartCoroutine(Fade(blinking));
        }

        protected IEnumerator Fade(bool looping)
        {
            yield return new WaitForSeconds(0.1f);
            while (blinkPower > 0)
            {
                blinkPower -= Time.deltaTime * 2;
                image.color = Color.Lerp(normalTint, highlightTint, blinkPower);

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
            blinkPower = 0;
            image.color = normalTint;
            if (blinkerCR != null)
                StopCoroutine(blinkerCR);
        }

        public override void SetContent<T>(T content)
        {
            if (typeof(T).Equals(typeof(string)) && label)
            {
                label.text = content as string;
                //image.gameObject.SetActive(false);
                return;
            }
            /*
            if (typeof(T).Equals(typeof(Sprite)) && image)
            {
                image.sprite = content as Sprite;
                label.gameObject.SetActive(false);
                return;
            }
            */
            Debug.LogWarning($"no suitable content container found for type {typeof(T)}.");
        }
    }
}
