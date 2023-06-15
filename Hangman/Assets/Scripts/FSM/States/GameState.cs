using Data;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

namespace FSM.States
{
    [Serializable]
    public sealed class GameState : BaseState
    {
        private class Letter
        {
            private readonly GameObject _rootGO;
            private readonly Text _text;

            private char _char;
            public char Char => _char;

            public Letter (GameObject rootGO)
            {
                _rootGO = rootGO;
                _text = _rootGO.GetComponentInChildren<Text>(true);
            }

            public void SetChar(char c) => _char = c;
            public void SetActive(bool state) => _rootGO.SetActive(state);
            public void SetActiveText(bool state) => _text.text = state ? _char.ToString() : string.Empty;
            public bool IsOpened => _text.text != string.Empty;
        }

        private class Key
        {
            public Action<char> OnTap;

            private readonly GameObject _rootGO;
            private readonly Text _text;
            private readonly Button _button;

            private char _char;

            public Key(GameObject rootGO)
            {
                _rootGO = rootGO;
                _text = _rootGO.GetComponentInChildren<Text>(true);
                _button = _rootGO.GetComponentInChildren<Button>(true);

                _button.onClick.RemoveAllListeners();
                _button.onClick.AddListener(() => OnTap.Invoke(_char));
            }

            public void SetActive(bool state) => _rootGO.SetActive(state);

            public void SetText(char c)
            {
                _char = c;
                _text.text = _char.ToString();
            }
        }

        [SerializeField] private GameStateConfig _config;

        [SerializeField] private GameObject _keyboardRootGO;
        [SerializeField] private GameObject _hangmanRootGO;

        [SerializeField] private GameObject _letterUnitGO;
        [SerializeField] private GameObject _keyboardUnitGO;

        [SerializeField] private Text _playerStatusTextArea;

        private readonly List<Letter> _letters = new();
        private readonly List<Key> _keys = new();
        private List<string> _words = new();

        private int _maxLives;
        private int _lives;
        private int _wins;
        private int _fails;

        private string _targetWord;

        protected override BaseStateConfig Config => _config;

        public override void OnStart(Data data)
        {
            base.OnStart(data);

            UpdateTargetWord();

            UpdateLetters();

            UpdateKeys();

            _maxLives = _hangmanRootGO.transform.childCount;
            _lives = _maxLives;

            UpdateHangman();           

            _keyboardRootGO.SetActive(true);

            _playerStatusTextArea.text = string.Format(_config.ResultStatus, _wins, _fails);
        }

        public override void OnEnd()
        {
            base.OnEnd();
            _keyboardRootGO.SetActive(false);

            for (var i = 0; i < _letters.Count; i++)
                _letters[i].SetActive(false);

            for (var i = 0; i < _keys.Count; i++)
                _keys[i].SetActive(false);
        }

        private void OnTap(char c)
        {
            bool? isWin = null;

            var uC = char.ToUpper(c);
            if (_targetWord.Any(ch => char.ToUpper(ch) == uC))
            {
                var isDone = true;
                for (var i = 0; i < _letters.Count; i++)
                {
                    if (char.ToUpper(_letters[i].Char) == uC)
                        _letters[i].SetActiveText(true);

                    if (!_letters[i].IsOpened)
                        isDone = false;
                }
                if (isDone)
                {
                    _words.RemoveAt(0);

                    _wins++;
                    isWin = true;
                }
            }
            else
            {
                _lives--;

                UpdateHangman();

                if (_lives <= 0)
                {
                    _fails++;
                    isWin = false;
                }
            }

            if (isWin.HasValue)
                _mainScreen.ChangeToEndState(isWin.Value, _wins, _fails);
        }

        private void UpdateTargetWord()
        {
            if (_words.Count == 0)
            {
                var rnd = new System.Random();
                _words = _config.Words.OrderBy((item) => rnd.Next()).ToList();
            }

            _targetWord = _words[0];
        }

        private void UpdateLetters()
        {
            _letterUnitGO.SetActive(false);
            var newCounts = _targetWord.Length - _letters.Count;
            for (var i = 0; i < newCounts; i++)
            {
                var letterGO = Object.Instantiate(_letterUnitGO, _letterUnitGO.transform.parent);
                _letters.Add(new Letter(letterGO));
            }

            foreach (var letter in _letters)
                letter.SetActive(false);

            for (var i = 0; i < _targetWord.Length; i++)
            {
                _letters[i].SetChar(_targetWord[i]);
                _letters[i].SetActive(true);
                _letters[i].SetActiveText(false);
            }
        }

        private void UpdateKeys()
        {
            _keyboardUnitGO.SetActive(false);
            var newKeys = _config.Chars.Length - _keys.Count;
            for (var i = 0; i < newKeys; i++)
            {
                var keyGO = Object.Instantiate(_keyboardUnitGO, _keyboardUnitGO.transform.parent);
                var key = new Key(keyGO);

                key.OnTap += OnTap;

                _keys.Add(key);
            }

            foreach (var key in _keys)
                key.SetActive(false);

            for (var i = 0; i < _config.Chars.Length; i++)
            {
                _keys[i].SetText(_config.Chars[i]);
                _keys[i].SetActive(true);
            }
        }

        private void UpdateHangman()
        {
            for (var i = 0; i < _maxLives; i++)
                _hangmanRootGO.transform.GetChild(i).gameObject.SetActive(_lives < (_maxLives - i));
        }
    }
}
