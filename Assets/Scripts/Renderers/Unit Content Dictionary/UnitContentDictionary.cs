using System;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{

    [System.Serializable]
    public struct UnitContent<T>
    {
        public string name;
        public T content;
    }

    [System.Serializable]
    public class UnitContentDictionary<T>
    {
        [SerializeField] private List<UnitContent<T>> inspectorList;
        [SerializeField] private T fallback;

        private Dictionary<string, T> dict;

        public UnitContentDictionary()
        {
            dict = new Dictionary<string, T>();
        }

        public UnitContentDictionary(Dictionary<string, T> dict)
        {
            this.dict = new Dictionary<string, T>();

            foreach(KeyValuePair<string, T> pair in dict)
                Set(pair.Key, pair.Value);
        }

        public UnitContentDictionary(T defaultContent, Dictionary<string, T> dict)
        {
            fallback = defaultContent;
            this.dict = new Dictionary<string, T>();

            foreach (KeyValuePair<string, T> pair in dict)
                Set(pair.Key, pair.Value);
        }

        public UnitContentDictionary(T defaultContent)
        {
            fallback = defaultContent;
            this.dict = new Dictionary<string, T>();
        }

        public void SetDefault(T defaultContent)
        {
            fallback = defaultContent;
        }

        public void Set(string slotName, T unitContent)
        {
            if (dict.ContainsKey(slotName))
                dict[slotName] = unitContent;
            else
                dict.Add(slotName, unitContent);
        }

        public T GetContent(string slotName)
        {
            if (dict.ContainsKey(slotName))
                return dict[slotName];

            return fallback;
        }

        public void Initialize()
        {
            if (dict == null) dict = new Dictionary<string, T>();
            else dict.Clear();

            foreach (UnitContent<T> uc in inspectorList)
                Set(uc.name, uc.content);
        }

        public static implicit operator UnitContentDictionary<T>(UnitContentDictionary<object> v)
        {
            throw new NotImplementedException();
        }
    }
}