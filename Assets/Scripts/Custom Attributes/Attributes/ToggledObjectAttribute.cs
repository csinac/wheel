using UnityEngine;
using System;

namespace RectangleTrainer.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, Inherited = true)]
    public class ToggledObjectAttribute : PropertyAttribute, IToggledAttribute
    {
        private string controller;
        private int condition;
        private ControlType type;

        public string Controller => controller;
        public int Condition => condition;
        public ControlType Type => type;

        /// <summary>
        /// Makes the property conditionally visible based on the provided controller
        /// </summary>
        /// <param name="boolController">Name of the boolean controller</param>
        public ToggledObjectAttribute(string boolController)
        {
            controller = boolController;
            type = ControlType.Boolean;
        }

        /// <summary>
        /// Makes the property conditionally visible based on the provided controller
        /// </summary>
        /// <param name="intController">Name of the integer controller</param>
        /// <param name="condition">Condition to compare controller against</param>
        public ToggledObjectAttribute(string intController, int condition)
        {
            controller = intController;
            this.condition = condition;
            type = ControlType.Integer;
        }
    }
}
