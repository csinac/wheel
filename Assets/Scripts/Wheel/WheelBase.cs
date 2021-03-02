using System;
using System.Collections.Generic;

namespace RectangleTrainer.WheelOfPseudoFortune.Model
{
    public abstract class WheelBase
    {
        public string name { get; protected set; }
        public uint chance { get; protected set; }
        public List<WheelBase> subslots { get; protected set; }
        public abstract List<WheelBase> LeafSlots { get; }
        public abstract string[] LeafNames { get; }
        public int SlotCount { get { return subslots.Count; } }
        public int LeafCount { get { return LeafSlots.Count; } }
        public uint subwheelTotal { get; protected set; }

        public abstract WheelBase Root { get; }
        public abstract int Depth { get; }
        public abstract int RootDistance { get; }

        public abstract bool AddSlot(WheelBase slot);
        public abstract bool AddSlotAt(int index, WheelBase slot);
        public abstract bool AddSubslot(int index, WheelBase slot);
        public abstract bool AddSubslot(string slotName, WheelBase slot);

        public abstract RollResult Roll();
        public event EventHandler<RollResult> OnRoll;
        protected void NotifySubscribers(RollResult result) => OnRoll?.Invoke(this, result);

        public abstract string Transcribe();
    }
}
