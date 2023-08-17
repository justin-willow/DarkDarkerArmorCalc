using Spectre.Console.Cli;
using DarkDarkerArmorCalc.Command;
using DarkDarkerArmorCalc.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Injection.Infrastructure;
using DarkDarkerArmorCalc.Commands;

var registrations = new ServiceCollection();
registrations.AddSingleton<ArmorCalculatorCommand>();
registrations.AddSingleton<ArmorRepository>();
registrations.AddSingleton<RaceRepository>();
registrations.AddSingleton<CharacterRepository>();

// Create a type registrar and register any dependencies.
// A type registrar is an adapter for a DI framework.
var registrar = new TypeRegistrar(registrations);
var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.AddCommand<ArmorCalculatorCommand>("armorcalc");
    config.AddCommand<ArmorGraphCommand>("armorgraph");
});

app.Run(args);