// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Game.PlayerLogic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     */
    public class GameSnapshot
    {
        public readonly int id;
        public readonly List<SnapshotPlayerInfo> playersInfo;

        public GameSnapshot(int id, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(int id, IEnumerable<Player> players)
        {
            this.id = id;
            playersInfo = players.Select(player => new SnapshotPlayerInfo(player.id, player.Position)).ToList();
        }

        public override string ToString()
        {
            List<string> playersInfoDescriptionPieces = new List<string>(playersInfo.Count);
            foreach (SnapshotPlayerInfo playerInfo in playersInfo)
            {
                playersInfoDescriptionPieces.Add($"{{ id: {playerInfo.id}, position: {playerInfo.position} }}");
            }

            string playersInfoDescription = string.Join(",", playersInfoDescriptionPieces);

            return $"#{id}. Players: {{ {playersInfoDescription} }}";
        }
    }
}
