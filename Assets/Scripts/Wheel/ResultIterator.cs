namespace RectangleTrainer.WheelOfPseudoFortune.Model {
    public class ResultIterator {
        private RollResult result;
        public int index { get; private set; }

        public WheelBase FinalSlot { get { return result.FinalSlot; } }
        public WheelBase RootSlot { get { return result.RootSlot; } }
        public WheelBase CurrentSlot { get { return result.Get(index); } }

        public void GoToRoot() => index = 0;
        public void GoToFirst() => index = 1;
        public void GoToFinal() => index = result.Chosen.Count - 1;
        public bool HasNext() => index < result.Chosen.Count - 1;
        public bool HasPrevious() => index > 0;
        public ResultIterator(RollResult result)
        {
            this.result = result;
        }

        public bool TryMoveNext()
        {
            if (index < result.Chosen.Count - 1)
            {
                index++;
                return true;
            }

            return false;
        }
        public bool TryMovePrevious()
        {
            if (index > 0)
            {
                index--;
                return true;
            }

            return false;
        }
    }
}