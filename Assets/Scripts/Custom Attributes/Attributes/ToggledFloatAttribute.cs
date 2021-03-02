using UnityEngine;
using System;

namespace RectangleTrainer.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, Inherited = true)]
    public class ToggledFloatAttribute : PropertyAttribute, IToggledAttribute
    {
        private string controller;
        private int condition;
        private ControlType type;

        public string Controller => controller;
        public int Condition => condition;
        public ControlType Type => type;

        public bool slider { get; private set; }
        public float min { get; private set; }
        public float max { get; private set; }

        /// <summary>
        /// Makes the property conditionally visible based on the provided controller
        /// </summary>
        /// <param name="boolController">Name of the boolean controller</param>
        /// <param name="limit1">Optional range limit</param>
        /// <param name="limit2">Optional range limit</param>
        public ToggledFloatAttribute(string boolController, float limit1 = float.NaN, float limit2 = float.NaN)
        {
            controller = boolController;
            type = ControlType.Boolean;

            if (float.IsNaN(limit2)) limit2 = 0;

            if (float.IsNaN(limit1) || limit1 == limit2)
            {
                slider = false;
            }
            else
            {
                slider = true;
                min = Mathf.Min(limit1, limit2);
                max = Mathf.Max(limit1, limit2);
            }
        }

        /// <summary>
        /// Makes the property conditionally visible based on the provided controller
        /// </summary>
        /// <param name="intController">Name of the integer controller</param>
        /// <param name="condition">Condition to compare controller against</param>
        /// <param name="limit1">Optional range limit</param>
        /// <param name="limit2">Optional range limit</param>
        public ToggledFloatAttribute(string intController, int condition, float limit1 = float.NaN, float limit2 = float.NaN)
        {
            controller = intController;
            this.condition = condition;
            type = ControlType.Integer;

            if (float.IsNaN(limit2)) limit2 = 0;

            if (float.IsNaN(limit1) || limit1 == limit2)
            {
                slider = false;
            }
            else
            {
                slider = true;
                min = Mathf.Min(limit1, limit2);
                max = Mathf.Max(limit1, limit2);
            }
        }
    }
}
