using UnityEngine;
using System.Collections;
using TMPro;
using Sirenix.OdinInspector;

namespace LD49.GUIElements
{
    public class TeleType : MonoBehaviour
    {
        [BoxGroup("Component Dependencies")]
        public TextMeshProUGUI TextMesh;
        [BoxGroup("Component Dependencies")]
        public AudioSource AudioSource;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator MochiAnimator;
        [BoxGroup("Component Dependencies")]
        public StoryAnimator BunnerlyAnimator;

        [BoxGroup("Audio Settings")]
        public AudioClip MochiSound;
        [BoxGroup("Audio Settings")]
        public AudioClip BunnerlySound;

        private int _visibleCount;
        private int _totalVisible;
        private int _counter = 0;
        private bool _holding = false;

        public StoryText StoryText { get; set; }
        public Speaker CurrentSpeaker { get; set; }

        public void Play()
        {
            TextMesh.text = StoryText.GetNext();
            TextMesh.ForceMeshUpdate(true);
            StartCoroutine(ShowText());
        }

        public void Stop()
        {
            StopAllCoroutines();
            _counter = 0;
            _visibleCount = 0;
            _totalVisible = 0;
            _holding = false;
        }

        public void Next()
        {
            Stop();
            _counter = 0;
            _visibleCount = 0;
            _totalVisible = 0;
            _holding = false;
            Play();
        }

        public IEnumerator ShowText()
        {
            AudioSource.clip = CurrentSpeaker == Speaker.Mochi ? MochiSound : BunnerlySound;

            _totalVisible = TextMesh.textInfo.characterCount;

            while ( true && !_holding )
            {
                _visibleCount = _counter % (_totalVisible + 1);
                TextMesh.maxVisibleCharacters = _visibleCount;

                if ( _visibleCount >= _totalVisible && _totalVisible > 0 )
                {
                    _holding = true;
                }

                if (_counter < TextMesh.text.Length)
                {
                    if (TextMesh.text[_counter].ToString() != " ")
                    {
                        // Debug.Log(_counter + " : " + TextMesh.text.Length);
                        AudioSource.Play();
                    }
                }

                _counter += 1;

                yield return new WaitForSeconds(0.08f);
            }
        }
    }
}
