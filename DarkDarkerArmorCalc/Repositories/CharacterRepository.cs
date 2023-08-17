using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DarkDarkerArmorCalc.Repositories;

internal class CharacterRepository : IRepository<Character>
{
    private readonly IEnumerable<Character> characterData;

    public CharacterRepository()
    {
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
        if (string.IsNullOrEmpty(assemblyDirectory))
            throw new ApplicationException("unable to detemine assembly directory");

        var classJson = File.ReadAllText(Path.Join(assemblyDirectory, "characters.json"));
        characterData = JsonConvert.DeserializeObject<IEnumerable<Character>>(classJson)
            ?? throw new ApplicationException("unable to find characters.json source");
    }


    public IEnumerable<Character> Get() => characterData;
}
