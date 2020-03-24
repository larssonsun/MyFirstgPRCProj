using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using MyFirstgRPCProj.Server;
using Scetia.Source.Item;

namespace MyFirstgRPCProj.Client.Con
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // The port number(5000) must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            // var client = new Greeter.GreeterClient(channel);
            // var reply = await client.SayHelloAsync(new HelloRequest { Name = "Larsson" });
            // await channel.ShutdownAsync();
            // Console.WriteLine($"服务Greeter返回的数据：{reply.Message}");

            // var client = new FukinBullShiter.FukinBullShiterClient(channel);
            // var reply = await client.FukUAsync(new FukWhoRequest { Name = "Larsson", Age = 39 });
            // await channel.ShutdownAsync();
            // Console.WriteLine($"{reply.Mesage}");

            var client = new ScetiaItemSourceGetter.ScetiaItemSourceGetterClient(channel);
            var reply = await client.GetKindsAsync(new Empty());
            await channel.ShutdownAsync();
            reply.Kinds.Any(x =>
            {
                Console.WriteLine($"{x.KindId}-{x.KindName}{(x.CanConsign ? string.Empty : "(不可收样)")}");
                return false;
            });
        }
    }
}
