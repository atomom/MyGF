using GameFramework.Network;
using UnityGameFramework.Runtime;

namespace IUV.SDN
{
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 1005;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCHeartBeat packetImpl = (SCHeartBeat)packet;
            Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());
        }
    }
}
