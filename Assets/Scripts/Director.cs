using ColorQuiz.SaveData;
using Cysharp.Threading.Tasks;
using HighElixir;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ColorQuiz
{
    [DefaultExecutionOrder(1)]
    public class Director : SingletonBehavior<Director>
    {
        [Header("音源")]
        [SerializeField] private AudioSource resetGame;


        [Header("カラーパレットオブジェクト")]
        [SerializeField] private List<ColorDataBase> _colors = new List<ColorDataBase>();
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private ColorPallet _ansColorParet;

        [Header("UIボタン")]
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _paretReset;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _colorName;

        public List<ColorPallet> colorPallets = new List<ColorPallet>();
        private ColorSetter _colorSetter;
        private StatsManager _statsManager;

        public string CurrentColorName => _colors[GetClampedDropdownIndex()].name;
        private int GetClampedDropdownIndex() => Mathf.Clamp(_dropdown.value, 0, _colors.Count);

        private void RegisterEventListeners()
        {
            _paretReset.onClick.AddListener(ResetButton);
            _resetButton.onClick.AddListener(GameReset);
            _dropdown.onValueChanged.AsObservable().Subscribe(_ =>
            {
                _colorSetter.SetColor(_colors[GetClampedDropdownIndex()]);
            }).AddTo(this);
            _exitButton.onClick.AsObservable().Subscribe(_ =>
            {
                var highScore = _statsManager.SortHighScore();
                var unused = ApplicationDirector.Quit(highScore);
            }).AddTo(this);
        }

        public void ResetButton()
        {
            StatsWatcher.IncrementResetCount();
            _colorSetter.SetColor(_colors[GetClampedDropdownIndex()]);
            _statsManager.CurrentScore--;
        }


        public void ClickTiles(ColorData data, bool isCollect)
        {
            _colorName.SetText(data.name);
            if (isCollect)
            {
                //Debug.Log("correct");
                _colorSetter.SetColor(_colors[GetClampedDropdownIndex()]);
                _statsManager.CurrentScore++;
                LifeCounter.instance.ResetShild();
                StatsWatcher.RecordCorrectAnswer();
            }
            else
            {
                LifeCounter.instance.AddShildDamage();
                StatsWatcher.RecordIncorrectAnswer();
            }
            StatsWatcher.RecordClick();
        }

        public void GameReset()
        {
            StatsWatcher.ResetClick();
            StatsWatcher.RecordGamePlayed();

            _statsManager.SetHighScore();
            resetGame.Play();
            _dropdown.value = 0;
            _statsManager.ResetScore();
            LifeCounter.instance.ResetGame();

            _colorSetter.SetColor(_colors[GetClampedDropdownIndex()]);
        }

        private async UniTask InitializeAsync()
        {
            Debug.Log("Director InitializeAsync Start");
            await SaveDataLoader.Invoke();
            Debug.Log("Director InitializeAsync End");
            RegisterEventListeners();
            _statsManager = GetComponent<StatsManager>();
            _colorSetter = new ColorSetter(Camera.main, colorPallets, _ansColorParet);
            _colorSetter.SetColor(_colors[0]);
        }
        protected override void Awake()
        {
            base.Awake();
            _ = InitializeAsync();
        }
        protected void FixedUpdate()
        {
            StatsWatcher.AddPlayTime(Time.fixedDeltaTime);
        }
    }
}