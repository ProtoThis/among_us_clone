using System.Collections;
using AmongUsClone.Client.Game.RoleReveal;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "RoleRevealGamePhase", menuName = "RoleRevealGamePhase")]
    public class RoleRevealGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private RoleRevealScreen roleRevealScreen;

        private const int SecondsForRoleExploration = GameConfiguration.SecondsForRoleExploration;
        private const int SecondsBeforeRoleRevealing = GameConfiguration.SecondsForRoleExploration / 2;

        public void Initialize()
        {
            roleRevealScreen = FindObjectOfType<RoleRevealScreen>();

            metaMonoBehaviours.coroutines.StartCoroutine(RevealRole());
            roleRevealScreen.ShelterPlayerGameObjects();
            roleRevealScreen.UpdateCamera();

            ScenesManager.UnloadScene(Scene.Lobby);
        }

        private IEnumerator RevealRole()
        {
            yield return new WaitForSeconds(SecondsBeforeRoleRevealing);

            roleRevealScreen.ShowRole();
            metaMonoBehaviours.coroutines.StartCoroutine(SwitchSceneToSkeld());
        }

        private static IEnumerator SwitchSceneToSkeld()
        {
            yield return new WaitForSeconds(SecondsForRoleExploration);

            ScenesManager.LoadScene(Scene.Skeld);
        }
    }
}
