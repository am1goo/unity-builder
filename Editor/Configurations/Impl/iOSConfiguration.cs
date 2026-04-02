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
    }
}
