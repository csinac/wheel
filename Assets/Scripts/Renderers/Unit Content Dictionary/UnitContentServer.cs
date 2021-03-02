using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class UnitContentServer<T> : MonoBehaviour
    {
        [SerializeField] protected UnitContentDictionary<T> content;
        public UnitContentDictionary<T> Dictionary { get => content; }

        private void Awake()
        {
            content.Initialize();
        }
    }
}
