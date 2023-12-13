using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventorySlot parentInventorySlot;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Items item = parentInventorySlot.GetItem();
        if (item != null)
        {
            ToolTipManager.instance.SetandShowToolTip(item.description);
        }

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

        Items item = parentInventorySlot.GetItem();
        if (item == null)
        {
            ToolTipManager.instance.HideToolTip();
        }
        ToolTipManager.instance.HideToolTip();
    }


}
