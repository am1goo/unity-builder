using System;
using UnityEditor;

namespace BuildSystem
{
    public class ConsoleConfiguration : BuilderConfiguration
    {
        public ConsoleConfiguration(Options options) : base(options)
        {
        }

        protected override void AssertBuildTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Switch:
                case BuildTarget.GameCoreXboxOne:
                case BuildTarget.GameCoreXboxSeries:
                case BuildTarget.PS4:
                case BuildTarget.PS5:
                    //do nothing
                    break;

                default:
                    throw new Exception($"unsupported target {target}");
            }
        }

        protected override string GetBuildTargetExecutable(BuildTarget buildTarget, string productName)
        {
            switch (buildTarget)
            {
                case BuildTarget.Switch:
                    return $"{productName}.nsp";

                default:
                    return base.GetBuildTargetExecutable(buildTarget, productName);
            }
        }
    }
}
