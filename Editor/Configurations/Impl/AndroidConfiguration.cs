using System;
using UnityEditor;

namespace BuildSystem
{
    public class AndroidConfiguration : BuilderConfiguration
    {
        private readonly AndroidVariant _variant;
        private readonly AndroidAppBundle _appBundle;

        public AndroidConfiguration(AndroidVariant variant, AndroidAppBundle appBundle, Options options) : base(options)
        {
            _variant = variant;
            _appBundle = appBundle;
        }

        protected override void AssertBuildTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    //do nothing
                    break;

                default:
                    throw new Exception($"unsupported target {target}");
            }
        }

        protected override string GetBuildTargetFolder(BuildTarget buildTarget)
        {
            return _variant.ToString().ToLowerInvariant();
        }

        protected override string GetBuildTargetExecutable(BuildTarget buildTarget, string productName)
        {
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    var extension = _appBundle.IsAab() ? "aab" : "apk";
                    return $"{productName}.{extension}";

                default:
                    return base.GetBuildTargetExecutable(buildTarget, productName);
            }
        }
    }
}
