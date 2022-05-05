using System;
using System.Collections.Generic;
using System.Text;
using MOE.Common.Models;

namespace MOE.Common.ModelGenerator
{
    public static class SignalGenerator
    {
        static int versionIdSeed = 1;
        public static List<Signal> GetGoodSignalsWithApproachesAndDetectors(int numberOfSignalsToGenerate)
        {
            List<Signal> signals = new List<Signal>();
           for (int i = 1; i <= numberOfSignalsToGenerate; i++)
            {
                signals.Add(
                    new Signal()
                    {
                        VersionID = versionIdSeed,
                        SignalID = versionIdSeed.ToString(),
                        PrimaryName = "Primary Test 1",
                        SecondaryName = "Secondary Test 1",
                        ControllerTypeID = 1,
                        Enabled = true,
                        Pedsare1to1 = true,
                        IPAddress = "127.0.0.1",
                        Latitude = "40.75854665",
                        Longitude = "-111.8824063",
                        RegionID = 1,
                        VersionActionId = 10,
                        Approaches = ApproachGenerator.GetApproaches(2, numberOfSignalsToGenerate, versionIdSeed.ToString(), 2, null)
                    });
            }
            versionIdSeed++;
                
            return signals;
        }
    }
}
