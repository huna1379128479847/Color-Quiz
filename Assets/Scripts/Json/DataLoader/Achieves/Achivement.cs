using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorQuiz.Achieves
{
    [Serializable]
    public class Achievement
    {
        public string UniqueName;
        public string Name;
        public string Description;
        public string IconPath;
        public string TargetStats;
        public int Goal;
    }

    [Serializable]
    public class Achievements
    {
        public List<Achievement> achivements = new List<Achievement>();
    }
}
