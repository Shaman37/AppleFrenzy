using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace AppleFrenzy.UI.Components.Combo
{   
    /// <summary>
    ///     Represents the 'Combo Bar' component, holding information about the Score, Combo Multiplier and Combo Progress.
    /// </summary>
    public class ComboBar : MonoBehaviour
    {   
        #region [0] - Fields
    
        [Header("Rainbow Gradient Settings")]
        [SerializeField] private float _gradientSpeed = 10f;
        [SerializeField] private float _colorChangeInterval = 0.15f;
    
        [Header("UI Variables")]
        [SerializeField] private TextMeshProUGUI _textScore;
        [SerializeField] private TextMeshProUGUI _textCombo;
        [SerializeField] private Image _comboProgressBar;
        [SerializeField] private RectTransform _progressEdge;
        [SerializeField] private RectTransform _comboParticlesEdge;
        [SerializeField] private ParticleSystemRenderer _comboParticles;
        [SerializeField] private Material _particlesMat;
    
        // Progress Bar Color Change fields
        private float _hue;
        private float _sat;
        private float _bri;
    
        // Progress Bar Animation fields
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
        }

        private void Update()
        {
            UpdateComboProgressColor();
        }
    
        #endregion
        
        #region [2] - Methods

        /// <summary>
        ///     Responsible for handling the progress bar animation.
        /// </summary>
        private void ProgressBarAnimation()
        {
            if (_comboProgressBar != null && _comboParticlesEdge != null)
            {
                UpdateComboProgress();
                UpdateParticlesAnchor();
            }
        }

        /// <summary>
        ///     Responsible for updating the Particle System anchor, so that the
        ///     particles to follow the edge of the current combo progress bar.
        /// </summary>
        private void UpdateParticlesAnchor()
        {
            Vector3 newAnchor = _comboParticlesEdge.anchoredPosition;
            newAnchor.x = -(_progressEdge.rect.width * 0.5f);
            newAnchor.x += _progressEdge.rect.width * _comboProgressBar.fillAmount;
    
            _comboParticlesEdge.DOAnchorPos(newAnchor, 0.2f).SetEase(Ease.InSine);
        }

        /// <summary>
        ///     Responsible for updating the combo progress bar, by increasing it's fill amount.
        /// </summary>
        private void UpdateComboProgress()
        {
            float newFillAmount = _currentCombo.x / (_currentCombo.y * 10);
    
            _comboProgressBar.DOFillAmount(newFillAmount, 0.2f).SetEase(Ease.InSine);
        }

        /// <summary>
        ///     Responsible for updating the combo progress bar color, which is constantly changing colors.
        /// </summary>
        private void UpdateComboProgressColor()
        {
            if (_colorChangeInterval >= Time.time)
            {
                // Calculate the new color for both the combo progress bar and particles
                Color.RGBToHSV(_comboProgressBar.color, out _hue, out _sat, out _bri);

                _hue += _gradientSpeed / 7500;

                if(_hue >= 1) _hue = 0;
    
                Color newColor = Color.HSVToRGB(_hue, _sat , _bri);
    
                _comboProgressBar.DOColor(newColor, 0.1f);
                _particlesMat.DOColor(newColor, 0.1f);

                _colorChangeInterval += _colorChangeInterval;
            }
        }

        /// <summary>
        ///     Responsible for updating the combo multiplier and progress.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="progress">
        ///         The current combo progress.
        ///     </param>
        ///     <param name="multiplier">
        ///         The current combo multiplier.
        ///     </param>
        ///     <param name="shouldUpdateMultiplier">
        ///         Should combo multiplier be updated or not.
        ///     </param>
        /// </parameters>
        public void UpdateComboText(int progress, int multiplier, bool shouldUpdateMultiplier)
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
            if (shouldUpdateMultiplier) _textCombo.text = "x" + multiplier;
    
            ProgressBarAnimation();
        }
    
        /// <summary>
        ///     Responsible for updating the score text.
        /// </summary>
        /// 
        /// <parameters>
        ///     <param name="score">
        ///         The current score.
        ///     </param>
        /// </parameters>
        public void UpdateScoreText(int score)
        {   
            _textScore.text = score.ToString();
        }
        
        #endregion
    }
}
