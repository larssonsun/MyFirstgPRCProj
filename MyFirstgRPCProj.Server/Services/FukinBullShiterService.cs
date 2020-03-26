using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MyFirstgRPCProj.Server
{
    public class FukinBullShiterService : FukinBullShiter.FukinBullShiterBase
    {
        private readonly ILogger<FukinBullShiterService> _logger;
        public FukinBullShiterService(ILogger<FukinBullShiterService> logger)
        {
            _logger = logger;
        }

        public override Task<FukedReply> FukU(FukWhoRequest request, ServerCallContext context)
        {
            return Task.FromResult(new FukedReply
            {
                Mesage = $"Fuk U {request.Name}({request.Age}才)"
            });
        }
    }
}