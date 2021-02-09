using System;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;

namespace AmongUsClone.Client.UI.Buttons
{
    public class MinimapButton : MonoBehaviour
    {
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private InputReader inputReader;

        public GameObject minimap;
        public SettingsButton settingsButton;

        public Action<bool> minimapToggled;

        private void OnEnable()
        {
            inputReader.onToggleMinimap += Toggle;
        }

        private void OnDisable()
        {
            inputReader.onToggleMinimap -= Toggle;
        }

        public void Toggle()
        {
            if (settingsButton.SettingsMenuActive)
            {
                settingsButton.ToggleMenu();
                return;
            }

            if (playGamePhase.clientSkeld.adminPanel.isControlledPlayerViewing)
            {
                playGamePhase.clientSkeld.adminPanel.Interact();
                return;
            }

            minimap.SetActive(!minimap.activeSelf);
            minimapToggled?.Invoke(minimap.activeSelf);
        }
    }
}
