using System;
using System.Collections.Generic;

namespace RectangleTrainer.WheelOfPseudoFortune.Model
{
    public delegate void RollNotification();
    public class Wheel : WheelBase
    {
        public static uint CHANCE_MAX = 1000000;
        public static uint DEPTH_MAX = 10;

        private Wheel parent;

        #region Constructors
        private Wheel(uint chance, string name)
        {
            this.chance = chance;
            this.name = name;
            parent = null;
            subslots = new List<WheelBase>();
        }

        /// <summary>
        /// Creates a wheel
        /// </summary>
        /// <param name="chance">Probability of this slot, maximum value defined by Wheel.CHANCE_MAX</param>
        /// <param name="name">Name of this wheel. Used for accessing slots</param>
        /// <returns></returns>
        public static Wheel Create(uint chance = 1, string name = "")
        {
            if (chance == 0)
                throw new Exceptions.ZeroChanceException();

            else if (chance > CHANCE_MAX)
                chance = CHANCE_MAX;

            Wheel slot = new Wheel(chance, name);
            return slot;
        }

        #endregion

        #region Add Slots

        /// <summary>
        /// Adds a wheel to the wheel as a subslot.
        /// </summary>
        /// <param name="slot">Wheel to be added as a slot</param>
        /// <returns></returns>
        override public bool AddSlot(WheelBase slot)
        {
            return AddSlotAt(subslots.Count, slot);
        }


        /// <summary>
        /// Inserts a wheel to the wheel as a subslot, at the desired index.
        /// </summary>
        /// <param name="index">Insertion index</param>
        /// <param name="slot">Wheel to be added as a slot</param>
        /// <returns></returns>
        override public bool AddSlotAt(int index, WheelBase slot)
        {
            if (slot.Root == Root)
                throw new Exceptions.SameWheelException(name, slot.name);

            if (RootDistance + slot.Depth > DEPTH_MAX)
                throw new Exceptions.MaxDepthReachedException(name, slot.name);

            if (slot != null && ((Wheel)slot).SetParent(this))
            {
                ResetDepth();
                if (subslots.Count < index)
                    subslots.Add(slot);
                else
                    subslots.Insert(index, slot);

                UpdateTotal();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a wheel to the subwheel at the given index as a subslot.
        /// </summary>
        /// <param name="index">Index of the subwheel</param>
        /// <param name="slot">Wheel to be added as a slot</param>
        /// <returns></returns>
        override public bool AddSubslot(int index, WheelBase slot)
        {
            if (index < subslots.Count)
            {
                depth = -1;
                return subslots[index].AddSlot(slot);
            }

            return false;
        }

        /// <summary>
        /// Adds a wheel to the subwheel at the given index as a subslot.
        /// </summary>
        /// <param name="slotName">Name of the subwheel<</param>
        /// <param name="slot"></param>
        /// <param name="slot">Wheel to be added as a slot</param>
        /// <returns></returns>
        override public bool AddSubslot(string slotName, WheelBase slot)
        {
            if (slot == null)
                return false;

            foreach (WheelBase subslot in subslots)
            {
                if (subslot.name == slotName)
                {
                    depth = -1;
                    return subslot.AddSlot(slot);
                }
            }

            return false;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Root wheel of the wheel, accessable from any subwheel
        /// </summary>
        override public WheelBase Root
        {
            get
            {
                if (parent == null)
                    return this;
                else
                    return parent.Root;
            }
        }

        private int depth = -1;
        /// <summary>
        /// Depth of the deepest subwheel in the wheel structure
        /// </summary>
        public override int Depth
        {
            get
            {
                if (depth < 0)
                    depth = RecursiveDepth();

                return depth;
            }
        }

        /// <summary>
        /// Distance of a slot to the root in the wheel structure
        /// </summary>
        public override int RootDistance => Root.Depth - Depth;

        private int RecursiveDepth(int deepest = 0)
        {
            int subdeepest = deepest;

            foreach (Wheel subslot in subslots)
            {
                int subdepth = subslot.RecursiveDepth(deepest + 1);
                subdeepest = subdepth > subdeepest ? subdepth : subdeepest;
            }

            return subdeepest;
        }

        /// <summary>
        /// List of final slots in the wheel structure.
        /// </summary>
        override public List<WheelBase> LeafSlots { get => RecursiveList(new List<WheelBase>()); }

        override public string[] LeafNames
        {
            get
            {
                List<WheelBase> finalSlots = LeafSlots;
                string[] names = new string[finalSlots.Count];

                for(int i = 0; i < names.Length; i++)
                    names[i] = finalSlots[i].name;

                return names;
            }
        }

        private List<WheelBase> RecursiveList(List<WheelBase> list, int level = 0)
        {
            if (level < DEPTH_MAX)
            {
                foreach (Wheel subslot in subslots)
                {
                    if (subslot.SlotCount == 0)
                        list.Add(subslot);
                    subslot.RecursiveList(list, level + 1);
                }
            }

            return list;
        }
        #endregion

        #region Setters
        private bool SetParent(Wheel parent)
        {
            if (parent == this)
                return false;

            WheelBase newRoot = parent.Root;

            if (Root == newRoot)
                return false;

            this.parent = parent;
            return true;
        }

        private void UpdateTotal()
        {
            subwheelTotal = 0;
            foreach (WheelBase subslot in subslots)
                subwheelTotal += subslot.chance;
        }

        private void ResetDepth()
        {
            depth = -1;
            if (parent != null) parent.ResetDepth();
        }
        #endregion

        #region Roll
        /// <summary>
        /// Rolls the wheel and returns a RollResult object, containing the result of the roll.
        /// </summary>
        /// <returns></returns>
        override public RollResult Roll()
        {
            RollResult result = new RollResult();
            result.Add(this, 1);
            result = Roll(0, result);

            NotifySubscribers(result);

            return result;
        }

        private RollResult Roll(int level, RollResult result)
        {
            if (subslots.Count > 0)
            {
                int roll = UnityEngine.Random.Range(0, (int)subwheelTotal);
                //UnityEngine.Debug.Log(roll);
                foreach (Wheel subslot in subslots)
                {
                    if (roll < subslot.chance)
                    {
                        result.Add(subslot, subwheelTotal);
                        return subslot.Roll(level + 1, result);
                    }

                    roll -= (int)subslot.chance;
                }
            }

            return result;
        }
        #endregion

        #region Print
        public override string ToString()
        {
            if (parent == null)
                return string.Format("{0}(Root), Wheel total: {1}", name, subwheelTotal);

            float odds = (float)chance / parent.subwheelTotal;
            for (Wheel p = parent; p.parent != null && p != this; p = p.parent)
            {
                odds *= (float)p.chance / p.parent.subwheelTotal;
            }

            if (subslots.Count > 0)
                return string.Format("{0}, Odds: {1} ({2}), Subwheel Total: {3}", name, chance, odds, subwheelTotal);
            else
                return string.Format("{0}, Odds: {1} ({2})", name, chance, odds);
        }

        private string RecursivePrint(int level = 0, float nOdds = 1)
        {
            string indentation = "";
            for (int i = 0; i < level; i++) indentation += ".";

            string encoded;
            nOdds *= parent == null ? 1 : (float)chance / parent.subwheelTotal;

            if (parent == null)
                encoded = "Root - Total Chances: " + subwheelTotal;
            else if (subslots.Count > 0)
                encoded = string.Format("{0}Name: {1}, Chance: {2} ({3}), Subchance Total: {4}", indentation, name, chance, nOdds, subwheelTotal);
            else
                encoded = string.Format("{0}Name: {1}, Chance: {2} ({3})", indentation, name, chance, nOdds);

            if (level < DEPTH_MAX)
            {
                foreach (Wheel subslot in subslots)
                    encoded += "\n" + indentation + subslot.RecursivePrint(level + 1, nOdds);
            }
            else
            {
                encoded += "\nDepth Exceeded";
            }

            return encoded;
        }

        /// <summary>
        /// Prints out a summary of the wheel, displaying the
        /// structure as well as the chances for each slot.
        /// </summary>
        /// <returns></returns>
        public override string Transcribe() => RecursivePrint();
        #endregion
    }
}