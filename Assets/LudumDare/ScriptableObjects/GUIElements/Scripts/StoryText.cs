using System.Collections.Generic;
using UnityEngine;


namespace LD49.GUIElements
{
    [CreateAssetMenu(fileName = "New Story Text", menuName = "GUIElements/Story Text")]
    public class StoryText : ScriptableObject
    {
        public List<string> StoryScript;

        public int CurrentIndex;

        public string GetNext()
        {
            if ( CurrentIndex < StoryScript.Capacity)
            {
                string NextScript = StoryScript[CurrentIndex];
                CurrentIndex += 1;
                return NextScript;
            }

            return null;
        }
    }
}

