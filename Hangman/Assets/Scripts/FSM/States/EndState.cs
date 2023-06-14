using Core;

using System;

using UnityEngine;
using UnityEngine.UI;

namespace FSM.States
{
    [Serializable]
    public class EndState : BaseState
    {
        public new class Data : BaseState.Data
        {
            private readonly bool _isWin;
            public bool IsWin => _isWin;

            public Data(MainScreen mainScreen, string screenName, bool isWin)
                : base(mainScreen, screenName)
            {
                _isWin = isWin;
            }
        }

        [SerializeField] private GameObject _rootGO;
        [SerializeField] private GameObject _stateRootGO;
        [SerializeField] private Button _restartButton;

        private Data _data;

        public override void OnStart(BaseState.Data data)
        {
            base.OnStart(data);

            _restartButton.onClick.AddListener(Restart);

            _rootGO.SetActive(true);
            _stateRootGO.SetActive(true);

            _data = (Data)data;
        }

        public override void OnEnd()
        {
            _restartButton.onClick.RemoveListener(Restart);

            _rootGO.SetActive(false);
            _stateRootGO.SetActive(false);
        }

        private void Restart()
        {
            _data.MainScreen.ChangeToGameState();
        }
    }
}

