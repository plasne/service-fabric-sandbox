# Service Fabric Sandbox

This project is just an area for me to play with Service Fabric. Very loosely this managing inventory for car dealerships.

## Architecture

This solution is composed of the following projects:

* Web - static file hosting on port 80
* Api - hosting of an API on port 8000
* Interfaces - all interfaces that will be used by the proxies
* Factory - a stateless service that will be accessed by internal services only (using remoting)
* CarActor - an Actor representing cars in inventory
* SfWebAppDemo - the Service Fabric Application

## Actor Proxy

To create an Actor follow these steps (ex. [CarActor](CarActor)):

1. Create an Actor project using the template in Visual Studio.
2. Create an interface which will include any methods and properties you intend to expose.
3. Implement the interface on the Actor (ex. internal class CarActor : Actor, ICarActor).
4. To get a persisted property:
  * this.StateManager.GetStateAsync<string>("property-name")
5. To set a persisted property:
  * this.StateManager.AddOrUpdateStateAsync("property-name", value, (k, v) => value)
  * The function can include any validation logic.
6. To create or reference an existing Actor (example):
  * ActorId id = ActorId.CreateRandom();
  * ICarActor car = ActorProxy.Create<ICarActor>(id, new Uri("fabric:/SfWebAppDemo/CarActorService"));

See [Inventory.cs](Api/Code/Inventory.cs) for examples on enumerating Actors, reading properties, setting properties, etc. Please note also that these methods are all async so you must handle that appropriately.

## Service Proxy

To create a Service follow these steps (ex. [Factory](Factory)):

1. Create a Stateful or Stateless Service using the template in Visual Studio.
2. Add a remoting listener endpoint under CreateServiceInstanceListeners():
  * return new[] { new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context)) };
3. Create an interface which will include any methods and properties you intend to expose.
4. Implement the interface on the Service (ex. internal sealed class Factory : StatelessService, IFactory).
5. To create or reference the Service (example):
  * IFactory factory = ServiceProxy.Create<IFactory>(new Uri("fabric:/SfWebAppDemo/Factory"));
  
See [Inventory.cs](Api/Code/Inventory.cs) for an example on calling a property on another object using the remoting interface.

See this example for further details: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-reliable-services-communication-remoting. 

## Hosting Static Files

To host static files using Web API:

```C#
PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(@".\wwwroot");

FileServerOptions fileOptions = new FileServerOptions();
fileOptions.EnableDefaultFiles = true;
fileOptions.RequestPath = PathString.Empty;
fileOptions.FileSystem = physicalFileSystem;
fileOptions.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
fileOptions.StaticFileOptions.FileSystem = fileOptions.FileSystem = physicalFileSystem;
fileOptions.StaticFileOptions.ServeUnknownFileTypes = true;
fileOptions.EnableDirectoryBrowsing = true;

config.MapHttpAttributeRoutes();
appBuilder.UseWebApi(config);
appBuilder.UseFileServer(fileOptions);
```

See [Startup.cs](Web/Startup.cs) for an example. The required libaries can be found in NuGet.

## Interfaces

Whenever you use a proxy to get to an object, you will need the interface of the object you are trying to get. I recommend putting all those interfaces into a separate project so you can include them easily when needed.
