using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExpUiManager : MonoBehaviour
    {
        [SerializeField]
        private Text levelText;
        [SerializeField]
        private Slider expSlider;
    
        public void UpdateExpBar(int currentExp, int expToNextLevel, int currentLevel)
        {
            float fillExp = (float) currentExp / expToNextLevel;

            if (expSlider != null)
            {
                expSlider.value = Mathf.Lerp(expSlider.value, fillExp, Time.deltaTime * 5f);
            }

            if (levelText != null)
            {
                levelText.text = $"{currentLevel}";
            }
        }
    }
}
