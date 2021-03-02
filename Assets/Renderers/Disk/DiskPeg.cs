using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskPeg : MonoBehaviour
    {
        [SerializeField] protected Transform innerTransform;
        public void UpdatePosition(float angle, float offset, float radius)
        {
            if (innerTransform)
            {
                transform.localEulerAngles = new Vector3(0, 0, angle);
                innerTransform.localPosition = new Vector3(offset + radius, 0, 0);

                return;
            }

            Debug.Log("Peg transforms are missing.");
        }

        public void UpdateSize(float size)
        {
            if (innerTransform)
            {
                if (innerTransform.GetType().Equals(typeof(RectTransform)))
                    ((RectTransform)innerTransform).sizeDelta = new Vector2(size, size);
                else
                    innerTransform.localScale = new Vector3(size, size, size);
                return;
            }

            Debug.Log("Peg transforms are missing.");
        }
    }
}