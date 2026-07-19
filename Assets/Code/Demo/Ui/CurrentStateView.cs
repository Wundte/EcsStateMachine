using Code.Logic.Services;
using TMPro;
using UnityEngine;

namespace Code.Demo.UI
{
    public sealed class CurrentStateView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentStateText;

        private void Update()
        {
            currentStateText.text = GameStateService.CurrentState.ToString();
        }
    }
}