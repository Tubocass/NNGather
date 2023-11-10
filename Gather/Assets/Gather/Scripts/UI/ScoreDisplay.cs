using UnityEngine;
using TMPro;
using gather;

namespace Gather.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        TMP_Text scoreText;
        public Counter count;
        private void Awake()
        {
            scoreText = GetComponentInChildren<TMP_Text>();
        }

        //public void AmountChanged(int amount)
        //{
        //    UpdateText();
        //}

        void Update()
        {
            scoreText.text = count?.amount.ToString();
        }
    }
}
