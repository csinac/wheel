using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Model
{
    public class NoGUI_Basics_Bootstrapper : DemoBase
    {
        protected override void CreateWheel()
        {
            /* Intro */

            //When creating a wheel, you can specify the chance of occurrence (uint)
            //and the name of the wheel. Both are optional and default to 1 and no name.

            //the root wheel, when printed, whill show as the Root
            wheel = Wheel.Create();

            //To add slots, simply create new wheels and add them. Every slot
            //is a wheel. Wheel is a wheel. Everything is a wheel. I am wheel.
            wheel.AddSlot(Wheel.Create(chance: 10, name: "10 chance slot"));

            wheel.AddSlot(Wheel.Create(chance: 1, name: "1 chance slot"));
            wheel.AddSlot(Wheel.Create(chance: 5, name: "5 chance slot"));

            //Transcribe will print out the structure of the wheel.
            //Right now you should see the a root wheel with three
            //of the above slots.
            Debug.Log(wheel.Transcribe());

            //Let's roll for fun and see the results.
            int testRollCount = 25;
            string[] results = new string[testRollCount];

            for(int i = 0; i < testRollCount; i++)
                results[i] = $"result {i}_ {wheel.Roll().FinalSlot.name}";

            Debug.Log(string.Join("\n", results));
            //Statistically speaking, you must see a bunch of 10 chance
            //with 5 chance sprinkled around, with a few 1 chance slots,
            //if any.

            /* Nesting Slots */
            wheel = Wheel.Create();

            Wheel lvl0_2 = Wheel.Create(name: "LVL0_2");
            lvl0_2.AddSlot(Wheel.Create(name: "LVL1_2_0"));
            lvl0_2.AddSlot(Wheel.Create(name: "LVL1_2_1"));

            wheel.AddSlot(Wheel.Create(name: "LVL0_0"));
            wheel.AddSlot(Wheel.Create(name: "LVL0_1"));
            wheel.AddSlot(lvl0_2); //lvl0_2 comes with it's own subwheels

            wheel.AddSubslot(0, Wheel.Create(name: "LVL1_0_0")); //add subslot to the zeroth subslot
            wheel.AddSubslot("LVL0_1", Wheel.Create(name: "LVL1_1_0")); //add to the subslot with the name LVL0_1

            lvl0_2.AddSubslot("LVL1_2_0", Wheel.Create(name: "LVL2_2_0"));
            lvl0_2.AddSubslot("LVL1_2_0", Wheel.Create(name: "LVL2_2_1")); //retroactively add 2 more slots back to lvl0_2

            //Here, you'll see the structure of the wheel transcribed into
            //the output in the form of indentations.
            Debug.Log(wheel.Transcribe());
        }
    }

}