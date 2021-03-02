using System.Collections.Generic;

namespace RectangleTrainer.WheelOfPseudoFortune.Model
{
    public class RollResult
    {
        public float chance { get; private set; }
        private List<WheelBase> chosen;

        public WheelBase FinalSlot { get { return chosen.Count > 0 ? chosen[chosen.Count - 1] : null; } }
        public WheelBase RootSlot { get { return chosen.Count > 0 ? chosen[0] : null; } }
        public int Count { get { return chosen.Count; } }

        public RollResult()
        {
            chance = 1;
            chosen = new List<WheelBase>();
        }

        public void Add(WheelBase slot, uint totalChance)
        {
            chosen.Add(slot);
            float singleChance = (float)slot.chance / totalChance;
            chance *= singleChance;
        }

        public IReadOnlyList<WheelBase> Chosen { get { return chosen; } }

        public override string ToString()
        {
            string encoded = "";
            for(int i = chosen.Count-1; i >= 0; i--)
                encoded += chosen[i].name + " ";

            return encoded + ": " + chance;
        }

        public WheelBase Get(int index)
        {
            if (chosen.Count > 0 && index > 0 && index < chosen.Count)
                return chosen[index];
            
            return null;
        }
    }
}