using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{   
    public float gradientSpeed = 10;
    // UI Variables
    private Text _textScore;
    private Text _textCombo;
    private Image _comboProgressBar;
    private RectTransform _progressEdge;
    private RectTransform _comboParticlesEdge;
    private ParticleSystemRenderer _comboParticles;


    private float _hue;
    private float _sat;
    private float _bri;

    private void Start()
    {
        _comboProgressBar = transform.Find("Combo Progress").GetComponent<Image>();
        _progressEdge = _comboProgressBar.GetComponent<RectTransform>();

        _comboParticlesEdge = _comboProgressBar.transform.Find("Combo Particles").GetComponent<RectTransform>();
        _comboParticles = _comboParticlesEdge.GetComponent<ParticleSystemRenderer>();

        _textCombo = transform.Find("Combo Multiplier").GetComponent<Text>();
        _textScore = transform.Find("Score").GetComponent<Text>();
    }

    private void Update()
    {
        Color.RGBToHSV(_comboProgressBar.color, out _hue, out _sat, out _bri);

        _hue += gradientSpeed / 7500;
        if(_hue >= 1) _hue = 0;

        _comboProgressBar.color = Color.HSVToRGB(_hue, _sat, _bri);

        Vector3 newAnchor = _comboParticlesEdge.anchoredPosition;
        newAnchor.x = -_progressEdge.rect.width / 2;
        newAnchor.x += _progressEdge.rect.width * _comboProgressBar.fillAmount;

        _comboParticlesEdge.anchoredPosition = newAnchor;
        _comboParticles.material.color = Color.HSVToRGB(_hue, _sat , _bri);
    }

    public void UpdateComboText(int progress, int multiplier, bool updateMultiplier)
    {   
        if (progress > 0)
        {
            _comboParticles.gameObject.SetActive(true);
        }
        else
        {
            _comboParticles.gameObject.SetActive(false);
        }

        _comboProgressBar.fillAmount = (float) progress / (multiplier * 10);

        if (updateMultiplier)
        {
            _textCombo.text = "x" + multiplier;
        }
    }

    public void UpdateScoreText(int score)
    {   
        _textScore.text = score.ToString();
    }
}
