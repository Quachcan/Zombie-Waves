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
            fillExp = Mathf.Clamp01(fillExp);

            if (expSlider != null)
            {
                expSlider.value = fillExp;
            }

            if (levelText != null)
            {
                levelText.text = $"{currentLevel}";
            }
        }
    }
}
