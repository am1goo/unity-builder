using System.Collections.Generic;
using UnityEditor;

namespace BuildSystem
{
    public interface IBuilderConfiguration
    {
        BuildTarget target { get; }
        BuildTargetGroup targetGroup { get; }
        BuildOptions buildOptions { get; }
        string productName { get; }
        string artifactsPath { get; }

        IEnumerable<IBuilderTask> preBuildTasks { get; }
        IEnumerable<IBuilderTask> postBuildTasks { get; }
    }
}