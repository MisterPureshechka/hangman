using Core;

using System;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public class RulesState : BaseState
    {
        public new class Data : BaseState.Data
        {
            private readonly string _rules;
            public string Rules => _rules;

            private readonly string _buttonLabel;
            public string ButtonLabel => _buttonLabel;

            public Data(MainScreen mainScreen, string screenName, string rules, string buttonLabel)
                : base(mainScreen, screenName)
            {
                _rules = rules;
                _buttonLabel = buttonLabel;
            }
        }

        [SerializeField] private Text _rulesTextArea;
        [SerializeField] private Button _playButton;
        [SerializeField] private GameObject _rootGO;

        private Text _buttonLabelTextArea;
        private Text ButtonLabelTextArea => _buttonLabelTextArea == null
            ? _buttonLabelTextArea = _playButton.GetComponentInChildren<Text>()
            : _buttonLabelTextArea;

        private Data _data;

        public override void OnStart(BaseState.Data data)
        {
            base.OnStart(data);

            _data = (Data)data;

            _rulesTextArea.text = _data.Rules;
            ButtonLabelTextArea.text = _data.ButtonLabel;

            _playButton.onClick.AddListener(OnStartButtonPressed);

            _rootGO.SetActive(true);
        }

        public override void OnEnd()
        {
            _playButton.onClick.RemoveListener(OnStartButtonPressed);

            _rootGO.SetActive(false);
        }

        private void OnStartButtonPressed() => _data.MainScreen.ChangeToGameState();
    }
}
