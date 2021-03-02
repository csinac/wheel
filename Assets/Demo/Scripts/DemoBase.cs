using UnityEngine;
using UnityEngine.UI;
using RectangleTrainer.WheelOfPseudoFortune.Model;

namespace RectangleTrainer.WheelOfPseudoFortune
{
    [DisallowMultipleComponent]
    public abstract class DemoBase : MonoBehaviour
    {
        [SerializeField] protected Button rollButton;
        [SerializeField] protected Text rollResult;
        protected WheelBase wheel;

        protected virtual void Start()
        {
            if(rollButton) rollButton.onClick.AddListener(Roll);
            CreateWheel();
        }

        protected abstract void CreateWheel();

        public virtual void Roll()
        {
            RollResult result = wheel.Roll();

            if (rollResult)
                rollResult.text = result.FinalSlot.name;
            else
                Debug.Log(result.FinalSlot.name);
        }
    }
}
