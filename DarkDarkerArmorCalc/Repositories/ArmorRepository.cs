using Newtonsoft.Json;
using System.Reflection;

namespace DarkDarkerArmorCalc.Repositories;

internal class ArmorRepository : IRepository<Armor>
{
    private readonly IEnumerable<Armor> armorData;

    public ArmorRepository()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(assemblyDirectory))
            throw new ApplicationException("unable to detemine assembly directory");

        var armorJson = File.ReadAllText(Path.Join(assemblyDirectory, "armors.json"));
        armorData = JsonConvert.DeserializeObject<IEnumerable<Armor>>(armorJson)
            ?? throw new ApplicationException("unable to find armor.json source");
    }


    public IEnumerable<Armor> Get() => armorData;
}
