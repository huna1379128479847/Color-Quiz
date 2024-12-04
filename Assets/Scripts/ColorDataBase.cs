using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorQuiz
{
    [CreateAssetMenu(menuName = "ColorData")]
    public class ColorDataBase : ScriptableObject
    {
        public Color backGround;
        public List<ColorData> colorDatas;
    }

    [Serializable]
    public class ColorData
    {
        public string name;
        public Color color;
    }
}