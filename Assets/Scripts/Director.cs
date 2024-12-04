using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using ColorQuiz.SaveData;
using HighElixir.Utilities;

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
        [SerializeField] private TMP_Text _tMP_Text;

        private Color _ansColor = Color.blue;
        public List<ColorPallet> colorPallets = new List<ColorPallet>();
        private ColorSetter _colorSetter;
        private StatsManager _statsManager;

        // Start is called before the first frame update
        void Start()
        {
            ColorAlphatoOne();
            RegisterEventListeners();
            _statsManager = GetComponent<StatsManager>();
            _colorSetter = new ColorSetter(Camera.main, colorPallets, _ansColorParet);
            SetColor(_colors[0]);
        }
        protected void Update()
        {
            StatsWatcher.AddPlayTime(Time.deltaTime);
        }
        private int GetClampedDropdownIndex() => Mathf.Clamp(_dropdown.value, 0, _colors.Count);

        private void ColorAlphatoOne()
        {
            foreach (var color in _colors)
            {
                foreach (var data in color.colorDatas)
                {
                    data.color.a = 1f;
                }
            }
        }
        private void RegisterEventListeners()
        {
            _paretReset.onClick.AddListener(ResetButton);
            _resetButton.onClick.AddListener(GameReset);
            _dropdown.onValueChanged.AddListener(OnValueChanged);
            _exitButton.onClick.AddListener(End);
        }

        public void SetColor(ColorDataBase color)
        {
            _ansColor = _colorSetter.SetColor(color);
        }

        public void ResetButton()
        {
            StatsWatcher.IncrementResetCount();

            SetColor(_colors[GetClampedDropdownIndex()]);
            _statsManager.AddScore(-1);
        }

        public void OnValueChanged(int _)
        {
            SetColor(_colors[GetClampedDropdownIndex()]);
        }

        public void ClickTiles(ColorPallet colorPallet)
        {
            if (LifeCounter.instance.IsGameOver()) { return; }
            if (colorPallet.gameObject.name == "Ans") { return; }
            var c = colorPallet.GetColor();
            _tMP_Text.SetText(colorPallet.GetColor().name);
            if (c.color == _ansColor)
            {
                //Debug.Log("correct");
                SetColor(_colors[GetClampedDropdownIndex()]);
                _statsManager.AddScore(1);
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

            SetColor(_colors[GetClampedDropdownIndex()]);
        }

        private async void End()
        {
            Debug.Log("ゲームを終了します");
            _statsManager.SortScoreList();
            HighScore highScore = new HighScore()
            {
                First = _statsManager.highScore[0],
                Second = _statsManager.highScore[1],
                Third = _statsManager.highScore[2]
            };
            await GetComponent<SaveDataLoader>().SaveDataAsync(StatsWatcher.CreateSaveData(), highScore);
            // ゲームを終了させる機能
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; //ゲームプレイ終了
#else
                Application.Quit(); //ゲームプレイ終了
#endif
        }

        
    }
}