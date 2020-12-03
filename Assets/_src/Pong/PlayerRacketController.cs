using System;
using NSTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerRacketController : BaseRacketController, IDragHandler
{
    private Text label;
    private void Start()
    {
        label = GetComponent<Text>();
        Model.Bind("player_name",UpdateName);
    }

    private void UpdateName(object obj)
    {
        label.text = Model.Get("player_name","");
    }

    public void OnDrag(PointerEventData eventData) => ProcessDrag(eventData);
}
