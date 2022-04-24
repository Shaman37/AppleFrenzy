using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{   
    // UI Variables
    [SerializeField] private Text textScore;
    [SerializeField] private Text textCombo;
    [SerializeField] private Image comboBar;

    public void UpdateCombo(int comboProgress, int comboMultiplier, bool updateMultiplier)
    {   
        comboBar.fillAmount = (float) comboProgress / (comboMultiplier * 10);

        if (updateMultiplier)
        {
            textCombo.text = "x" + comboMultiplier;
        }
    }

    public void UpdateScore(int score)
    {   
        textScore.text = score.ToString();
    }
}
