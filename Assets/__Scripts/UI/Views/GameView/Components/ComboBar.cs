using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

public class ComboBar : MonoBehaviour
{   
    #region [0] - Fields

    [Header("Rainbow Gradient Settings")]
    [SerializeField] private float _gradientSpeed = 10;
    [SerializeField] private float _colorChangeInterval;

    [Header("UI Variables")]
    [SerializeField] private Text _textScore;
    [SerializeField] private Text _textCombo;
    [SerializeField] private Image _comboProgressBar;
    [SerializeField] private RectTransform _progressEdge;
    [SerializeField] private RectTransform _comboParticlesEdge;
    [SerializeField] private ParticleSystemRenderer _comboParticles;

    // Progress Bar Color Change fields
    private float _hue;
    private float _sat;
    private float _bri;

    // Progress Bar Animation fields
    private bool _inProgressAnimation;
    private float _progressAnimationStart;
    private float _progressAnimationDuration = 0.25f;
    private float _previousFillAmount;
    private Vector3 _previousAnchorPosition;
    private Vector2 _currentCombo;

    #endregion

    #region [1] - Unity Event Methods

    private void Awake()
    {
        if (_comboProgressBar == null) Debug.LogError("Combo Progress Bar Image not found!");
        if (_progressEdge == null) Debug.LogError("Combo Progress Bar Rect Transform not found!");
        if (_comboParticlesEdge == null) Debug.LogError("Combo Particles Rect Transform not found!");
        if (_comboParticles == null) Debug.LogError("Combo Particle System Renderer not found!");
        if (_textCombo == null) Debug.LogError("Text Combo not found!");
        if (_textScore == null) Debug.LogError("Text Score not found!");
    }

    private void Start()
    {
        _comboParticles.gameObject.SetActive(false);
        StartCoroutine(ComboBarColorChange());
    }

    private IEnumerator ComboBarColorChange()
    {
        while(true) 
        { 
            // Calculate the new color for both the combo progress bar and particles
            Color.RGBToHSV(_comboProgressBar.color, out _hue, out _sat, out _bri);

            _hue += _gradientSpeed / 7500;
            if(_hue >= 1) _hue = 0;

            Color newColor = Color.HSVToRGB(_hue, _sat , _bri);

            _comboProgressBar.DOColor(newColor, 1f);

            yield return new WaitForSeconds(1f);
        }
    }

    private void ProgressBarAnimation()
    {
        UpdateComboProgress();
        UpdateParticlesAnchor();
    }

    private void UpdateParticlesAnchor()
    {
        Vector3 newAnchor = _comboParticlesEdge.anchoredPosition;
        newAnchor.x = -(_progressEdge.rect.width / 2);
        newAnchor.x += _progressEdge.rect.width * _comboProgressBar.fillAmount;

        _comboParticlesEdge.DOAnchorPos(newAnchor, 0.2f).SetEase(Ease.InSine);
    }

    private void UpdateComboProgress()
    {
        _previousFillAmount = _comboProgressBar.fillAmount;
        float newFillAmount = _currentCombo.x / (_currentCombo.y * 10);

        _comboProgressBar.DOFillAmount(newFillAmount, 0.2f).SetEase(Ease.InSine);
    }

    #endregion

    #region [2] - UI Update Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="progress"></param>
    /// <param name="multiplier"></param>
    /// <param name="updateMultiplier"></param>
    public void UpdateComboText(int progress, int multiplier, bool updateMultiplier)
    {   
        // Handle particles 
        if (progress > 0)
        {
            _comboParticles.gameObject.SetActive(true);
        }
        else
        {
            _comboParticles.gameObject.SetActive(false);
        }

        _currentCombo = new Vector2(progress, multiplier);

        // Check if a new multiplier was achieved
        if (updateMultiplier) _textCombo.text = "x" + multiplier;

        ProgressBarAnimation();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScoreText(int score)
    {   
        _textScore.text = score.ToString();
    }
    
    #endregion
}
