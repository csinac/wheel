using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    [DisallowMultipleComponent]
    public abstract class WheelRenderer : MonoBehaviour, IWheelRenderer
    {
        [Header("Wheel Renderer Global Options")]
        [SerializeField] protected Transform unitContainer;
        [SerializeField] protected bool shuffle;

        protected WheelBase wheel;
        protected Coroutine animationCoroutine = null;
        protected bool needsReset = false;

        public Transform UnitContainer { get { return unitContainer; } }
        public bool PreviewRenderer { get => hideFlags.HasFlag(HideFlags.HideInHierarchy); } 

        protected virtual void Awake() {
            if (unitContainer == null)
                unitContainer = GetComponent<Transform>();
        }

        virtual public void DestroyWheel()
        {
            UnitRenderer[] units = GetComponentsInChildren<UnitRenderer>();
            foreach (UnitRenderer unit in units) Destroy(unit.gameObject);
        }

        virtual public void RenderResult(object sender, RollResult roll)
        {
            if (roll.RootSlot == null) return;
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }

        virtual public bool ResetView() {
            if (wheel == null) return false;
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);

            return true;
        }

        virtual public void UnsetWheel()
        {
            if (wheel != null)
                wheel.OnRoll -= RenderResult;

            wheel = null;
        }

        virtual public void SetWheel(WheelBase wheel)
        {
            if (this.wheel != null)
                this.wheel.OnRoll -= RenderResult;

            this.wheel = wheel;

            if (this.wheel != null)
                this.wheel.OnRoll += RenderResult;
        }

        virtual protected void CreateWheel(List<WheelBase> slots)
        {
            if (UnitContainer == null)
                throw new Exceptions.MissingUnitContainerException(gameObject.name);

            DestroyWheel();
        }

        protected int[] SlotOrder(int length)
        {
            int[] order = new int[length];

            for (int i = 0; i < length; i++)
                order[i] = i;

            if (shuffle)
            {
                for (int i = 0; i < order.Length; i++)
                {
                    int r = Random.Range(i, order.Length);
                    int tmp = order[i];
                    order[i] = order[r];
                    order[r] = tmp;
                }
            }

            return order;
        }

        protected abstract UnitRenderer MakeUnit(int order, WheelBase slot);

    }
}