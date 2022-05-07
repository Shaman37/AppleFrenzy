using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{   
    [Header("Gradient Effect Settings")]
    [SerializeField] private float gradientSpeed = 10;

    // UI Variables
    private Text _textScore;
    private Text _textCombo;
    private Image _comboProgressBar;
    private RectTransform _progressEdge;
    private RectTransform _comboParticlesEdge;
    private ParticleSystemRenderer _comboParticles;

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

    private void Start()
    {
        // Combo Progress
        _comboProgressBar = transform.Find("Combo Progress").GetComponent<Image>();
        _progressEdge = _comboProgressBar.GetComponent<RectTransform>();
        
        // Combo Particles fields
        _comboParticlesEdge = _comboProgressBar.transform.Find("Combo Particles").GetComponent<RectTransform>();
        _comboParticles = _comboParticlesEdge.GetComponent<ParticleSystemRenderer>();
        _comboParticles.gameObject.SetActive(false);

        // Text Objects
        _textCombo = transform.Find("Combo Multiplier").GetComponent<Text>();
        _textScore = transform.Find("Score").GetComponent<Text>();
    }

    private void Update()
    {   
        if (_inProgressAnimation)
        {
            float u = (Time.time - _progressAnimationStart) / _progressAnimationDuration;
            if(u >= 1)
            {
                u = 1;
                _inProgressAnimation = false;
            }


            // Lerp fill amount increase
            u = Utils.Ease(u, Utils.EasingType.easeIn);
            _comboProgressBar.fillAmount = Mathf.Lerp(_previousFillAmount, _currentCombo.x / (_currentCombo.y * 10), u);

            // Calculate new Anchor Position
            Vector3 newAnchor = _previousAnchorPosition;
            newAnchor.x = - _progressEdge.rect.width / 2;
            newAnchor.x += _progressEdge.rect.width * _comboProgressBar.fillAmount;

            u = Utils.Ease(u, Utils.EasingType.easeOut);
            _comboParticlesEdge.anchoredPosition = Vector3.Lerp(_previousAnchorPosition, newAnchor, u);
        }
        // Update particles anchor position
        

        // Calculate the new color for both the combo progress bar and particles
        Color.RGBToHSV(_comboProgressBar.color, out _hue, out _sat, out _bri);

        _hue += gradientSpeed / 7500;
        if(_hue >= 1) _hue = 0;

        _comboProgressBar.color = Color.HSVToRGB(_hue, _sat, _bri);
        _comboParticles.material.color = Color.HSVToRGB(_hue, _sat , _bri);
    }

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

        // Calculate fill amount of the progress bar, based on current combo stats
        _progressAnimationStart = Time.time;
        _inProgressAnimation = true;
        _previousFillAmount = _comboProgressBar.fillAmount;
        _previousAnchorPosition = _comboParticlesEdge.anchoredPosition;
        _currentCombo = new Vector2(progress, multiplier);

        // Check if a new multiplier was achieved
        if (updateMultiplier) _textCombo.text = "x" + multiplier;
    }

    public void UpdateScoreText(int score)
    {   
        _textScore.text = score.ToString();
    }
}
