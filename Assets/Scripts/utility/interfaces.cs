using UnityEngine.SceneManagement;

namespace HighElixir.Utilities
{
    public struct SceneManagerArgumants
    {
        public string fromScene;
    }
    public interface ISceneManagement
    {
        void Execute(SceneManagerArgumants args);
    }

    public interface ISceneHandler
    {
        Scene FromScene { get; }
        Scene CurrentScene { get; }
    }
}