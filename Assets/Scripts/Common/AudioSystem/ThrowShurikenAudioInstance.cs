using UnityEngine;

[CreateAssetMenu(fileName = "NewThrowShurikenAudioInstance", menuName = "ThrowShurikenAudioInstance")]
public class ThrowShurikenAudioInstance : AudioInstance
{
    [SerializeField] private int _maxSources = 4;
    
    public override void Play(AudioSourceController source)
    {
        if (Sources.Count >= _maxSources) return;
        base.Play(source);
    }
}
