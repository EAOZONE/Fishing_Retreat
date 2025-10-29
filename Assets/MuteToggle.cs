using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class MuteToggle : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;
    public string exposedParam = "MasterVolume";
    [Tooltip("dB value used as 'mute' (<= -80 is effectively silent).")]
    public float mutedDb = -80f;

    [Header("UI Hooks (optional)")]
    public UnityEvent onMuted;
    public UnityEvent onUnmuted;

    private bool _muted;
    private float _prevDb = 0f; // remembers non-muted level

    void Awake()
    {
        // Try to read current mixer value as starting point
        if (mixer != null && mixer.GetFloat(exposedParam, out var current))
        {
            _prevDb = Mathf.Max(current, -20f); // default fallback if already low
            _muted = current <= mutedDb + 0.5f;
        }
    }

    public void ToggleMute()
    {
        if (mixer == null) return;

        if (_muted)
        {
            mixer.SetFloat(exposedParam, _prevDb);
            _muted = false;
            onUnmuted?.Invoke();
        }
        else
        {
            if (mixer.GetFloat(exposedParam, out var current))
                _prevDb = current;
            mixer.SetFloat(exposedParam, mutedDb);
            _muted = true;
            onMuted?.Invoke();
        }
    }

    // Optional helpers if you want discrete calls
    public void Mute()  { if (!_muted) ToggleMute(); }
    public void Unmute(){ if (_muted)  ToggleMute(); }
    public bool IsMuted => _muted;
}
