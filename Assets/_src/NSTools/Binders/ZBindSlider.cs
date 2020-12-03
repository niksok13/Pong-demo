using System.Collections;
using System.Collections.Generic;
using NSTools;
using UnityEngine;
using UnityEngine.UI;

public class ZBindSlider : ZModelBind
{
    private Slider _slider;
    // Start is called before the first frame update
    new void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(UpdateValue);    
        base.Awake();
    }

    private void UpdateValue(float arg0)
    {
        Model.Set(key, arg0);
    }

    protected override void SetValue(object arg)
    {
        _slider.value = Model.Get(key, 0f);
    }
}
