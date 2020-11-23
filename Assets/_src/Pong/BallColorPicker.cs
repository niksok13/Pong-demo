using UnityEngine;
using UnityEngine.UI;

namespace NSTools
{
    public class BallColorPicker : MonoBehaviour
    {
        public Slider sliderRed;
        public Slider sliderGreen;
        public Slider sliderBlue;
        
        new void Awake()
        {
            sliderRed.onValueChanged.AddListener(Slide);
            sliderGreen.onValueChanged.AddListener(Slide);
            sliderBlue.onValueChanged.AddListener(Slide);
            Model.Bind("ball_color",SetValue);
        }

        private void Slide(float value)
        {

            var r = sliderRed.value;
            var g = sliderGreen.value;
            var b = sliderBlue.value;
            FSM.Signal("set_ball_color",new Color(r,g,b));
        }

        protected void SetValue(object arg)
        {
            var value = Model.Get("ball_color", Color.white);
            sliderRed.value = value.r;
            sliderGreen.value = value.g;
            sliderBlue.value = value.b;
        }
      
    }
}