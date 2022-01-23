using AsyncMessaging.PubSub.Implementations.RabbitMq;
using CommandLine;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
                     .ConfigureServices((_, services) => { })
                     .Build();


var subscriber = new RabbitMqPubSub("host=localhost;username=test;password=test123!");
await subscriber.Subscribe(new Handler());
// while(!task.IsCompleted)
// {}
Console.WriteLine("Waiting for input.");
Console.Read();

await host.RunAsync();

Console.WriteLine("Press <Enter> to exit.");
Console.Read();
//
// using var bus = RabbitHutch.CreateBus("host=localhost;username=test;password=test123!");
//
// var message = await bus.PubSub.SubscribeAsync<StringMessage>("test", message => { new StringMessageHandler(1).Handle(message);});
//


// var subscriber = new RabbitMqPubSub("host=localhost;username=test;password=test123!");
// await subscriber.Subscribe(new Handler());
// Console.WriteLine("Waiting for input.");
// Console.Read();

// var result = await Parser.Default
//                    .ParseArguments<CommandLineOptions>(args)
//                    .MapResult(async options =>
//                               {
//                                   try
//                                   {
//                                       return options.Index switch
//                                       {
//                                           1 => await Subscribe(options.Subscribers),
//                                           2 => await Publish(options.Publishers),
//                                           _ => 0
//                                       };
//                                   }
//                                   catch (Exception ex)
//                                   {
//                                       Console.WriteLine(ex);
//                                       return -3;
//                                   }
//                               }, errors => Task.FromResult(-1));
//
//
// Console.WriteLine("Press <Enter> to exit.");
// Console.Read();
// return result;

static async Task<int> Subscribe(int amount)
{
    for (var i = 0; i < amount; i++)
    {
        var subscriber = new RabbitMqPubSub("host=localhost;username=test;password=test123!");
        await subscriber.Subscribe(new Handler());
    }
// var subscriber2 = new RabbitMqPubSub("host=localhost;username=test;password=test123!");
// await subscriber2.Subscribe(new StringMessageHandler2());

    return 1;
}

static async Task<int> Publish(int amount)
{
    for (var i = 0; i < amount; i++)
    {
        var publisher = new RabbitMqPubSub("host=localhost;username=test;password=test123!");//;publisherConfirms=true;timeout=10

        await publisher.Publish(new StringMessage{Text = $"{i}: Hello!" });
    }

    return 1;
}

public class StringMessageHandler1: IMessageHandler<StringMessage>
{
    public Task Handle(StringMessage message)
    {
        Console.WriteLine($"{typeof(StringMessageHandler1).FullName}: {message.Text}");
        return Task.CompletedTask;
    }
}

public class StringMessageHandler2: IMessageHandler<StringMessage>
{
    public Task Handle(StringMessage message)
    {
        Console.WriteLine($"{typeof(StringMessageHandler2).FullName}: {message.Text}");
        return Task.CompletedTask;
    }
}

public class StringMessage
{
    public string Text { get; set; }
}

public class CommandLineOptions
{
    [Value(0, Required = true, HelpText = "The function to execute.\n1: Subscribe\n2: Publish")]
    public int Index { get; set; }

    [Option(shortName: 'p', longName: "publishers", Required = false, HelpText = "The number of items to publish.", Default = 10)]
    public int Publishers { get; set; }

    [Option(shortName: 's', longName: "subscribers", Required = false, HelpText = "The number of subscribers.", Default = 1)]
    public int Subscribers { get; set; }
}

public class StringMessageHandler : IMessageHandler<StringMessage>
{
    private readonly int _i;

    public StringMessageHandler(int i)
    {
        _i = i;
    }
    public Task Handle(StringMessage message)
    {
        Console.WriteLine($"{typeof(StringMessageHandler).FullName}_{_i}: {message.Text}");
        return Task.CompletedTask;
    }
}

internal class Handler : IMessageHandler<string>
{
    public Task Handle(string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}