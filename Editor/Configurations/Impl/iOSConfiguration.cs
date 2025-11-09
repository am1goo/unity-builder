using System;
using UnityEditor;

namespace BuildSystem
{
    public class iOSConfiguration : BuilderConfiguration
    {
        public iOSConfiguration(Options options) : base(options)
        {
        }

        protected override void AssertBuildTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.iOS:
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
                case BuildTarget.iOS:
                    return $"{productName}.ipa";

                default:
                    return base.GetBuildTargetExecutable(buildTarget, productName);
            }
        }
    }
}
