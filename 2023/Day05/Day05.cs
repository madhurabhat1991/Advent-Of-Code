using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template;

namespace _2023.Day05
{
    public class Day05 : Day<Farm, long, long>
    {
        public override string DayNumber { get { return "05"; } }

        public override long PartOne(Farm input)
        {
            return input.FindLowestLocation();
        }

        public override long PartTwo(Farm input)
        {
            return input.FindLowestLocation2();
        }

        public override Farm ProcessInput(string[] input)
        {
            Farm farm = new Farm();
            var blocks = input.Blocks();
            // Assume these are seeds for part 1
            farm.Seeds = blocks[0][0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList();
            // Seed ranges for part 2 - List<(start, end)> - all inclusive
            farm.SeedRanges = new List<(long, long)>();
            for (int i = 0; i < farm.Seeds.Count; i++)
            {
                if (i % 2 == 0)
                {
                    farm.SeedRanges.Add((farm.Seeds[i], farm.Seeds[i] + farm.Seeds[i + 1] - 1));
                    i++;
                }
            }
            // All mappers - List<(src start, src end, dest start, dest end)> - all inclusive
            for (int b = 1; b < blocks.Count; b++)
            {
                var block = blocks[b];
                var mapper = InitializeMapper(block);
                var map = block[0];
                switch (map.ToLower())
                {
                    case string s when s.StartsWith("seed"):
                        farm.SeedToSoil = mapper;
                        break;
                    case string s when s.StartsWith("soil"):
                        farm.SoilToFertilizer = mapper;
                        break;
                    case string s when s.StartsWith("fertilizer"):
                        farm.FertilizerToWater = mapper;
                        break;
                    case string s when s.StartsWith("water"):
                        farm.WaterToLight = mapper;
                        break;
                    case string s when s.StartsWith("light"):
                        farm.LightToTemp = mapper;
                        break;
                    case string s when s.StartsWith("temperature"):
                        farm.TempToHumidity = mapper;
                        break;
                    case string s when s.StartsWith("humidity"):
                        farm.HumidityToLoc = mapper;
                        break;
                    default:
                        break;
                }
            }
            return farm;
        }

        /// <summary>
        /// Find the ranges of src start, src end, dest start, dest end - all inclusive
        /// </summary>
        /// <param name="block"></param>
        /// <returns>List<(src start, src end, dest start, dest end)></returns>
        private List<(long, long, long, long)> InitializeMapper(List<String> block)
        {
            List<(long, long, long, long)> mapper = new List<(long, long, long, long)>();
            for (int i = 1; i < block.Count; i++)
            {
                var nums = block[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).StringArrayToLongList();
                mapper.Add((nums[1], nums[1] + nums[2] - 1, nums[0], nums[0] + nums[2] - 1));
            }
            return mapper;
        }
    }

    public class Farm
    {
        public List<long> Seeds { get; set; }
        public List<(long, long)> SeedRanges { get; set; }
        public List<(long, long, long, long)> SeedToSoil { get; set; }
        public List<(long, long, long, long)> SoilToFertilizer { get; set; }
        public List<(long, long, long, long)> FertilizerToWater { get; set; }
        public List<(long, long, long, long)> WaterToLight { get; set; }
        public List<(long, long, long, long)> LightToTemp { get; set; }
        public List<(long, long, long, long)> TempToHumidity { get; set; }
        public List<(long, long, long, long)> HumidityToLoc { get; set; }

        public long FindLowestLocation()
        {
            List<long> locations = new List<long>();
            foreach (var seed in Seeds)
            {
                locations.Add(FindLocation(seed));
            }
            return locations.Min();
        }

        public long FindLowestLocation2()
        {
            long loc = -1;
            long seed = 0;
            do
            {
                loc++;
                seed = FindSeed(loc);
            } while (!(SeedRanges.Where(r => seed >= r.Item1 && seed <= r.Item2).Any()));
            return loc;
        }

        private long FindLocation(long seed)
        {
            return
                MapForward(
                    MapForward(
                        MapForward(
                            MapForward(
                                MapForward(
                                    MapForward(
                                        MapForward(seed, SeedToSoil),
                                        SoilToFertilizer),
                                    FertilizerToWater),
                                WaterToLight),
                            LightToTemp),
                        TempToHumidity),
                    HumidityToLoc);
        }

        private long MapForward(long input, List<(long, long, long, long)> mapper)
        {
            var collection = mapper.Where(r => input >= r.Item1 && input <= r.Item2);
            return collection.Any() ? (collection.First().Item3 + (input - collection.First().Item1)) : input;
        }

        private long FindSeed(long loc)
        {
            return
                MapBackward(
                    MapBackward(
                        MapBackward(
                            MapBackward(
                                MapBackward(
                                    MapBackward(
                                        MapBackward(loc, HumidityToLoc),
                                        TempToHumidity),
                                    LightToTemp),
                                WaterToLight),
                            FertilizerToWater),
                        SoilToFertilizer),
                    SeedToSoil);
        }

        private long MapBackward(long input, List<(long, long, long, long)> mapper)
        {
            var collection = mapper.Where(r => input >= r.Item3 && input <= r.Item4);
            return collection.Any() ? (collection.First().Item1 + (input - collection.First().Item3)) : input;
        }
    }
}
