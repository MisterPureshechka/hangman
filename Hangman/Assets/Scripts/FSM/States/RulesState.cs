using Data;

using System;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public sealed class RulesState : BaseState
    {
        [SerializeField] private RulesStateConfig _config;

        [SerializeField] private Text _rulesTextArea;
        [SerializeField] private Button _playButton;

        private Text _buttonLabelTextArea;
        private Text ButtonLabelTextArea => _buttonLabelTextArea == null
            ? _buttonLabelTextArea = _playButton.GetComponentInChildren<Text>()
            : _buttonLabelTextArea;

        protected override BaseStateConfig Config => _config;

        public override void OnStart(Data data)
        {
            base.OnStart(data);

            _rulesTextArea.text = _config.Rules;
            ButtonLabelTextArea.text = _config.ButtonLabel;

            _playButton.onClick.AddListener(OnStartButtonPressed);
        }

        public override void OnEnd()
        {
            base.OnEnd();

            _playButton.onClick.RemoveListener(OnStartButtonPressed);
        }

        private void OnStartButtonPressed() => _mainScreen.ChangeToGameState();
    }
}
