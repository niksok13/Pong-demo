using System;
using UnityEngine;

namespace NSTools
{
    public abstract class ZModelBind :MonoBehaviour
    {
        
        public string key;
        public bool global;
        protected void Awake()
        {
            if (global)
                Model.BindGlobal(key,SetValue);
            else
                Model.Bind(key,SetValue);
        }

        protected abstract void SetValue(object arg);

        protected void OnDestroy()
        {
            if (global)
                Model.UnbindGlobal(key,SetValue);
            else
                Model.Unbind(key,SetValue);
        }
    }
}