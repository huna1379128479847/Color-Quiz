using System;
using System.Collections.Generic;

namespace ColorQuiz.SaveData
{
    [Serializable]
    public class HighScore : IEnumerable<int>
    {
        public string ColorName = "DefaultColor";
        public int First = 0;
        public int Second = 0;
        public int Third = 0;

        public IEnumerator<int> GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[int index]
        {
            get
            {
                return index switch
                {
                    0 => First,
                    1 => Second,
                    2 => Third,
                    _ => throw new IndexOutOfRangeException("Index must be 0, 1, or 2.")
                };
            }
            set
            {
                switch (index)
                {
                    case 0: First = value; break;
                    case 1: Second = value; break;
                    case 2: Third = value; break;
                    default: throw new IndexOutOfRangeException("Index must be 0, 1, or 2.");
                }
            }
        }
    }

    [Serializable]
    public class HighScores
    {
        public List<HighScore> Scores = new List<HighScore>();
    }
}
