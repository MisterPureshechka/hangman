using Core;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public class GameState : BaseState
    {
        public new class Data : BaseState.Data
        {
            private readonly string _targetWord;
            public string TargetWord => _targetWord;

            public Data(MainScreen mainScreen, string screenName, string targetWord)
                : base(mainScreen, screenName)
            {
                _targetWord = targetWord;
            }
        }

        [SerializeField] private GameObject _rootGO;
        [SerializeField] private GameObject _keyboardRootGO;
        [SerializeField] private GameObject _hangmanRootGO;

        [SerializeField] private GameObject _letterUnitGO;
        [SerializeField] private GameObject _keyboardUnitGO;

        private Data _data;

        private readonly List<GameObject> _letters = new();
        private readonly List<GameObject> _keys = new();

        private int _lives;

        public override void OnStart(BaseState.Data data)
        {
            base.OnStart(data);

            _data = (Data)data;

            _letterUnitGO.SetActive(false);
            foreach (var c in _data.TargetWord)
            {
                var letterGO = GameObject.Instantiate(_letterUnitGO, _letterUnitGO.transform.parent);
                var letterLabel = letterGO.GetComponentInChildren<Text>();

                letterLabel.text = c.ToString();
                letterLabel.gameObject.SetActive(false);
                letterGO.SetActive(true);

                _letters.Add(letterGO);
            }

            _keyboardUnitGO.SetActive(false);
            for (char c = 'A'; c <= 'Z'; c++)
            {
                var buttonGO = GameObject.Instantiate(_keyboardUnitGO, _keyboardUnitGO.transform.parent);
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

            _rootGO.SetActive(true);
            _keyboardRootGO.SetActive(true);
        }

        public override void OnEnd()
        {
            _rootGO.SetActive(false);
            _keyboardRootGO.SetActive(false);

            for (var i = 0; i < _letters.Count; i++)
                GameObject.Destroy(_letters[i]);
            _letters.Clear();

            for (var i = 0; i < _keys.Count; i++)
                GameObject.Destroy(_keys[i]);
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
                    _data.MainScreen.ChangeToEndState();
            }
            else
            {
                _lives--;

                for (var i = 0; i < _hangmanRootGO.transform.childCount; i++)
                    _hangmanRootGO.transform.GetChild(i).gameObject.SetActive(_lives < (_hangmanRootGO.transform.childCount - i));

                if (_lives <= 0)
                    _data.MainScreen.ChangeToEndState();
            }
        }
    }
}
