using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RectangleTrainer.CustomAttributes;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public abstract class GridBase : WheelRenderer
    {
        protected enum SizingPriority { MatchRows, MatchColumns }

        [Header("General")]
        [SerializeField] protected SizingMode sizingMode = SizingMode.step;
        [SerializeField, ToggledObject("sizingMode", (int)SizingMode.step)] protected Vector2 cellStep;
        [SerializeField, ToggledObject("sizingMode", (int)SizingMode.grid)] protected Vector2 gridSize;

        [SerializeField] protected SizingPriority priority;
        [SerializeField, ToggledInt("priority", (int)SizingPriority.MatchRows), Min(1)] protected uint rows;
        [SerializeField, ToggledInt("priority", (int)SizingPriority.MatchColumns), Min(1)] protected uint columns;
        [Header("Animation")]
        [SerializeField] protected AnimationCurve settleIntervals = AnimationCurve.EaseInOut(0.1f, 0, 1, 0.5f);
        [SerializeField] protected int steps = 50;
        [SerializeField] protected float settleDuration = 5;

        protected Dictionary<WheelBase, UnitRenderer> cellDict;
        protected Vector2 offset;

        override protected void Awake()
        {
            base.Awake();
        }

        protected void CalculateSize(int count)
        {
            if (rows < 1) rows = 1;
            if (columns < 1) columns = 1;

            Vector2Int gridDimensions = Vector2Int.zero;

            if (priority == SizingPriority.MatchColumns)
                gridDimensions = new Vector2Int((int)columns, Mathf.CeilToInt(1f * count / columns));
            else if (priority == SizingPriority.MatchRows)
                gridDimensions = new Vector2Int(Mathf.CeilToInt(1f * count / rows), (int)rows);

            if (sizingMode == SizingMode.step)
                gridSize = new Vector2(gridDimensions.x * cellStep.x, gridDimensions.y * cellStep.y);
            else
                cellStep = new Vector2(gridSize.x / gridDimensions.x, gridSize.y / gridDimensions.y);
        }

        public override void SetWheel(WheelBase wheel)
        {
            base.SetWheel(wheel);
            CreateWheel(wheel.LeafSlots);
        }

        public override bool ResetView()
        {
            if (base.ResetView())
            {
                CreateWheel(wheel.LeafSlots);
                return true;
            }

            return false;
        }

        protected override void CreateWheel(List<WheelBase> slots)
        {
            base.CreateWheel(slots);
            int slotCount = slots.Count;

            CalculateSize(slotCount);
            int[] order = SlotOrder(slotCount);
            cellDict = new Dictionary<WheelBase, UnitRenderer>();

            for (int i = 0; i < slotCount; i++)
            {
                UnitRenderer cell = MakeUnit(order[i], slots[i]);
                cellDict.Add(slots[i], cell);
            }
        }

        override public void RenderResult(object sender, RollResult result)
        {
            base.RenderResult(sender, result);
            if (result.RootSlot == null) return;

            if (needsReset)
                CreateWheel(result.RootSlot.LeafSlots);

            foreach (KeyValuePair<WheelBase, UnitRenderer> cellPair in cellDict)
                cellPair.Value.Dehighlight();

            animationCoroutine = StartCoroutine(RollAnimation(result));
        }

        protected virtual void CalculateCellPosition(int order, out float x, out float y)
        {
            x = 0;
            y = 0;

            if (priority == SizingPriority.MatchColumns)
            {
                x = order % columns;
                y = -order / columns;
            }
            else if (priority == SizingPriority.MatchRows)
            {
                x = order / rows;
                y = -order % rows;
            }

            x = (x + 0.5f) * cellStep.x - gridSize.x / 2f;
            y = (y - 0.5f) * cellStep.y + gridSize.y / 2f;
        }

        protected IEnumerator RollAnimation(RollResult result)
        {
            int count = wheel.SlotCount;
            float f = 0;
            List<WheelBase> keyList = new List<WheelBase>(cellDict.Keys);

            float rawTime = 0;
            for (float i = 0; i < steps; i++) rawTime += settleIntervals.Evaluate(i / steps);

            for (float i = 0; i < steps; i++)
            {
                float t = settleIntervals.Evaluate(i / steps);

                t /= rawTime / settleDuration;
                f += t;

                int randCell = Random.Range(0, keyList.Count);
                cellDict[keyList[randCell]].Highlight(false);

                yield return new WaitForSeconds(t);
            }

            cellDict[result.FinalSlot].Highlight(true);
            animationCoroutine = null;
            yield return null;
        }

        protected enum SizingMode {[InspectorName("By Step Size")] step, [InspectorName("By Entire Size")] grid }
    }
}
