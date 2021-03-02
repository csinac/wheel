using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class C86Entry : UnitRenderer
    {
        [SerializeField] protected Text label;

        protected Coroutine blinkerCR;
        protected RectTransform rt;
        protected CanvasGroup cg;
        protected float life;
        protected float lifeMax;
        protected float loopMax;
        protected float loopMult = 0.2f;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();
        }

        public void Initialize(Transform parent, int x, int y, float xRange, float yRange, float life, float fadeDelay)
        {
            rt.SetParent(parent);

            rt.localPosition = Vector3.zero;

            rt.anchorMin = new Vector2(x * xRange, y * yRange);
            rt.anchorMax = new Vector2((x + 1) * xRange, (y + 1) * yRange);

            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            rt.localEulerAngles = Vector3.zero;
            rt.localScale = Vector3.one;

            this.life = life;
            this.lifeMax = life;
            this.loopMax = life * loopMult;

            StartCoroutine(FadeOut(fadeDelay));
        }

        protected IEnumerator FadeOut(float fadeDelay = 0, bool looping = false)
        {
            yield return new WaitForSeconds(fadeDelay);

            while(life > 0)
            {
                life -= Time.deltaTime;
                cg.alpha = life / (looping ? loopMax : lifeMax);
                yield return new WaitForSeconds(Time.deltaTime);

                if (looping && life <= 0) life += loopMax;
            }

            Destroy(gameObject);
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

        override public void Highlight(bool blinking)
        {
            StopAllCoroutines();

            life = loopMax;
            StartCoroutine(FadeOut(looping: true));
        }

        override public void Dehighlight()
        {
            StopAllCoroutines();
        }
    }
}
