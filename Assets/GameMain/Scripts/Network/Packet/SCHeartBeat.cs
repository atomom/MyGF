using ProtoBuf;
using System;

namespace IUV.SDN
{
    [Serializable, ProtoContract(Name = @"SCHeartBeat")]
    public partial class SCHeartBeat : SCPacketBase
    {
        public SCHeartBeat()
        {

        }

        public override int Id
        {
            get
            {
                return 1005;
            }
        }

        public override void Clear()
        {

        }
    }
}
