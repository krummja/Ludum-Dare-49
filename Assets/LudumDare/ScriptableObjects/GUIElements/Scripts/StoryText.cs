using System.Collections.Generic;
using UnityEngine;


namespace LD49.GUIElements
{
    public enum Speaker
    {
        Mochi,
        Bunnerly,
    }

    [CreateAssetMenu(fileName = "New Story Text", menuName = "GUIElements/Story Text")]
    public class StoryText : ScriptableObject
    {
        public List<string> StoryScript;

        public List<Speaker> SpeakerSequence;
        public List<Expression> MochiSequence;
        public List<Expression> BunnerlySequence;

        public int CurrentIndex;

        public bool Complete;

        public string GetNext()
        {
            if ( CurrentIndex == StoryScript.Capacity - 1 )
            {
                Complete = true;
            }

            if ( CurrentIndex < StoryScript.Capacity)
            {
                string NextScript = StoryScript[CurrentIndex];
                CurrentIndex += 1;
                return NextScript;
            }

            return null;
        }

        public void StartExpressions(StoryAnimator mochi, StoryAnimator bunnerly)
        {
            mochi.Expression = MochiSequence[0];
            mochi.SwitchSprite();

            bunnerly.Expression = BunnerlySequence[0];
            bunnerly.SwitchSprite();
        }

        public void NextMochiExpression(StoryAnimator animator)
        {
            animator.Expression = MochiSequence[CurrentIndex];
            animator.SwitchSprite();
        }

        public void NextBunnerlyExpression(StoryAnimator animator)
        {
            animator.Expression = BunnerlySequence[CurrentIndex];
            animator.SwitchSprite();
        }
    }
}

