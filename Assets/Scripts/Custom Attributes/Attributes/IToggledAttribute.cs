using UnityEngine;
using System;

namespace RectangleTrainer.CustomAttributes
{
    public interface IToggledAttribute
	{
        string Controller { get; }
        int Condition { get; }
        ControlType Type { get; }
    }
}