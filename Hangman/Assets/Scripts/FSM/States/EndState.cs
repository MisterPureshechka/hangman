using Data;

using System;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public sealed class EndState : BaseState
    {
        public sealed new class Data : BaseState.Data
        {
            private readonly bool _isWin;
            public bool IsWin => _isWin;

            private readonly int _wins;
            public int Wins => _wins;

            private readonly int _fails;
            public int Fails => _fails;

            public Data(bool isWin, int wins, int fails)
            {
                _isWin = isWin;
                _wins = wins;
                _fails = fails;
            }
        }

        [SerializeField] private EndStateConfig _config;

        [SerializeField] private GameObject _stateRootGO;
        [SerializeField] private Button _restartButton;

        [SerializeField] private Text _winFailTextArea;
        [SerializeField] private Text _playerStatusTextArea;

        private Text _buttonLabelTextArea;
        private Text ButtonLabelTextArea => _buttonLabelTextArea == null
            ? _buttonLabelTextArea = _restartButton.GetComponentInChildren<Text>()
            : _buttonLabelTextArea;

        protected override BaseStateConfig Config => _config;

        public override void OnStart(BaseState.Data data)
        {
            base.OnStart(data);

            _restartButton.onClick.AddListener(Restart);
            _stateRootGO.SetActive(true);

            var endStateData = (Data)data;
            ButtonLabelTextArea.text = _config.ButtonLabel;
            _winFailTextArea.text = _config.GetMatchResult(endStateData.IsWin);
            _playerStatusTextArea.text = string.Format(_config.ResultStatus, endStateData.Wins, endStateData.Fails);
        }

        public override void OnEnd()
        {
            base.OnEnd();

            _restartButton.onClick.RemoveListener(Restart);
            _stateRootGO.SetActive(false);
        }

        private void Restart() => _mainScreen.ChangeToGameState();
    }
}

