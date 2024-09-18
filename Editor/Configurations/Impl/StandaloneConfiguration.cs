using System;
using UnityEditor;

namespace BuildSystem
{
    public class StandaloneConfiguration : BuilderConfiguration
    {
        public StandaloneConfiguration(BuildTarget target, string artifactsPath, OnGetBuildTasksDelegate onPreBuildTasks = null, OnGetBuildTasksDelegate onPostBuildTasks = null) : base(target, artifactsPath, onPreBuildTasks, onPostBuildTasks)
        {
        }

        protected override void AssertBuildTarget(BuildTarget target)
        {
            base.AssertBuildTarget(target);

            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                    //do nothing
                    break;

                default:
                    throw new Exception($"unsupported target {target}");
            }
        }
    }
}
