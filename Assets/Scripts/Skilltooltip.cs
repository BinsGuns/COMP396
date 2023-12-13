using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Skills;

public class Skilltooltip : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button skillSlot;
    private Skills skill;

    public void Start()
    {
        skill = FindObjectOfType<Skills>();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        
        if (skillSlot != null)
        {

            if(skillSlot.name == "Skill1")
            {
                // Access the meteorCircle instance
                MeteorCircleSkill meteorCircleInstance = skill.meteorCircle;
                ToolTipManager.instance.SetandShowToolTip(meteorCircleInstance.getDescription());
            }

            
        }

    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (skillSlot == null)
        { 
           ToolTipManager.instance.HideToolTip();
        }
        ToolTipManager.instance.HideToolTip();

    }

}
