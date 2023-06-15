using System.Collections.Generic;

using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameStateConfig", menuName = "ScriptableObjects/GameStateConfig")]
    public sealed class GameStateConfig : BaseStateConfig
    {
        [SerializeField] private string _resultStatus;
        public string ResultStatus => _resultStatus;

        [SerializeField] private string[] _words;
        public string[] Words => _words;

        [SerializeField] private char _firstChar;
        [SerializeField] private char _lastChar;
        public char[] Chars
        {
            get
            {
                var results = new List<char>();
                for (char c = _firstChar; c <= _lastChar; c++)
                    results.Add(c);
                return results.ToArray();
            }
        }
    }
}
