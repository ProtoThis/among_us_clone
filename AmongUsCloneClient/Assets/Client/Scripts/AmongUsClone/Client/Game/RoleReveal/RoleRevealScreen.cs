using System.Collections;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Shared.Game;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.RoleReveal
{
    public class RoleRevealScreen : MonoBehaviour
    {
        [SerializeField] private PlayersManager playersManager;

        [SerializeField] private GameObject shhhScreen;
        [SerializeField] private GameObject crewmateEnvironment;
        [SerializeField] private GameObject impostorEnvironment;
        [SerializeField] private Text impostorsAmountExplanationLabel;
        [SerializeField] private Text impostorsAmountLabel;
        [SerializeField] private GameObject playersContainer;
        [SerializeField] private ClientPlayer[] playerDummies;
        [SerializeField] private GameObject curtains;
        [SerializeField] private GameObject leftCurtain;
        [SerializeField] private GameObject rightCurtain;

        public float curtainOpeningSpeed = 0.1f;

        public void ShowRole()
        {
            shhhScreen.SetActive(false);

            if (playersManager.controlledClientPlayer.basePlayer.impostorable.isImpostor)
            {
                impostorEnvironment.SetActive(true);
            }
            else
            {
                UpdateImpostorsAmountLabel();
                crewmateEnvironment.SetActive(true);
            }

            UpdatePlayers(playersManager.controlledClientPlayer.basePlayer.impostorable.isImpostor);
            playersContainer.SetActive(true);

            curtains.SetActive(true);
            StartCoroutine(OpenCurtain());
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        // This Coroutine will stop the recursion when the RoleReveal scene unloads
        private IEnumerator OpenCurtain()
        {
            yield return new WaitForEndOfFrame();

            leftCurtain.transform.position -= new Vector3(curtainOpeningSpeed, 0f, 0f);
            rightCurtain.transform.position += new Vector3(curtainOpeningSpeed, 0f, 0f);

            StartCoroutine(OpenCurtain());
        }

        public static void UpdateCamera()
        {
            // Todo: Add different camera to every scene
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = null;
            playerCamera.transform.position = new Vector3(0, 0, -7);
            playerCamera.shaking = false;
        }

        private void UpdatePlayers(bool impostorsOnly)
        {
            int playerDummyIndex = 0;

            // Update colors of active dummies
            for (int playerId = 0; playerId < playersManager.players.Count; playerId++)
            {
                ClientPlayer clientPlayer = playersManager.players[playerId];

                if (impostorsOnly && !clientPlayer.basePlayer.impostorable.isImpostor)
                {
                    continue;
                }

                playerDummies[playerDummyIndex].basePlayer.colorable.ChangeColor(clientPlayer.basePlayer.colorable.color);

                playerDummyIndex++;
            }

            // Hide not active dummies
            while (playerDummyIndex < GameConfiguration.MaxPlayersAmount)
            {
                playerDummies[playerDummyIndex].gameObject.SetActive(false);
                playerDummyIndex++;
            }
        }

        private void UpdateImpostorsAmountLabel()
        {
            if (playersManager.impostorsAmount == 1)
            {
                impostorsAmountExplanationLabel.text = "There is                   among us";
                impostorsAmountLabel.text = "an imposter";
            }
            else
            {
                impostorsAmountExplanationLabel.text = "There are                     among us";
                impostorsAmountLabel.text = $" {playersManager.impostorsAmount} impostors";
            }
        }
    }
}
