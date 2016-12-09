using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface ICarActor : IActor
    {

        Task<string> GetBrandAsync();
        Task SetBrandAsync(string value);

        Task<string> GetModelAsync();
        Task SetModelAsync(string value);

        Task<int> GetYearAsync();
        Task SetYearAsync(int value);

        Task<string> GetColorAsync();
        Task SetColorAsync(string value);


    }
}
