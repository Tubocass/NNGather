using UnityEngine;
using TMPro;
using gather;

namespace Gather.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        TMP_Text scoreText;
        public Counter counter;
        private void Awake()
        {
            scoreText = GetComponentInChildren<TMP_Text>();
        }
        
        public void SetCounter(Counter counter)
        {
            this.counter = counter;
            counter.counterEvent.AddListener(UpdateText);
            UpdateText();
        }

        public void UpdateText() 
        {
            scoreText.text = counter?.Amount.ToString();
        }
    }
}
