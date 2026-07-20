using Code.EcsStateMachine.Runtime.Logic.Services;
using TMPro;
using UnityEngine;

namespace Code.Demo.Runtime.Ui
{
    public sealed class CurrentStateView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentStateText;

        private void Update()
        {
            currentStateText.text = EcsStateService.CurrentState.ToString();
        }
    }
}