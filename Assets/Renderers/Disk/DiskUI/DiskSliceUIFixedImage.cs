using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class DiskSliceUIFixedImage: DiskSliceUIBase
    {
        [SerializeField] Color textColor = Color.black;
        public Image Image
        {
            get
            {
                if (image == null)
                    image = GetComponent<Image>();

                return image;
            }
        }

        /// <summary>
        /// Styling Setup for Fixed Image Disk Slice
        /// </summary>
        /// <param name="args">0th element slice image (sprite)</param>
        override protected void SetStyle(params object[] args)
        {
            if (args.Length != 1)
                throw new System.Exception("Parameter count does not match the styling requirements (needs one: slice image (sprite)");

            image = GetComponent<Image>();
            Sprite sprite = (Sprite)args[0];
            image.sprite = sprite;
        }
    }
}
