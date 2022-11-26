using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseMask : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) => transform.parent.gameObject.SetActive(false);
}
