using System.Text;

namespace Day05;

public readonly record struct Almanac
(
    Map SeedToSoil,
    Map SoilToFertilizer,
    Map FertilizerToWater,
    Map WaterToLight,
    Map LightToTemperature,
    Map TemperatureToHumidity,
    Map HumidityToLocation
)
{
    public static Almanac Parse(IEnumerable<string> blocks)
    {
        var maps = blocks
            .Select((block) => Map.Parse(block.Split(Environment.NewLine)))
            .ToArray();

        return new Almanac
        (
            maps[0],
            maps[1],
            maps[2],
            maps[3],
            maps[4],
            maps[5],
            maps[6]
        );
    }

    public uint GetLocation(uint seed)
    {
        var soil = SeedToSoil.Convert(seed);
        var fertilizer = SoilToFertilizer.Convert(soil);
        var water = FertilizerToWater.Convert(fertilizer);
        var light = WaterToLight.Convert(water);
        var temperature = LightToTemperature.Convert(light);
        var humidity = TemperatureToHumidity.Convert(temperature);
        return HumidityToLocation.Convert(humidity);
    }

    public override string ToString() => new StringBuilder()
            .AppendLine("seed-to-soil map:")
            .AppendLine(SeedToSoil.ToString())
            .AppendLine()
            .AppendLine("soil-to-fertilizer map:")
            .AppendLine(SoilToFertilizer.ToString())
            .AppendLine()
            .AppendLine("fertilizer-to-water map:")
            .AppendLine(FertilizerToWater.ToString())
            .AppendLine()
            .AppendLine("water-to-light map:")
            .AppendLine(WaterToLight.ToString())
            .AppendLine()
            .AppendLine("light-to-temperature map:")
            .AppendLine(LightToTemperature.ToString())
            .AppendLine()
            .AppendLine("temperature-to-humidity map:")
            .AppendLine(TemperatureToHumidity.ToString())
            .AppendLine()
            .AppendLine("humidity-to-location map:")
            .Append(HumidityToLocation.ToString())
            .ToString();
}