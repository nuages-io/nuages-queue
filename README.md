# Nuages.Queue

Nuages.Queue introduce the QueryWorker abstract class which is responsible to get message from a queue. The class needs to inherited from in order to provide overload for queue manipulations.

Two pre-build package are available on nuget :

- [Nuages.Queue.SQS](https://www.nuget.org/packages/Nuages.Queue.SQS/) for Simple Queue Service (SQS) on AWS
- [Nuages.Queue.ASQ](https://www.nuget.org/packages/Nuages.Queue.ASQ/) for Azure Storage Queue on Azure
