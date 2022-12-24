using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCheckerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseCheckerManager.instance.mouseCheck = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseCheckerManager.instance.mouseCheck = false;
    }
}
