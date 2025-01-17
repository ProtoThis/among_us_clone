using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game
{
    // Todo: fix a bug where players stop moving randomly, but they can connect \ disconnect
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayersManager", menuName = "PlayersManager")]
    public class PlayersManager : ScriptableObject
    {
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PacketsSender packetsSender;

        public BasePlayersManager basePlayersManager;

        public const int MinPlayerId = 0;
        public const int MaxPlayerId = GameConfiguration.MaxPlayersAmount - 1;

        public readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public static bool IsLobbyHost(int playerId)
        {
            return playerId == MinPlayerId;
        }

        public List<int> GetImpostorPlayerIds()
        {
            List<int> impostorPlayerIds = new List<int>();

            foreach (Client client in clients.Values)
            {
                if (client.basePlayer.impostorable.isImpostor)
                {
                    impostorPlayerIds.Add(client.playerId);
                }
            }

            return impostorPlayerIds;
        }

        public void DisconnectPlayer(int playerId)
        {
            if (!clients.ContainsKey(playerId))
            {
                Logger.LogNotice(LoggerSection.Connection, $"Skipping player {playerId} disconnect, because it is already disconnected");
                return;
            }

            Logger.LogEvent(LoggerSection.Connection, $"{clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            if (clients[playerId].serverPlayer.remoteControllable.isLobbyHost)
            {
                packetsSender.SendKickedPacket(playerId);
                foreach (int playerIdToRemove in clients.Keys.ToList())
                {
                    RemovePlayerFromGame(playerIdToRemove);
                }

                Logger.LogEvent(LoggerSection.Connection, $"Removed every player, because the host player {playerId} has disconnected");

                // Get ready to accept fresh new players
                if (scenesManager.GetActiveScene() != Scene.Lobby)
                {
                    scenesManager.SwitchScene(Scene.Lobby);
                }
            }
            else
            {
                RemovePlayerFromGame(playerId);
            }
        }

        private void RemovePlayerFromGame(int playerId)
        {
            PlayerColors.ReleasePlayerColor(playerId);
            Destroy(clients[playerId].serverPlayer.gameObject);
            clients.Remove(playerId);
            basePlayersManager.players.Remove(playerId);
        }
    }
}
