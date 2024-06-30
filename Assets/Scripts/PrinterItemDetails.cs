using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrinterItemDetails : MonoBehaviour
{
    // Details to be show in the 3D printer
    [SerializeField] public string itemName = "Test Name";
    [Tooltip("The item name will be displayed in the 3D printer when previewing")]
    [TextArea(4,8)]
    [SerializeField] public string itemDescription = "Test description";
    [Tooltip("The item description will be displayed in the 3D printer when previewing")]
    [SerializeField] public int itemCost = 100;
    [Tooltip("The cost of the item")]

    public void _()
    {
        return;
    }
}
