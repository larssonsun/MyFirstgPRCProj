using System;
using System.Linq;
using System.Threading.Tasks;
using Consul;
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

            #region  non-api-gateway

            // // The port number(5000) must match the port of the gRPC server.
            // var channel = GrpcChannel.ForAddress("http://localhost:5000");

            // var client = new Greeter.GreeterClient(channel);
            // var reply = await client.SayHelloAsync(new HelloRequest { Name = "Larsson" });
            // await channel.ShutdownAsync();
            // Console.WriteLine($"服务Greeter返回的数据：{reply.Message}");

            // var client2 = new FukinBullShiter.FukinBullShiterClient(channel);
            // var reply2 = await client2.FukUAsync(new FukWhoRequest { Name = "Larsson", Age = 39 });
            // await channel.ShutdownAsync();
            // Console.WriteLine($"{reply2.Mesage}");

            // var client3 = new ScetiaItemSourceGetter.ScetiaItemSourceGetterClient(channel);
            // var reply3 = await client3.GetKindsAsync(new Empty());
            // await channel.ShutdownAsync();
            // reply3.Kinds.Any(x =>
            // {
            //     Console.WriteLine($"{x.KindId}-{x.KindName}{(x.CanConsign ? string.Empty : "(不可收样)")}");
            //     return false;
            // });

            #endregion

            #region api-gateway inuse

            var serviceAddress = string.Empty;

            using (var consulClient = new ConsulClient(option => option.Address = new Uri("http://localhost:8500")))
            {
                var serviceName = "MyFirstgRPCMicroServ";
                var services = await consulClient.Catalog.Service(serviceName);
                if (services.Response.Length == 0)
                {
                    Console.WriteLine($"can not find the service \"{serviceName}\"");
                    return;
                }

                var service = services.Response[0];
                serviceAddress = $"http://{service.ServiceAddress}:{service.ServicePort}";
                Console.WriteLine($"service \"{serviceName}\" load succeed.");
            }

            BoolReply reply5 = null;
            using (var channelFromConsul = GrpcChannel.ForAddress(serviceAddress))
            {
                var serviceClient = new ScetiaItemSourceGetter.ScetiaItemSourceGetterClient(channelFromConsul);
                var request = new KindsRequest();
                request.Kinds.Add(new Kind { KindId = "21", KindName = "垃圾", CanConsign = false });
                reply5 = await serviceClient.SetKindsAsync(request);
                await channelFromConsul.ShutdownAsync();
            }
            Console.WriteLine($"result：{reply5.Result}. kindTotalCount：{reply5.TotalCount}");

            #endregion
        }
    }
}
