using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskSliceUIGenerated: DiskSliceUIBase
    {
        private Material mat;
        [Space]
        [SerializeField] Color[] fillColors;
        [SerializeField] bool intermediateColors = true;
        [SerializeField] Color borderColor = Color.black;
        [Space]
        [SerializeField] Color textColor = Color.black;

        public override void Initialize(float radius, float arc, Transform parent, int index)
        {
            base.Initialize(radius, arc, parent, index);
            SetStyle(index, arc);
        }

        /// <summary>
        /// Styling Setup for Solid Color Disk Slice
        /// </summary>
        /// <param name="args">0th element slice index (int), 1st element arc in degrees (float)</param>
        override protected void SetStyle(params object[] args)
        {
            if (args.Length != 2)
                throw new System.Exception("Parameter count does not match the styling requirements (needs two: index (int), arc in degrees (float)");

            int index = (int)args[0];
            float arc = (float)args[1] / 180 * Mathf.PI;

            mat = Instantiate(GetComponent<Image>().material);
            GetComponent<Image>().material = mat;

            if (mat)
            {
                mat.SetFloat("arc", arc);
                if(fillColors.Length > 0)
                {
                    Color color;

                    if(intermediateColors)
                    {
                        color = Color.Lerp(fillColors[ index      % fillColors.Length],
                                           fillColors[(index + 1) % fillColors.Length],
                                           Random.Range(0f, 1f));
                    }
                    else
                    {
                        color = fillColors[index % fillColors.Length];
                    }
                    mat.SetColor("tint", color);
                }

                mat.SetColor("borderColor", borderColor);
                mat.SetFloat("aspect", rt.sizeDelta.y / rt.sizeDelta.x);
            }

            label.color = textColor;
        }
    }
}
