using Amazon.SQS;
using Nuages.Queue;
using Nuages.Queue.Samples.SQS.Web;
using Nuages.Queue.SQS;
// ReSharper disable UnusedParameter.Local

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

ConfigureTaskQueue(builder);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();

void ConfigureTaskQueue(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Configuration.AddJsonFile("appsettings.local.json", true);
    webApplicationBuilder.Services.AddDefaultAWSOptions(webApplicationBuilder.Configuration.GetAWSOptions())
        .AddAWSService<IAmazonSQS>();
    webApplicationBuilder.Services.AddSampleWorker(webApplicationBuilder.Configuration)
        .Configure<QueueOptions>(options =>
        {
            //set options here  
        }).Configure<QueueWorkerOptions>(options =>
        {
          //set options here  
        });
}

