using UnityEngine;
using UnityEngine.UI;
using HighElixir.Utilities;

namespace ColorQuiz
{
    public class LifeCounter : SingletonBehavior<LifeCounter>
    {
        [SerializeField] AudioSource _damage;
        [SerializeField] Image _life;
        [SerializeField] Image _shild;
        [SerializeField] GameObject _gameover;
        public int shildDamage = 0;
        public int lifeDamage = 0;
        public bool changeFlag = false;

        protected override void Awake()
        {
            base.Awake();
            ResetGame();
        }
        private void Update()
        {
            if (changeFlag)
            {
                _life.fillAmount = CalcRatio(lifeDamage);
                _shild.fillAmount = CalcRatio(shildDamage);
                changeFlag = false;
            }
        }

        public bool IsGameOver() => lifeDamage >= 3;
        public float CalcRatio(float x) => 1 - 0.33f * x;
        public void AddShildDamage()
        {
            shildDamage++;
            if (shildDamage >= 3)
            {
                AddLifeDamage();
                shildDamage = 0;
            }
            changeFlag = true;
        }
        
        public void AddLifeDamage()
        {
            _damage.Play();
            lifeDamage++;
            Director.instance.ResetButton();
            if (IsGameOver())
            {
                GameOver();
            }
            changeFlag = true;
        }

        public void GameOver()
        {
            _gameover.SetActive(true);
            _shild.gameObject.SetActive(false);
            _life.gameObject.SetActive(false);
        }
        public void ResetGame()
        {
            ResetLife();
            ResetShild();
            _gameover.SetActive(false);
            _life.gameObject.SetActive(true);
            _shild.gameObject.SetActive(true);
        }

        public void ResetLife()
        {
            lifeDamage = 0;
            changeFlag = true;
        }
        public void ResetShild()
        {
            shildDamage = 0;
            changeFlag = true;
        }
    }
}