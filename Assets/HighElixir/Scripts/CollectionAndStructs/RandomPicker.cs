using System.Collections.Generic;
using System.Linq;
using System;

namespace HighElixir
{
    /// <summary>
    /// ゲーム内で使う汎用的なヘルパーメソッドをまとめたクラス。
    /// </summary>
    public static class RandomPicker
    {
        private static Random random = new Random();
        /// <summary>
        /// リストからランダムに1つの要素を選ぶ。
        /// </summary>
        /// <typeparam name="T">リストの要素の型。</typeparam>
        /// <param name="values">要素を持つリスト。</param>
        /// <returns>ランダムに選ばれた要素。リストが空またはnullの場合はデフォルト値を返す。</returns>
        public static T RandomPick<T>(this List<T> values)
        {
            if (values == null || values.Count == 0) return default;
            return values[random.Next(0, values.Count)];
        }

        public static T RandomPick<T>(this List<T> values, HashSet<T> exists)
        {
            if (values == null || values.Count == 0) return default;
            List<T> v = values.Where(item => !exists.Contains(item)).ToList();
            // もし未使用の要素がない場合は、デフォルト値を返す。
            if (v.Count == 0) return default;
            return RandomPick(v);
        }
    }
}