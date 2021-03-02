using UnityEngine;
using System.Collections.Generic;


namespace RectangleTrainer.WheelOfPseudoFortune.Model
{
    /// <summary>
    /// In this demo, let's create a practically useless wheel for the sake of
    /// probabilities. We create a two dimensional bar chart, and plot out
    /// the result of n number of consecutive rolls. The wheel has depth of 2.
    /// One axis plots the distribution of first level results, and the other
    /// axis plots the distribution of the second level results.
    ///
    /// If the Wheel works correctly, it should produce a beautiful bell curve
    /// that continually gets more refined. (Spoiler: I wrote this comment after
    /// completing the code below)
    /// 
    /// You can change the values on the monobehaviour during runtime to see how
    /// it changes the result in real time.
    /// </summary>
    public class NoGUI_AStatisticalStudy_Bootstrapper : DemoBase
    {
        [SerializeField, Range(10, 100)] private int resolution = 10;

        [SerializeField, Range(1, 100)] private uint probability1 = 1;
        [SerializeField, Range(1, 100)] private uint probability2 = 1;

        [SerializeField, Range(1, 100)] private uint probability11 = 1;
        [SerializeField, Range(1, 100)] private uint probability12 = 1;

        [SerializeField, Range(1, 100)] private uint probability21 = 1;
        [SerializeField, Range(1, 100)] private uint probability22 = 1;

        private Dictionary<string, uint> prob = new Dictionary<string, uint>();
        private int res;

        Transform[,] barTransforms;
        int[,] barHeights;

        float baseHeight = 0.01f;
        float heightMultiplier = 10;
        GameObject barChart;
        int burst = 100;

        int BarMax
        {
            get
            {
                int max = 0;
                for (int i = 0; i < res; i++)
                {
                    for (int j = 0; j < res; j++)
                    {
                        if (barHeights[j, i] > max)
                            max = barHeights[j, i];
                    }
                }

                return max;
            }
        }

        float BarMaxF { get => BarMax; }

        private void Awake()
        {
            InitializeVariables();
        }

        void InitializeVariables()
        {
            res = resolution;
            prob.Add("1", probability1);
            prob.Add("2", probability2);

            prob.Add("11", probability11);
            prob.Add("12", probability12);

            prob.Add("21", probability21);
            prob.Add("22", probability22);
        }

        override protected void Start()
        {
            base.Start();
            Initialize2DBarChart();
        }

        private void Initialize2DBarChart()
        {
            if (barChart)
                Destroy(barChart);

            barChart = new GameObject();
            barChart.name = "bar chart";

            barTransforms = new Transform[res, res];
            barHeights = new int[res, res];

            for (int i = 0; i < res; i++)
            {
                for (int j = 0; j < res; j++)
                {
                    barHeights[j, i] = 0;
                    GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    bar.name = $"{j}-{i}";
                    bar.transform.localScale = new Vector3(0.8f, baseHeight, 0.8f);
                    bar.transform.position = new Vector3(j - (res - 1) / 2f, 0, i - (res - 1) / 2f);
                    bar.transform.SetParent(barChart.transform);

                    barTransforms[j, i] = bar.transform;
                    barTransforms[j, i].gameObject.SetActive(false);
                }
            }

            barChart.transform.localScale = new Vector3(10f / res, 1, 10f / res);
        }

        protected override void CreateWheel()
        {
            wheel = Wheel.Create();

            //level 1 slots
            wheel.AddSlot(Wheel.Create(prob["1"], "1_1"));
            wheel.AddSlot(Wheel.Create(prob["2"], "1_2"));

            //level 2 slots
            wheel.AddSubslot("1_1", Wheel.Create(prob["11"], "2_1"));
            wheel.AddSubslot("1_1", Wheel.Create(prob["12"], "2_2"));

            //level 2 slots
            //there are slots sharing names. it's OK in this
            //example, since we are just using them to observe
            //the distribution and we know the nesting.
            wheel.AddSubslot("1_2", Wheel.Create(prob["21"], "2_1"));
            wheel.AddSubslot("1_2", Wheel.Create(prob["22"], "2_2"));
        }

        private void Update()
        {
            RollBurst();
            UpdateBarHeights();

            if(ChangesMade())
            {
                CreateWheel();
                Initialize2DBarChart();
            }
        }

        private void RollBurst()
        {
            for(int i = 0; i < burst; i++)
                RollAndRecord();
        }

        private void RollAndRecord()
        {
            int level1 = 0;
            int level2 = 0;

            for (int i = 0; i < res - 1; i++)
            {
                RollResult result = wheel.Roll();

                //ResultIterator helps you iterate over the roll result,
                //in case you used a nested roll, like this one.
                ResultIterator iterator = new ResultIterator(result);
                iterator.GoToFinal();

                if (iterator.CurrentSlot.name == "2_1")
                    level2++;

                if(iterator.TryMovePrevious())
                {
                    if(iterator.CurrentSlot.name == "1_1")
                        level1++;
                }

            }

            barHeights[level1, level2] ++;
        }

        private void UpdateBarHeights()
        {
            float barMaxF = BarMaxF;
            if (barMaxF == 0)
                return;

            for (int i = 0; i < res; i++)
            {
                for (int j = 0; j < res; j++)
                {
                    float height = heightMultiplier * (barHeights[j, i] / barMaxF);
                    if (height > float.Epsilon)
                    {
                        barTransforms[j, i].gameObject.SetActive(true);
                        barTransforms[j, i].transform.localScale = new Vector3(barTransforms[j, i].transform.localScale.x,
                                                                               height,
                                                                               barTransforms[j, i].transform.localScale.z);

                        barTransforms[j, i].transform.localPosition = new Vector3(barTransforms[j, i].transform.localPosition.x,
                                                                                  barTransforms[j, i].transform.localScale.y / 2,
                                                                                  barTransforms[j, i].transform.localPosition.z);
                    }
                }
            }
        }

        bool ChangesMade()
        {
            bool changed = false;

            if (res != resolution)  { res = resolution; changed = true; }

            if (prob["1"] != probability1) { prob["1"] = probability1; changed = true; }
            if (prob["2"] != probability2) { prob["2"] = probability2; changed = true; }

            if (prob["11"] != probability11) { prob["11"] = probability11; changed = true; }
            if (prob["12"] != probability12) { prob["12"] = probability12; changed = true; }

            if (prob["21"] != probability21) { prob["21"] = probability21; changed = true; }
            if (prob["22"] != probability22) { prob["22"] = probability22; changed = true; }

            return changed;
        }
    }
}