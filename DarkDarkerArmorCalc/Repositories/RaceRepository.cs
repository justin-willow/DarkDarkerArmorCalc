using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DarkDarkerArmorCalc.Repositories;

internal class RaceRepository : IRepository<Race>
{
    private readonly IEnumerable<Race> raceData;

    public RaceRepository()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(assemblyDirectory))
            throw new ApplicationException("unable to detemine assembly directory");

        var raceJson = File.ReadAllText(Path.Join(assemblyDirectory, "races.json"));
        raceData = JsonConvert.DeserializeObject<IEnumerable<Race>>(raceJson)
            ?? throw new ApplicationException("unable to find race.json source");
    }


    public IEnumerable<Race> Get() => raceData;
}
