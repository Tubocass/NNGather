using UnityEngine;
using TMPro;

namespace Gather.UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        TMP_Text scoreText;
        Counter counter;
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
            scoreText.text = counter?.GetAmount().ToString();
        }
    }
}
