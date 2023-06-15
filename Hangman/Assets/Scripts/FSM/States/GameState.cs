using Data;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public sealed class GameState : BaseState
    {
        public sealed new class Data : BaseState.Data
        {
            private readonly string _targetWord;
            public string TargetWord => _targetWord;

            public Data(string targetWord) => _targetWord = targetWord;
        }

        [SerializeField] private GameStateConfig _config;

        [SerializeField] private GameObject _keyboardRootGO;
        [SerializeField] private GameObject _hangmanRootGO;

        [SerializeField] private GameObject _letterUnitGO;
        [SerializeField] private GameObject _keyboardUnitGO;

        [SerializeField] private Text _playerStatusTextArea;

        private Data _data;

        private readonly List<GameObject> _letters = new();
        private readonly List<GameObject> _keys = new();

        private int _lives;
        private int _wins;
        private int _fails;

        protected override BaseStateConfig Config => _config;

        public override void OnStart(BaseState.Data data)
        {
            base.OnStart(data);

            _data = (Data)data;

            _letterUnitGO.SetActive(false);
            foreach (var c in _data.TargetWord)
            {
                var letterGO = UnityEngine.Object.Instantiate(_letterUnitGO, _letterUnitGO.transform.parent);
                var letterLabel = letterGO.GetComponentInChildren<Text>();

                letterLabel.text = c.ToString();
                letterLabel.gameObject.SetActive(false);
                letterGO.SetActive(true);

                _letters.Add(letterGO);
            }

            _keyboardUnitGO.SetActive(false);
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var buttonGO = UnityEngine.Object.Instantiate(_keyboardUnitGO, _keyboardUnitGO.transform.parent);
                var buttonLabel = buttonGO.GetComponentInChildren<Text>();
                var button = buttonGO.GetComponentInChildren<Button>();

                buttonLabel.text = c.ToString();
                var currentChar = c;
                button.onClick.AddListener(() => OnTap(currentChar));
                buttonGO.SetActive(true);

                _keys.Add(buttonGO);
            }

            _lives = _hangmanRootGO.transform.childCount;
            for (var i = 0; i < _hangmanRootGO.transform.childCount; i++)
                _hangmanRootGO.transform.GetChild(i).gameObject.SetActive(false);

            _keyboardRootGO.SetActive(true);

            _playerStatusTextArea.text = string.Format(_config.ResultStatus, _wins, _fails);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            _keyboardRootGO.SetActive(false);

            for (var i = 0; i < _letters.Count; i++)
                UnityEngine.Object.Destroy(_letters[i]);
            _letters.Clear();

            for (var i = 0; i < _keys.Count; i++)
                UnityEngine.Object.Destroy(_keys[i]);
            _keys.Clear();
        }

        private void OnTap(char c)
        {
            if (_data.TargetWord.Any(ch => ch.ToString().ToUpper() == c.ToString().ToUpper()))
            {
                var isWin = true;
                for (var i = 0; i < _letters.Count; i++)
                {
                    var letterLabel = _letters[i].GetComponentInChildren<Text>(true);
                    if (letterLabel.text.ToUpper() == c.ToString().ToUpper())
                        letterLabel.gameObject.SetActive(true);

                    if (!letterLabel.gameObject.activeSelf)
                        isWin = false;
                }
                if (isWin)
                {
                    _wins++;
                    _mainScreen.ChangeToEndState(true, _wins, _fails);
                }
            }
            else
            {
                _lives--;

                for (var i = 0; i < _hangmanRootGO.transform.childCount; i++)
                    _hangmanRootGO.transform.GetChild(i).gameObject.SetActive(_lives < (_hangmanRootGO.transform.childCount - i));

                if (_lives <= 0)
                {
                    _fails++;
                    _mainScreen.ChangeToEndState(false, _wins, _fails);
                }
            }
        }
    }
}
