using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class C86HeaderValue : MonoBehaviour
    {
        [SerializeField] Text title;
        [SerializeField] Text value;

        public void Init(string title, string value)
        {
            this.title.text = title;
            this.value.text = value;
        }

        public void Set(string value)
        {
            this.value.text = value;
        }

        public void Init(string title, float value) => Init(title, value.ToString("N1"));
        public void Set(float value) => Set(value.ToString("N1"));
    }
}