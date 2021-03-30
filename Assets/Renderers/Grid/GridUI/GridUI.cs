using RectangleTrainer.WheelOfPseudoFortune.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RectangleTrainer.WheelOfPseudoFortune.Renderer
{
    public class GridUI : GridBase
    {
        [Header("UI Grid Options")]
        [SerializeField] protected GridCellUI cellPF;
        [SerializeField] protected Vector2 padding; 

        protected override UnitRenderer MakeUnit(int order, WheelBase slot)
        {
            float x, y;
            CalculateCellPosition(order, out x, out y);

            GridCellUI cell = Instantiate(cellPF);
            cell.Initialize(x, y, unitContainer, padding);
            cell.SetContent(slot.name);

            return cell;
        }
    }
}