using NUnit.Framework;

namespace RectangleTrainer.WheelOfPseudoFortune.Tests.UnitTests
{
    using Model;

    public class WheelTests
    {
        [Test]
        public void Create_CannotCreateZeroChanceWheel_ShouldThrowException()
        {
            Assert.Throws<Exceptions.ZeroChanceException>(() => Wheel.Create(chance: 0));
        }

        [Test]
        public void Create_ChanceCanBe1MillonMax_ShouldCapAt1M()
        {
            WheelBase wheel = Wheel.Create(chance: 2000000);

            Assert.LessOrEqual(wheel.chance, Wheel.CHANCE_MAX);
        }

        [Test]
        public void AddSubslot_ShouldNotAcceptItselfAsSubslot_ShouldThrowException()
        {
            WheelBase wheel = Wheel.Create();
            Assert.Throws<Exceptions.SameWheelException>(() => wheel.AddSlot(wheel));
        }

        [Test]
        public void AddSlot_AddToSameTree_ShouldThrowException()
        {
            WheelBase wheel = Wheel.Create();
            WheelBase subwheel1 = Wheel.Create();
            WheelBase subwheel2 = Wheel.Create();

            wheel.AddSlot(subwheel1);
            subwheel1.AddSlot(subwheel2);

            Assert.Throws<Exceptions.SameWheelException>(() => subwheel2.AddSlot(wheel));
        }

        [Test]
        public void AddSubslot_AddToSameTree_ShouldThrowException()
        {
            WheelBase wheel = Wheel.Create();
            WheelBase subwheel1 = Wheel.Create();
            WheelBase subwheel2 = Wheel.Create();

            wheel.AddSlot(subwheel1);
            subwheel1.AddSlot(subwheel2);

            Assert.Throws<Exceptions.SameWheelException>(() => subwheel1.AddSubslot(0, wheel));
        }

        [Test]
        public void Depth_CalculatesCorrectly()
        {
            int[] inputDepths = new int[Wheel.DEPTH_MAX];
            int[] outputDepths = new int[Wheel.DEPTH_MAX];

            for (int i = 0; i < Wheel.DEPTH_MAX; i++)
            {
                inputDepths[i] = i;
                WheelBase wheel = CreateDummyWheelWithDepth(inputDepths[i]);
                outputDepths[i] = wheel.Depth;
            }

            CollectionAssert.AreEqual(inputDepths, outputDepths);
        }

        [Test]
        public void RootDistance_CalculatesCorrectly()
        {
            int[] randomDepths = new int[Wheel.DEPTH_MAX];
            int[] outputDepths = new int[Wheel.DEPTH_MAX];

            for (int i = 0; i < Wheel.DEPTH_MAX; i++)
            {
                randomDepths[i] = i;
                WheelBase wheel = CreateDummyWheelWithDepth(randomDepths[i], getLeaf: true);
                outputDepths[i] = wheel.RootDistance;
            }

            CollectionAssert.AreEqual(randomDepths, outputDepths);
        }

        [Test]
        public void AddSlot_NewSubslotOnMaxedOutWheelRoot_ShouldWorkFine()
        {
            WheelBase parentWheel = CreateDummyWheelWithDepth((int)Wheel.DEPTH_MAX);
            WheelBase childWheel = Wheel.Create();

            Assert.IsTrue(parentWheel.AddSlot(childWheel));
        }

        [Test]
        public void AddSlot_AddMoreThanMaxDepth_ShouldThrowException()
        {
            int depth1 = UnityEngine.Random.Range(0, (int)Wheel.DEPTH_MAX);
            int depth2 = 1 + (int)Wheel.DEPTH_MAX - depth1;

            WheelBase parentWheel = CreateDummyWheelWithDepth(depth1, getLeaf: true);
            WheelBase childWheel = CreateDummyWheelWithDepth(depth2);

            Assert.Throws<Exceptions.MaxDepthReachedException>(() => parentWheel.AddSlot(childWheel));
        }

        [Test]
        public void LeafSlots_ShouldOnlyContainLeafSlots()
        {
            WheelBase wheel = Wheel.Create();

            wheel.AddSlot(Wheel.Create(name: "A"));
            wheel.AddSlot(Wheel.Create(name: "B"));
            wheel.AddSlot(Wheel.Create(name: "C"));

            wheel.AddSubslot("B", Wheel.Create(name: "D"));

            WheelBase eWheel = Wheel.Create(name: "E");
            eWheel.AddSlot(Wheel.Create(name: "F"));
            eWheel.AddSlot(Wheel.Create(name: "G"));

            eWheel.AddSubslot("F", Wheel.Create(name: "H"));
            eWheel.AddSubslot("F", Wheel.Create(name: "I"));

            wheel.AddSlot(eWheel);

            string[] givenLeaves = { "A", "C", "D", "G", "H", "I" };

            CollectionAssert.AreEquivalent(wheel.LeafNames, givenLeaves);
        }

        private WheelBase CreateDummyWheelWithDepth(int depth, bool getLeaf = false)
        {
            WheelBase wheel = Wheel.Create();
            WheelBase lastChild = wheel;

            for (int i = 0; i < depth; i++)
            {
                WheelBase newChild = Wheel.Create();
                lastChild.AddSlot(newChild);
                lastChild = newChild;
            }

            if (getLeaf)
                return lastChild;
            else
                return wheel;
        }
    }
}