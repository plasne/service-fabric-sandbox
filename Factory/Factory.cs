using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace Factory
{

    internal sealed class Factory : StatelessService, IFactory
    {
        public Factory(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {

            // setup a remoting listener
            return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };

        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

            // sample code, just iterates a number
            long iterations = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }

        }

        public Task<string> paint()
        {

            // return a paint color
            return Task.FromResult<string>("blue");

        }

    }
}
