# Nuages.Queue

Nuages.Queue introduce the QueryWorker abstract class which is responsible to get message from a queue. The class needs to inherited from in order to provide overload for queue manipulations.

Two pre-build package are available on nuget :

- [Nuages.Queue.SQS](https://www.nuget.org/packages/Nuages.Queue.SQS/) for Simple Queue Service (SQS) on AWS
- [Nuages.Queue.ASQ](https://www.nuget.org/packages/Nuages.Queue.ASQ/) for Azure Storage Queue on Azure


##Using Nuages.Queue.SQS

Follow those steps to use Nuages.Queue.SQS in your .NET 6 console project. 

1. Create the project

```cmd
dotnet new console -n Nuages.Queue.Samples.SQS
cd Nuages.Queue.Samples.SQS
```

2. Add a package reference to Nuages.Queue.SQS

```cmd
dotnet add package Nuages.Queue.SQS 
```

3. Add other references to the .csproj

```xml
<ItemGroup>
  <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.7.1" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
  <PackageReference Include="Microsoft.Extensions.Logging.Console" Version="6.0.0" />
</ItemGroup>
```