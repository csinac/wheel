using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class Console86 : WheelRenderer
    {
        protected enum SizingPriority { MatchRows, MatchColumns }

        [Header("Header Fields")]
        [SerializeField] C86HeaderValue wheelNameHeader;
        [SerializeField] C86HeaderValue depthHeader;
        [SerializeField] C86HeaderValue probabilityHeader;
        [SerializeField] C86HeaderValue randHeader;
        [Header("Footer Fields")]
        [SerializeField] Button initSequenceButton;
        [SerializeField] Text idFieldFooter;
        [SerializeField] Text timer;

        [Header("Entry")]
        [SerializeField] protected C86Entry entryPF;
        [SerializeField] protected Vector2 entryRatio = new Vector2(0.2f, 0.1f);
        [SerializeField] protected float entryFadeDelay = 0.25f;
        [Header("Animation")]
        [SerializeField] protected AnimationCurve sequenceIntervals = AnimationCurve.EaseInOut(0.1f, 0, 1, 0.5f);
        [SerializeField] protected int steps = 50;
        [SerializeField] protected float sequenceDuration = 5;

        protected Dictionary<Vector2Int, UnitRenderer> entryDict;
        Vector2Int entryPosSteps;
        Coroutine countdownCoroutine;

        float wheelotronLoad = 0;
        float wheelotronRange = 10;
        float wheelotronMax = 100;

        protected override void Awake()
        {
            base.Awake();

            entryPosSteps = new Vector2Int(Mathf.RoundToInt(1f / entryRatio.x),
                                            Mathf.RoundToInt(1f / entryRatio.y));

            entryRatio = new Vector2(1f / entryPosSteps.x,
                                      1f / entryPosSteps.y);

            entryDict = new Dictionary<Vector2Int, UnitRenderer>();
        }

        #region Console FX
        private void InitializeUI()
        {
            InitializeHeader();
            InitializeFooter();
        }

        private void InitializeHeader()
        {
            if (wheel == null) return;

            wheelNameHeader.Init("Generation Pool", wheel.name);
            depthHeader.Init("Depth", wheel.Depth);
            probabilityHeader.Init("Total Outcomes", wheel.LeafCount);
            randHeader.Init("Wheelotron\nLoad", Random.Range(0, 100));

            InvokeRepeating("WheelotronEffect", 1, 1);
        }

        private void InitializeFooter()
        {
            idFieldFooter.text = $"ID:{(char)(Random.Range(0, 26) + 'A')}-{Random.Range(1000, 9999)}";
        }

        private void ResetUI()
        {
            if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
            initSequenceButton.interactable = true;
            wheelotronLoad = 0;
            timer.text = "00:00.000";
        }

        private void WheelotronEffect() => randHeader.Init("Wheelotron\nLoad", Random.Range(wheelotronLoad, wheelotronLoad + wheelotronRange));
        #endregion

        #region Model Relation
        public override void SetWheel(WheelBase wheel)
        {
            base.SetWheel(wheel);
            CreateWheel(wheel.LeafSlots);

            InitializeUI();
        }

        public override bool ResetView()
        {
            if (base.ResetView())
            {
                CreateWheel(wheel.LeafSlots);
                ResetUI();
                return true;
            }

            return false;
        }

        override public void RenderResult(object sender, RollResult result)
        {
            base.RenderResult(sender, result);
            if (result.RootSlot == null) return;

            DestroyWheel();

            initSequenceButton.interactable = false;
            RestartCoroutine(ref animationCoroutine, RollAnimation(result));
            RestartCoroutine(ref countdownCoroutine, Countdown(result.FinalSlot.name));
        }
        #endregion

        #region Animation
        protected void RestartCoroutine(ref Coroutine routine, IEnumerator action)
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(action);
        }

        protected IEnumerator Countdown(string resultName)
        {
            for (float timeLeft = sequenceDuration; timeLeft > 0; timeLeft -= Time.deltaTime)
            {
                string sec = Mathf.FloorToInt(timeLeft).ToString("D2");
                string millis = Mathf.FloorToInt((timeLeft % 1) * 1000).ToString("D3");
                timer.text = $"00:{sec}.{millis}";

                yield return new WaitForEndOfFrame();
            }

            timer.text = resultName;
            countdownCoroutine = null;
        }

        protected IEnumerator RollAnimation(RollResult result)
        {
            List<WheelBase> leaves = wheel.LeafSlots;
            int target = leaves.IndexOf(result.FinalSlot);
            if (target < 0) yield break;

            float f = 0;
            float time = Time.time;
            float rawTime = 0;
            for (float i = 0; i < steps; i++) rawTime += sequenceIntervals.Evaluate(i / steps);

            for (float i = 0; i < steps; i++)
            {
                float t = sequenceIntervals.Evaluate(i / steps);
                t /= rawTime / sequenceDuration;
                f += t;

                int randEntry = Random.Range(0, leaves.Count);
                MakeUnit(randEntry, leaves[randEntry]);

                wheelotronLoad += (wheelotronMax - wheelotronLoad) * 0.1f; //FX
                yield return new WaitForSeconds(t);
            }

            UnitRenderer final = MakeUnit(target, leaves[target]);
            final.Highlight(blinking: true);

            //FX
            while (wheelotronLoad > 0)
            {
                wheelotronLoad -= Random.Range(5f, 15f);
                yield return new WaitForSeconds(0.2f);
            }

            wheelotronLoad = 0;
            initSequenceButton.interactable = true;
            animationCoroutine = null;
            yield return null;
        }

        #endregion

        #region Entry Creation
        protected override UnitRenderer MakeUnit(int order, WheelBase slot)
        {
            Vector2Int pos = FindEntryPosition();
            C86Entry entry = CreateEntry(pos, slot);
            SaveEntry(pos, entry);

            return entry;
        }

        protected Vector2Int FindEntryPosition()
        {
            Vector2Int pos;
            int failsafe = 0;
            bool occupied;
            do
            {
                failsafe++;
                pos = new Vector2Int(Random.Range(0, entryPosSteps.x), Random.Range(0, entryPosSteps.y));
                occupied = entryDict.ContainsKey(pos) && entryDict[pos] != null;
            } while (occupied && failsafe < 99);

            return pos;
        }

        protected C86Entry CreateEntry(Vector2Int pos, WheelBase slot)
        {
            C86Entry entry = Instantiate(entryPF);
            entry.Initialize(unitContainer, pos.x, pos.y, entryRatio.x, entryRatio.y, sequenceDuration / 2f, entryFadeDelay);
            entry.SetContent(slot.name);

            return entry;
        }

        protected void SaveEntry(Vector2Int pos, C86Entry entry)
        {
            if (entryDict.ContainsKey(pos))
            {
                if (entryDict[pos] != null)
                    Destroy(entryDict[pos].gameObject);

                entryDict[pos] = entry;
            }
            else
            {
                entryDict.Add(pos, entry);
            }
        }
        #endregion
    }
}
