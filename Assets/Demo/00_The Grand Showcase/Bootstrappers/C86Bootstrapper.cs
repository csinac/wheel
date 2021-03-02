using UnityEngine;
using UnityEngine.UI;
using RectangleTrainer.WheelOfPseudoFortune.Model;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune
{
    public class C86Bootstrapper : DemoView
    {
        override protected void CreateWheel() {
            wheel = Wheel.Create(name: "Lab Upgrade");

            WheelBase telepod = Wheel.Create(name: "Telepod");
            telepod.AddSlot(Wheel.Create(5, "Safety Locks"));
            telepod.AddSlot(Wheel.Create(3, "+1 Atomic Decimator"));
            telepod.AddSlot(Wheel.Create(3, "+1 Atomic Regenerator"));
            telepod.AddSlot(Wheel.Create(1, "+1 Telepod"));

            telepod.AddSlot(Wheel.Create(10, "Cabling"));
            telepod.AddSubslot("Cabling", Wheel.Create(50, "+1 Copper Cable"));
            telepod.AddSubslot("Cabling", Wheel.Create(10, "+1 Gold Cable"));
            telepod.AddSubslot("Cabling", Wheel.Create(1, "+1 Carbon Cable"));

            telepod.AddSlot(Wheel.Create(20, "Upgrades"));
            telepod.AddSubslot("Upgrades", Wheel.Create(1, "+10% UV Blocking"));
            telepod.AddSubslot("Upgrades", Wheel.Create(1, "+10% Insulation"));
            telepod.AddSubslot("Upgrades", Wheel.Create(1, "+10% Regen Speed"));

            WheelBase computer = Wheel.Create(name: "Computer");
            computer.AddSlot(Wheel.Create(1, "Turbo CPU Add-on"));
            computer.AddSlot(Wheel.Create(5, "+64K Memory"));
            computer.AddSlot(Wheel.Create(3, "DNA Validator"));
            computer.AddSlot(Wheel.Create(10, "Insectoid DNA Filter"));
            computer.AddSlot(Wheel.Create(10, "Tissue Translator"));
            computer.AddSlot(Wheel.Create(10, "+1 Teleport Subject"));
            computer.AddSlot(Wheel.Create(10, "Voice Activation"));

            wheel.AddSlot(telepod);
            wheel.AddSlot(computer);

            foreach (WheelRenderer renderer in wheelRenderers)
                if (renderer) renderer.SetWheel(wheel);
        }
    }
}