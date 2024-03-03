using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class Helper
    {

        public static Vector3 Abs(this Vector3 vec)
        {
            return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
        }
        public static float mod(float a, float b)
        {
            return a - b * Mathf.Floor(a / b);
        }
        public static int mod(int a, int b)
        {
            return a - b * (int)Mathf.Floor((float)a / b);
        }
        public static Color PowerColorToColor(PowerColor pc)
        {
            switch (pc)
            {
                case PowerColor.Gray: return new Color(.5f, .5f, .5f);
                case PowerColor.Red: return new Color(1.0f, 0.47843137254901963f, 0.34901960784313724f);
                case PowerColor.Yellow: return new Color(0.8470588235294118f, 0.5450980392156862f, 0.2f);
                case PowerColor.Blue: return new Color(0.12549019607843137f, 0.4745098039215686f, 1.0f);
                case PowerColor.Purple: return new Color(0.6784313725490196f, 0.4470588235294118f, 0.9137254901960784f);
                case PowerColor.Green: return new Color(0.10588235294117647f, 0.7176470588235294f, 0.5333333333333333f);
            }
            return Color.black;
        }
    }
}
