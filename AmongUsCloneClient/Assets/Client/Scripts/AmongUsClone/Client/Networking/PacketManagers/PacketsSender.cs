﻿using AmongUsClone.Client.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    public class PacketsSender : MonoBehaviour
    {
        public static void SendWelcomeReceivedPacket()
        {
            const ClientPacketType clientPacketType = ClientPacketType.WelcomeReceived;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                packet.Write(GameManager.instance.connectionToServer.myPlayerId);
                packet.Write(GameManager.instance.mainMenu.userNameField.text);

                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            }
        }

        public static void SendPlayerInputPacket(PlayerInput playerInput)
        {
            const ClientPacketType clientPacketType = ClientPacketType.PlayerInput;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                packet.Write(playerInput);
                GameManager.instance.connectionToServer.SendUdpPacket(clientPacketType, packet);
            }
        }

        public static void SendColorChangeRequestPacket()
        {
            const ClientPacketType clientPacketType = ClientPacketType.ColorChangeRequest;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            }
        }
    }
}