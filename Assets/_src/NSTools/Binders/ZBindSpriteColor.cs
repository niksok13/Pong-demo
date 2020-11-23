using NSTools;
using UnityEngine;
using UnityEngine.UI;

public class ZBindSpriteColor : ZModelBind
{
    public Color defaultValue;
    private SpriteRenderer img;

    new void Awake()
    {
        img = GetComponent<SpriteRenderer>();
        base.Awake();
    }

    protected override void SetValue(object obj)
    {
        var value = Model.Get(key, defaultValue);
        Log.Trace($"{this}: {key} = {value}",gameObject);
        img.color = Model.Get(key, defaultValue);
    }
    

}
