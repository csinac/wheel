using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using RectangleTrainer.CustomAttributes;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class DiskBase<Content> : WheelRenderer
    {
        [Header("Unit Styling")]
        [SerializeField] protected ContentType contentType = ContentType.text;
        protected bool DictContent { get { return contentType == ContentType.dict; } }
        protected bool TextContent { get { return contentType == ContentType.text; } }

        [SerializeField, ToggledFloat("contentType", (int)ContentType.text)] protected int fontSize = 24;
        [SerializeField, ToggledFloat("contentType", (int)ContentType.text)] protected float textOffset = 0;
        [SerializeField, ToggledFloat("contentType", (int)ContentType.dict, 1)] protected float contentScale = 0.5f;
        [SerializeField, ToggledFloat("contentType", (int)ContentType.dict, 360)] protected float contentRotation = 0;
        [SerializeField, ToggledFloat("contentType", (int)ContentType.dict)] protected float contentOffset = 0;

        [Header("Wheel")]
        [SerializeField] protected bool chained;
        [SerializeField] protected float radius;
        [SerializeField, ToggledObject("contentType", (int)ContentType.dict)] protected UnitContentServer<Content> contentServer;

        [Header("Animation")]
        [SerializeField] protected AnimationCurve completionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] protected float rollDuration = 1;
        [SerializeField] protected int minimumFullTurns = 2;
        [SerializeField] protected float settleDelay = 0.5f;
        [SerializeField] protected float settleDuration = 0.25f;
        [SerializeField] protected bool hideUnchosen = false;
        [SerializeField, ToggledFloat("chained")] protected float chainDelay = 0.5f;

        [Header("Peg")]
        [SerializeField] DiskPeg peg;
        [SerializeField, Range(0, 360)] protected float pegAngle = 90;
        [SerializeField, Min(0)] protected float pegSize;
        [SerializeField] protected float pegOffset;

        public float PegOffset { get { return pegOffset; } }
        public float PegAngle { get { return pegAngle; } }
        public float PegSize { get { return pegSize; } }
        public float Radius { get { return radius; } }

        protected float arc;
        protected Dictionary<WheelBase, DiskSliceBase> sliceDict = new Dictionary<WheelBase, DiskSliceBase>();
        protected UnitRenderer highlighted;

        abstract protected void OnDrawGizmos();

        #region Visual Setup

        override protected void Awake()
        {
            base.Awake();
            TrySetContentDictionary();
        }

        public void SetPegTransform(float? angle = null, float? offset = null, float? radius = null)
        {
            if (angle != null) pegAngle = (float)angle;
            if (angle != null) pegOffset = (float)offset;
            if (angle != null) this.radius = (float)radius;

            if (peg != null)
            {
                peg.UpdatePosition(pegAngle, pegOffset, this.radius);
                return;
            }
            Debug.Log("Peg missing.");
        }

        public void SetPegSize(float? size = null)
        {
            if (size != null) pegSize = (float)size;

            if (peg != null)
            {
                peg.UpdateSize(pegSize);
                return;
            }
            Debug.Log("Peg missing.");
        }

        protected UnitContentDictionary<Content> contentDict;

        public void SetContentDictionary(UnitContentDictionary<Content> contentDict)
        {
            this.contentDict = contentDict;
        }

        protected void TrySetContentDictionary()
        {
            if (contentServer == null)
                contentServer = GetComponentInChildren<UnitContentServer<Content>>();

            if (contentServer)
                contentDict = contentServer.Dictionary;
            else
                contentDict = new UnitContentDictionary<Content>();
        }

        public void SetContent<DiskSlice>(ref DiskSlice slice, string slotName) where DiskSlice : DiskSliceBase
        {
            Content content = default(Content);
            if (DictContent)
            {
                if (contentDict == null)
                {
                    Debug.LogWarning("Content dictionary is missing. Falling back to text label.");
                }
                else
                {
                    content = contentDict.GetContent(slotName);
                    if (content == null)
                        Debug.LogWarning($"No sprite is assigned to slot name \"{slotName}\". Falling back to text label.");
                }
            }

            if (content != null)
            {
                slice.SetContent(content);
                slice.SetContentRadialOffset(contentOffset);
                slice.SetContentScale(contentScale);
                slice.SetContentRotation(contentRotation);
            }
            else
            {
                slice.SetContent(slotName);
                slice.SetFontSize(fontSize);
                slice.SetTextOffset(textOffset);
            }
        }

        #endregion

        #region Model Relation
        public override void SetWheel(WheelBase wheel)
        {
            base.SetWheel(wheel);
            CreateWheel(chained ? wheel.subslots : wheel.LeafSlots);
        }

        public override bool ResetView()
        {
            if (base.ResetView())
            {
                CreateWheel(chained ? wheel.subslots : wheel.LeafSlots);
                return true;
            }

            return false;
        }

        protected override void CreateWheel(List<WheelBase> slots)
        {
            base.CreateWheel(slots);

            sliceDict = new Dictionary<WheelBase, DiskSliceBase>();
            unitContainer.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            arc = 360f / slots.Count;

            int[] order = SlotOrder(slots.Count);

            for (int i = 0; i < slots.Count; i++)
            {
                DiskSliceBase slice = (DiskSliceBase)MakeUnit(order[i], slots[i]);
                sliceDict.Add(slots[i], slice);
            }
        }

        #endregion

        #region Animation
        protected float GetUnitRotation(UnitRenderer unit) => unit.transform.localEulerAngles.z;

        override public void RenderResult(object sender, RollResult result)
        {
            base.RenderResult(sender, result);
            if (result.RootSlot == null) return;

            if (highlighted)
            {
                highlighted.Dehighlight();
                highlighted = null;
            }

            if (chained)
            {
                if (needsReset)
                    CreateWheel(result.RootSlot.subslots);

                needsReset = true;
            }

            if (hideUnchosen)
            {
                foreach (KeyValuePair<WheelBase, DiskSliceBase> slicePair in sliceDict)
                    slicePair.Value.Toggle(state: true);
            }

            animationCoroutine = StartCoroutine(RollAnimation(result));
        }

        protected IEnumerator RollAnimation(RollResult result)
        {
            ResultIterator iter = new ResultIterator(result);

            if (chained)
                iter.GoToFirst();
            else
                iter.GoToFinal();

            DiskSliceBase unit;
            do
            {
                float startZ, targetZ, offset;
                unit = sliceDict.ContainsKey(iter.CurrentSlot) ? sliceDict[iter.CurrentSlot] : null;

                SetAnimParameters(unit, out startZ, out targetZ, out offset);

                for (float tl = 0; tl < 1; tl += Time.deltaTime / rollDuration)
                {
                    float t = completionCurve.Evaluate(tl); //linear t to curve t
                    unitContainer.localEulerAngles = new Vector3(0, 0, startZ * (1 - t) + targetZ * t);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(settleDelay);
                float finalPosWithOffset = unitContainer.localEulerAngles.z;
                for (float t = 0; t < 1; t += Time.deltaTime / settleDuration)
                {
                    unitContainer.localEulerAngles = new Vector3(0, 0, finalPosWithOffset + t * offset);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(chainDelay);

                if (iter.HasNext())
                    CreateWheel(iter.CurrentSlot.subslots);
            } while (iter.TryMoveNext());

            if (hideUnchosen)
            {
                foreach (KeyValuePair<WheelBase, DiskSliceBase> pair in sliceDict)
                {
                    if (pair.Value != unit)
                        pair.Value.Toggle(state: false);
                }
            }

            unit.Highlight(blinking: true);
            highlighted = unit;
            animationCoroutine = null;
            yield return null;
        }

        protected void SetAnimParameters(UnitRenderer unit, out float start, out float target, out float offset)
        {
            float targetPos = unit ? GetUnitRotation(unit) : 0;

            start = unitContainer.localEulerAngles.z;
            target = -targetPos + pegAngle;
            offset = Random.Range(-0.45f, 0.45f) * arc;
            target -= offset;
            target -= minimumFullTurns * 360;
        }

        #endregion
    }
}
