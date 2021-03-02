using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public abstract class DiskSliceUIBase : DiskSliceBase
    {
        [SerializeField] protected Text label;
        [SerializeField] protected RectTransform imageContainer;
        protected RectTransform rt;
        protected Image image;
        protected RectTransform selectedContentRT;

        #region Initialization
        public virtual void Initialize(float radius, float arc, Transform parent, int index)
        {
            rt = GetComponent<RectTransform>();
            rt.SetParent(parent);

            rt.localPosition = Vector3.zero;
            rt.sizeDelta = new Vector2(radius, Mathf.Sin(arc / 180 * Mathf.PI / 2) * 2 * radius);
            rt.localScale = Vector3.one;
            rt.localEulerAngles = new Vector3(0, 0, index * arc);

            if (imageContainer)
            {
                image = imageContainer.GetComponentInChildren<Image>();
            }
            else
            {
                Debug.LogWarning("Image container is missing.");
            }
        }
        #endregion

        #region Setters

        protected abstract void SetStyle(params object[] args);

        public override void SetContent<T>(T content)
        {
            if (typeof(T).Equals(typeof(string)) && label)
            {
                label.text = content as string;
                selectedContentRT = label.rectTransform;
                image.gameObject.SetActive(false);
                return;
            }
            if (typeof(T).Equals(typeof(Sprite)) && image)
            {
                image.sprite = content as Sprite;
                selectedContentRT = image.rectTransform;
                label.gameObject.SetActive(false);
                return;
            }
            Debug.LogWarning($"no suitable content container found for type {typeof(T)}.");
        }

        public override void SetFontSize(int size)
        {
            if (label)
            {
                label.fontSize = size;
                return;
            }

            throw new System.Exception("Label is missing");
        }

        public override void SetTextOffset(float offset)
        {
            if (label)
            {
                label.rectTransform.anchoredPosition = new Vector2(offset, label.rectTransform.anchoredPosition.y);
                return;
            }

            throw new System.Exception("Label is missing");
        }

        public override void SetContentScale(float scale)
        {
            if (image)
            {
                image.rectTransform.localScale = new Vector3(scale, scale, scale);
                return;
            }

            throw new System.Exception("Image is missing");
        }

        public override void SetContentRadialOffset(float offset)
        {
            if (imageContainer)
            {
                imageContainer.offsetMin = new Vector2(offset, imageContainer.offsetMin.y);
                imageContainer.offsetMax = new Vector2(offset, imageContainer.offsetMax.y);
                return;
            }

            throw new System.Exception("Image container is missing");
        }

        public override void SetContentRotation(float rotation)
        {
            if (image)
            {
                image.rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
                return;
            }

            throw new System.Exception("Image is missing");
        }
        #endregion

        #region Highlighting
        Coroutine highlightCR;
        float originalScale;
        float popupDelta;

        override public void Highlight(bool blinking)
        {
            if (highlightCR != null) Dehighlight();

            originalScale = selectedContentRT.localScale.x;
            popupDelta = originalScale;
            highlightCR = StartCoroutine(Highlight());
            transform.SetAsLastSibling();
        }

        override public void Dehighlight()
        {
            if (highlightCR == null) return;
            StopCoroutine(highlightCR);
            selectedContentRT.localScale = new Vector2(originalScale, originalScale);
            highlightCR = null;
        }

        private IEnumerator Highlight()
        {
            float t = 0;
            while (true)
            {
                selectedContentRT.localScale = new Vector2(originalScale + t * popupDelta, originalScale + t * popupDelta);
                t = (t + Time.deltaTime) % 1;
                yield return new WaitForEndOfFrame();
            }
        }
        #endregion
    }
}
