// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global


using System;
using System.Net.Sockets;
using AmongUsClone.Shared.Meta;

namespace AmongUsClone.Shared.Networking
{
    /**
     * Class that encapsulates a connection via TCP
     */
    public class TcpConnection
    {
        protected const int DataBufferSize = 4096;

        public TcpClient tcpClient;

        protected NetworkStream stream;
        protected Packet receivePacket;
        protected byte[] receiveBuffer;

        public delegate void OnPacketReceivedCallback(int packetTypeId, Packet packet);

        private readonly MetaMonoBehaviours metaMonoBehaviours;

        public TcpConnection(MetaMonoBehaviours metaMonoBehaviours)
        {
            this.metaMonoBehaviours = metaMonoBehaviours;
        }

        /**
         * Send data to a connected TcpClient
         */
        public void SendPacket(Packet packet)
        {
            if (tcpClient == null)
            {
                throw new Exception("TcpConnection has no tcpClient");
            }

            try
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.GetLength(), null, null);
            }
            catch (Exception exception)
            {
                throw new Exception($"Unable to send tcp data: ${exception}");
            }
        }

        /**
         * TCP protocol is stream based - that means that we don't always have exactly 1 packet there - we just have
         * random bytes which can include half a packet, one packet, one and a half packet, or even more.
         * Because of it we need co compose our packets from those bytes by hand, and when we got ourselves a full packet - process it with HandlePacketCallback
         *
         * @returns - if full packet reset is needed
         */
        protected bool HandleData(byte[] data, OnPacketReceivedCallback onPacketReceivedCallback)
        {
            int packetLength = 0;

            receivePacket.WriteBytesAndPrepareToRead(data);

            // If start of the packet is (DataPacketTypeId) - read it and let try to create a packet to handle
            if (receivePacket.GetUnreadLength() >= sizeof(int))
            {
                packetLength = receivePacket.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            // If we have a packet to handle - handle it on main thread
            while (packetLength > 0 && packetLength <= receivePacket.GetUnreadLength())
            {
                byte[] packetBytes = receivePacket.ReadBytes(packetLength);

                metaMonoBehaviours.applicationCallbacks.ScheduleFixedUpdateAction(() =>
                {
                    Packet packet = new Packet(packetBytes);
                    int packetTypeId = packet.ReadInt();
                    onPacketReceivedCallback(packetTypeId, packet);
                });

                packetLength = 0;

                // If we still have a packet
                if (receivePacket.GetUnreadLength() >= sizeof(int))
                {
                    packetLength = receivePacket.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 1)
            {
                return true;
            }

            // We got ourselves only a part of a full packet - so we need to wait for it's next parts
            return false;
        }
    }
}
