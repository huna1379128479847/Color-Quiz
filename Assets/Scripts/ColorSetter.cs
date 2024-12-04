using System.Collections.Generic;
using UnityEngine;
using HighElixir.Utilities;

namespace ColorQuiz
{
    public class ColorSetter // 変更点：Directerから色変更機能をすべてこのクラスへ委譲した。
    {
        private List<ColorPallet> _colorPallets = new List<ColorPallet>();
        private Camera _camera;
        private ColorPallet _ansShowPallet;
        public ColorSetter(Camera camera, List<ColorPallet> colorPallets, ColorPallet ansShowPallet)
        {
            _camera = camera;
            _colorPallets = colorPallets;
            _ansShowPallet = ansShowPallet;
        }
        public Color SetColor(ColorDataBase color)
        {
            _camera.backgroundColor = color.backGround;
            HashSet<ColorData> usedIndices = new HashSet<ColorData>(); // 色の重複排除
            _colorPallets.ForEach(colorPallet => 
            {
                var a = CollectionsHelper.RandomPick(color.colorDatas, usedIndices);
                usedIndices.Add(a);
                colorPallet.SetColor(a);
            });
            // 1～9番目の色からランダムに1つ選んで答え表示用のパレットに設定
            return _ansShowPallet.SetColor(CollectionsHelper.RandomPick(_colorPallets).GetColor());
        }
    }
}
