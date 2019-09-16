using ProtoBuf;
using System;

namespace IUV.SDN
{
    [Serializable, ProtoContract(Name = @"CSHeartBeat")]
    public partial class CSHeartBeat : CSPacketBase
    {
        public CSHeartBeat()
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
