using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ColorQuiz
{
    public class GameOver : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.transform
                .DOScale(1.2f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .Play();
        }
    }
}