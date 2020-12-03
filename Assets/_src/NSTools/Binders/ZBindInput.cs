using NSTools;
using UnityEngine;
using UnityEngine.UI;

public class ZBindInput : ZModelBind
{
    public string defaultValue = "";
    private InputField img;

    new void Awake()
    {
        img = GetComponent<InputField>();
        img.onEndEdit.AddListener(OnSubmit);
        base.Awake();
    }

    private void OnSubmit(string arg)
    {
        Model.Set(key,arg.ToUpper());
    }

    protected override void SetValue(object obj)
    {
        var value = Model.Get(key, defaultValue);
        Log.Trace($"{this}: {key} = {value}",gameObject);
        img.text = Model.Get(key, defaultValue);
    }

}