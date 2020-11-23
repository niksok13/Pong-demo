using NSTools;
using UnityEngine;
using UnityEngine.UI;

public class ZBindImageColor : ZModelBind
{
    public Color defaultValue;
    private Image img;

    new void Awake()
    {
        img = GetComponent<Image>();
        base.Awake();
    }

    protected override void SetValue(object obj)
    {
        var value = Model.Get(key, defaultValue);
        Log.Trace($"{this}: {key} = {value}",gameObject);
        img.color = Model.Get(key, defaultValue);
    }

}
