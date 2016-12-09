using Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static System.Fabric.FabricClient;

namespace Api.Code
{
    public class Inventory : ApiController
    {

        [DataContract]
        public class car
        {

            [DataMember]
            public string brand { get; set; }

            [DataMember]
            public string model { get; set; }

            [DataMember]
            public int year { get; set; }

            [DataMember]
            public string color { get; set; }
                 
        }

        [HttpGet, Route("inventory")]
        public async Task<List<car>> GetInventory()
        {
            List<car> list = new List<car>();

            IActorService actorServiceProxy = ActorServiceProxy.Create(new Uri("fabric:/SfWebAppDemo/CarActorService"), 1);

            ContinuationToken continuationToken = null;
            CancellationToken cancellationToken;
            List<ActorId> actorIds = new List<ActorId>();

            do
            {
                PagedResult<ActorInformation> page = await actorServiceProxy.GetActorsAsync(continuationToken, cancellationToken);
                foreach(ActorInformation info in page.Items)
                {
                    actorIds.Add(info.ActorId);
                }
                continuationToken = page.ContinuationToken;
            }
            while (continuationToken != null);

            actorIds.ForEach((id) => {
                ICarActor carproxy = ActorProxy.Create<ICarActor>(id, new Uri("fabric:/SfWebAppDemo/CarActorService"));
                Task<string> t1 = carproxy.GetBrandAsync();
                Task<string> t2 = carproxy.GetModelAsync();
                Task<int> t3 = carproxy.GetYearAsync();
                Task<string> t4 = carproxy.GetColorAsync();
                Task.WaitAll(t1, t2, t3, t4);
                if (t1.IsCompleted && t2.IsCompleted && t3.IsCompleted && t4.IsCompleted)
                {
                    car car = new car() { brand = t1.Result, model = t2.Result, year = t3.Result, color = t4.Result };
                    list.Add(car);
                }
                else
                {
                    // error
                }
            });

            return list;
        }

        [HttpGet, Route("paint")]
        public Task<string> paint()
        {
            //return "paint

            //ServicePartitionResolver resolver = ServicePartitionResolver.GetDefault();
            //CancellationToken cancellationToken;
            //ResolvedServicePartition partition = await resolver.ResolveAsync(new Uri("fabric:/SfWebAppDemo/Factory"), new ServicePartitionKey(), cancellationToken);
            //var client = new ServicePartitionClient(this)

            IFactory factory = ServiceProxy.Create<IFactory>(new Uri("fabric:/SfWebAppDemo/Factory"));
            return factory.paint();
        }

        [HttpGet, Route("create")]
        public bool Create()
        {

            ActorId id = ActorId.CreateRandom();
            ICarActor car = ActorProxy.Create<ICarActor>(id, new Uri("fabric:/SfWebAppDemo/CarActorService"));
            Task t1 = car.SetBrandAsync("GM");
            Task t2 = car.SetModelAsync("Volt");
            Task t3 = car.SetYearAsync(2012);
            Task t4 = car.SetColorAsync("black");

            Task.WaitAll(t1, t2, t3, t4);
            bool success = t1.IsCompleted && t2.IsCompleted && t3.IsCompleted && t4.IsCompleted;

            return success;
        }

    }
}
