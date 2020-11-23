using System;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerRacketController : BaseRacketController, IDragHandler
{
    public void OnDrag(PointerEventData eventData) => ProcessDrag(eventData);
}
