using UnityEngine;
using UnityEngine.UI;
using RectangleTrainer.WheelOfPseudoFortune.Model;
using RectangleTrainer.WheelOfPseudoFortune.Renderer;

namespace RectangleTrainer.WheelOfPseudoFortune
{
    public class DiskBootstrapper : DemoView
    {
        override protected void CreateWheel() {
            wheel = Wheel.Create(name: "Wheel of Fate");

            WheelBase weapons = Wheel.Create(name: "Weapons");
            weapons.AddSlot(Wheel.Create(1, "Sword"));
            weapons.AddSubslot("Sword", Wheel.Create(1, "Epic Sword"));
            weapons.AddSubslot("Sword", Wheel.Create(5, "Rare Sword"));
            weapons.AddSubslot("Sword", Wheel.Create(15, "Common Sword"));

            weapons.AddSlot(Wheel.Create(1, "Halberd"));
            weapons.AddSubslot("Halberd", Wheel.Create(1, "Epic Halberd"));
            weapons.AddSubslot("Halberd", Wheel.Create(5, "Rare Halberd"));
            weapons.AddSubslot("Halberd", Wheel.Create(15, "Common Halberd"));

            weapons.AddSlot(Wheel.Create(1, "Bow"));
            weapons.AddSubslot("Bow", Wheel.Create(1, "Epic Bow"));
            weapons.AddSubslot("Bow", Wheel.Create(5, "Rare Bow"));
            weapons.AddSubslot("Bow", Wheel.Create(15, "Common Bow"));

            weapons.AddSlot(Wheel.Create(1, "Mace"));
            weapons.AddSubslot("Mace", Wheel.Create(1, "Epic Mace"));
            weapons.AddSubslot("Mace", Wheel.Create(5, "Rare Mace"));
            weapons.AddSubslot("Mace", Wheel.Create(15, "Common Mace"));

            WheelBase armors = Wheel.Create(name: "Armors");
            armors.AddSlot(Wheel.Create(2, "Leggings"));
            armors.AddSlot(Wheel.Create(4, "Chestplate"));
            armors.AddSlot(Wheel.Create(1, "Shield"));
            armors.AddSlot(Wheel.Create(10, "Boots"));

            WheelBase powerups = Wheel.Create(name: "Powerups");
            powerups.AddSlot(Wheel.Create(1, "Increased HP"));
            powerups.AddSubslot(0, Wheel.Create(1, "HP +100"));
            powerups.AddSubslot(0, Wheel.Create(9, "HP +25"));
            powerups.AddSubslot(0, Wheel.Create(40, "HP +10"));
            powerups.AddSlot(Wheel.Create(1, "Increased ATK"));
            powerups.AddSubslot(1, Wheel.Create(1, "ATK +1000"));
            powerups.AddSubslot(1, Wheel.Create(9, "ATK +400"));
            powerups.AddSubslot(1, Wheel.Create(40, "ATK +100"));
            powerups.AddSlot(Wheel.Create(1, "Increased DEX"));
            powerups.AddSubslot(2, Wheel.Create(1, "DEX +20"));
            powerups.AddSubslot(2, Wheel.Create(9, "DEX +6"));
            powerups.AddSubslot(2, Wheel.Create(40, "DEX +2"));

            wheel.AddSlot(weapons);
            wheel.AddSlot(armors);
            wheel.AddSlot(powerups);

            foreach (WheelRenderer renderer in wheelRenderers)
                if (renderer) renderer.SetWheel(wheel);
        }
    }
}