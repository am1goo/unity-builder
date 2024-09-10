using System.Collections.Generic;
using UnityEditor;

namespace BuildSystem
{
    public interface IBuilderConfiguration
    {
        BuildTarget target { get; }
        BuildTargetGroup targetGroup { get; }
        string artifactsPath { get; }

        IEnumerable<IBuilderTask> preBuildTasks { get; }
        IEnumerable<IBuilderTask> postBuildTasks { get; }
    }
}