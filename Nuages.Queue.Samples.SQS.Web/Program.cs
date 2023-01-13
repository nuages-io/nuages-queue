using Amazon.SQS;
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

    webApplicationBuilder.Services.AddSQSQueue()
    .AddSampleWorker(webApplicationBuilder.Configuration, "SampleWorker",
            options =>
            {
                //set options here  
            },
            options =>
            {
                //set options here  
                options.QueueName = "Queue1";
            })
    .AddSampleWorker(webApplicationBuilder.Configuration, "SampleWorker2",
        options =>
        {
            //set options here  
        },
        options =>
        {
            //set options here  
            options.QueueName = "Queue2";
        });
       
}

