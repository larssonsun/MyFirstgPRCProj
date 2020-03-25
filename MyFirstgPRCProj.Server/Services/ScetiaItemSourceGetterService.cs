using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Scetia.Source.Item;
using System.Linq;
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

        public static readonly IList<KindEntity> kinds = new List<KindEntity>
        {
            new KindEntity
            {
                KId = "11",
                KName = "结构材料",
                Cc = true
            },
            new KindEntity
            {
                KId = "12",
                KName = "市政道路",
                Cc = false
            },
            new KindEntity
            {
                KId = "13",
                KName = "节能材料",
                Cc = true
            },
        };


        public override Task<KindsReply> GetKinds(Empty request, ServerCallContext context)
        {
            var result = new KindsReply
            {
                Kinds =
                {
                    kinds.Select(x => new Kind{KindId = x.KId, KindName = x.KName, CanConsign = x.Cc})
                }
            };

            return Task.FromResult(result);
        }

        public override Task<BoolReply> SetKinds(KindsRequest request, Grpc.Core.ServerCallContext context)
        {
            request.Kinds.Any(x =>
            {
                kinds.Add(new KindEntity { KId = x.KindId, KName = x.KindName, Cc = x.CanConsign });
                return false;
            });

            return Task.FromResult(new BoolReply { Result = true, TotalCount = kinds.Count });
        }
    }

    public class KindEntity
    {
        public string KId { get; set; }
        public string KName { get; set; }
        public bool Cc { get; set; }
    }
}