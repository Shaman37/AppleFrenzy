using System;

/// <summary>
///     Class which handles 'Pause View' events and displayed information.
/// </summary>
public class PauseView : BaseView
{   
    // Button Events
    public event Action OnResumeClicked;
    public event Action OnOptionsClicked;
    public event Action OnQuitClicked;

    public void ResumeClick()
    {
        OnResumeClicked?.Invoke();
    }

    public void OptionsClick()
    {
        OnOptionsClicked?.Invoke();
    }

    public void QuitClick()
    {
        OnQuitClicked?.Invoke();
    }
}
