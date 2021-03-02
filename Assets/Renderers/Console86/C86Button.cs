using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class C86Button : Button
    {
        [SerializeField] Color highlightColor;
        [SerializeField] Color busyColor;
        [SerializeField] Color textColor;
        private Text text;

        protected override void Start()
        {
            base.Start();
            text = GetComponentInChildren<Text>();
        }


        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (text == null)
                text = gameObject.GetComponentInChildren<Text>();

            switch (state)
            {
                case SelectionState.Normal:
                    Dehighlight();
                    break;
                case SelectionState.Highlighted:
                    Highlight();
                    break;
                case SelectionState.Pressed:
                    Highlight();
                    break;
                case SelectionState.Disabled:
                    Busy();
                    break;
                default:
                    Dehighlight();
                    break;
            }
        }

        private void Highlight()
        {
            base.image.color = textColor;
            text.color = highlightColor;
        }

        private void Dehighlight()
        {
            base.image.color = highlightColor;
            text.color = textColor;
        }

        private void Busy()
        {
            base.image.color = busyColor;
            text.color = textColor;
        }

    }
}
