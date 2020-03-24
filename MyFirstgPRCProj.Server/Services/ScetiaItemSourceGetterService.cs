using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Scetia.Source.Item;
using System.Linq;
using static Scetia.Source.Item.KindsReply.Types;
using Google.Protobuf.WellKnownTypes;

namespace MyFirstgRPCProj.Server
{
    public class ScetiaItemSourceGetterService : ScetiaItemSourceGetter.ScetiaItemSourceGetterBase
    {
        private readonly ILogger<ScetiaItemSourceGetterService> _logger;
        public ScetiaItemSourceGetterService(ILogger<ScetiaItemSourceGetterService> logger)
        {
            _logger = logger;
        }

        public static readonly IList<Kind> kinds = new List<Kind>
        {
            new Kind
            {
                KindId = "11",
                KindName = "结构材料",
                CanConsign = true
            },
            new Kind
            {
                KindId = "12",
                KindName = "市政道路",
                CanConsign = false
            },
            new Kind
            {
                KindId = "13",
                KindName = "节能材料",
                CanConsign = true
            },
        };


        public override Task<KindsReply> GetKinds(Empty request, ServerCallContext context)
        {
            var result = new KindsReply
            {
                Kinds =
                {
                    kinds.Select(x=>x)
                }
            };

            return Task.FromResult(result);
        }
    }
}