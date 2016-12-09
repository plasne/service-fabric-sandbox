using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using Interfaces;

namespace CarActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class CarActor : Actor, ICarActor
    {
        /// <summary>
        /// Initializes a new instance of CarActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public CarActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        public Task<string> GetBrandAsync()
        {
            return this.StateManager.GetStateAsync<string>("brand");
        }
        public Task SetBrandAsync(string value)
        {
            return this.StateManager.AddOrUpdateStateAsync("brand", value, (k, v) => value);
        }

        public Task<string> GetModelAsync()
        {
            return this.StateManager.GetStateAsync<string>("model");
        }

        public Task SetModelAsync(string value)
        {
            return this.StateManager.AddOrUpdateStateAsync("model", value, (k, v) => value);
        }

        public Task<string> GetColorAsync()
        {
            return this.StateManager.GetStateAsync<string>("color");
        }

        public Task SetColorAsync(string value)
        {
            return this.StateManager.AddOrUpdateStateAsync("color", value, (k, v) => value);
        }

        public Task<int> GetYearAsync()
        {
            return this.StateManager.GetStateAsync<int>("year");
        }

        public Task SetYearAsync(int value)
        {
            return this.StateManager.AddOrUpdateStateAsync("year", value, (k, v) => value);
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            //return this.StateManager.TryAddStateAsync("count", 0);
            return Task.FromResult<bool>(true);

        }

    }
}
