using UnityEngine;
using UnityEngine.UI;
using RectangleTrainer.WheelOfPseudoFortune.Model;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune
{
    public class StylingBootstrapper : DemoView
    {
        override protected void CreateWheel() {
            wheel = Model.Wheel.Create();
            wheel.AddSlot(Model.Wheel.Create(name: "Axe"));
            wheel.AddSlot(Model.Wheel.Create(name: "Crossbow"));
            wheel.AddSlot(Model.Wheel.Create(name: "Bow"));
            wheel.AddSlot(Model.Wheel.Create(name: "Hammer"));
            wheel.AddSlot(Model.Wheel.Create(name: "Mace"));
            wheel.AddSlot(Model.Wheel.Create(name: "Wand"));
            wheel.AddSlot(Model.Wheel.Create(name: "Staff"));
            wheel.AddSlot(Model.Wheel.Create(name: "Sword"));
            wheel.AddSlot(Model.Wheel.Create(name: "Dagger"));
            wheel.AddSlot(Model.Wheel.Create(name: "Plasma\nCannon"));

            SetRendererWheels();
        }
    }
}