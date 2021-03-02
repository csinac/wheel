using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class DiskSliceWorld : DiskSliceBase
    {
        protected MeshFilter meshFilter;
        [SerializeField] protected TextMesh label;
        [SerializeField] protected Transform contentTransform;
        [SerializeField] protected Transform contentContainer;

        protected float radius;
        protected float arc;
        protected float thickness;

        #region Initialization

        abstract protected Mesh Mesh();

        virtual public void Initialize(float arc, float radius, float thickness, Transform parent, int index)
        {
            this.radius = radius;
            this.arc = arc;
            this.thickness = thickness;

            meshFilter = GetComponent<MeshFilter>();

            label = GetComponentInChildren<TextMesh>();
            meshFilter.mesh = Mesh();
            transform.SetParent(parent);

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localEulerAngles = new Vector3(0, 0, index * arc);
        }

        #endregion

        #region Setters
        override public void SetContent<T>(T content)
        {
            if (typeof(T).Equals(typeof(string)) && label)
            {
                label.text = content as string;
                contentTransform.gameObject.SetActive(false);
                return;
            }
            if (typeof(T).Equals(typeof(GameObject)) && contentTransform)
            {
                GameObject contentGO = Instantiate(content as GameObject);
                contentGO.transform.SetParent(contentTransform);
                contentGO.transform.localPosition = Vector3.zero;
                contentGO.transform.localEulerAngles = Vector3.zero;
                contentGO.transform.localScale = Vector3.one;

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
                label.transform.localPosition = new Vector3(offset,
                                                             label.transform.localPosition.y,
                                                             label.transform.localPosition.z);
                return;
            }

            throw new System.Exception("Label is missing");
        }

        public override void SetContentScale(float scale)
        {
            if (contentTransform)
            {
                contentTransform.localScale = new Vector3(scale, scale, scale);
                return;
            }

            throw new System.Exception("Image is missing");
        }

        public override void SetContentRadialOffset(float offset)
        {
            if (contentTransform)
            {
                contentTransform.localPosition = new Vector3(offset,
                                                              contentTransform.localPosition.y,
                                                              contentTransform.localPosition.z);
                return;
            }

            throw new System.Exception("Image is missing");
        }

        public override void SetContentRotation(float rotation)
        {
            if (contentTransform)
            {
                contentTransform.localEulerAngles = new Vector3(0, 0, rotation);
                return;
            }

            throw new System.Exception("Image is missing");
        }
        #endregion


        #region Highlighting

        Coroutine highlightCR;
        Vector3 originalEulerAngles;
        Transform originalParent;

        override public void Highlight(bool blinking)
        {
            if (highlightCR != null) Dehighlight();

            originalEulerAngles = new Vector3(transform.localEulerAngles.x,
                                              transform.localEulerAngles.y,
                                              transform.localEulerAngles.z);

            originalParent = transform.parent;
            transform.SetParent(originalParent.parent); //move outside unit container
            highlightCR = StartCoroutine(Highlight());
        }

        override public void Dehighlight()
        {
            if (highlightCR == null) return;
            StopCoroutine(highlightCR);
            highlightCR = null;
            transform.parent = originalParent;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = originalEulerAngles;
        }

        private IEnumerator Highlight()
        {
            float t = 0;
            Quaternion targetRotation = Quaternion.AngleAxis(90, Vector3.forward);
            Quaternion startRotation = transform.localRotation;

            Vector3 targetPosition = new Vector3(x: 0,
                                                    y: -radius / 2,
                                                    z: -Mathf.Max(meshFilter.mesh.bounds.size.y, meshFilter.mesh.bounds.size.z));

            Vector3 undulation;
            float undulationLimit = meshFilter.mesh.bounds.size.x / 5;
            float undulationCycleTime = 2;
            while (true)
            {
                undulation = new Vector3(0, Mathf.Sin(t / undulationCycleTime) * undulationLimit, 0);
                if (t < 1f)
                {
                    transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
                    transform.localPosition = Vector3.Lerp(Vector3.zero, targetPosition + undulation, t);
                }
                else
                {
                    transform.localPosition = targetPosition + undulation;
                }

                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion

        #region Toggle
        /*
        new public void Toggle(bool state)
        {

        }
        */
        #endregion
    }
}
