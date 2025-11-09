using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BuildSystem
{
    public class Builder
    {
        private IBuilderConfiguration _configuration;

        public Builder(IBuilderConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BuildReport Run()
        {
            if (_configuration.isConfigured == false)
                throw new Exception($"Run: current configuration is not configured properly");

            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            var activeBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var switched = EditorUserBuildSettings.SwitchActiveBuildTarget(_configuration.targetGroup, _configuration.target);
            if (!switched)
                throw new Exception($"Run: not switched from {activeBuildTarget} [{activeBuildTargetGroup}] to {_configuration.target} [{_configuration.targetGroup}]");

            var taskReports = new List<TaskReport>();

            foreach (var task in _configuration.preBuildTasks)
            {
                var res = task.Run(_configuration, default);
                taskReports.Add(new TaskReport(preBuild: true, task, res));
            }

            var options = _configuration.buildOptions;
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
                target = _configuration.target,
                targetGroup = _configuration.targetGroup,
                options = options,
                locationPathName = GetArtifactPath(_configuration, absolute: false),
            };
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            foreach (var task in _configuration.postBuildTasks)
            {
                var res = task.Run(_configuration, report.summary);
                taskReports.Add(new TaskReport(preBuild: false, task, res));
            }

            foreach (var taskReport in taskReports)
            {
                taskReport.Print();
            }

            return report;
        }

        public static string GetProjectPath(bool absolute)
        {
            if (absolute)
            {
                return Application.dataPath.Replace("/Assets", "");
            }
            else
            {
                return "";
            }
        }

        public static string GetArtifactPath(IBuilderConfiguration configuration, bool absolute)
        {
            var relativePath = $"Artifacts/{configuration.artifactsPath}";
            return Path.Combine(GetProjectPath(absolute), relativePath);
        }

        public static bool IsOSX(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                    return true;

                default:
                    return false;
            }
        }

        private struct TaskReport
        {
            public bool preBuild;
            public IBuilderTask task;
            public IBuilderTask.Result result;

            public TaskReport(bool preBuild, IBuilderTask task, IBuilderTask.Result result)
            {
                this.preBuild = preBuild;
                this.task = task;
                this.result = result;
            }

            public void Print()
            {
                var str = $"Run: [{(preBuild ? "PreBuild" : "PostBuild")}] {task} with result {result}";
                switch (result)
                {
                    case IBuilderTask.Result.Failed:
                        Debug.LogError(str);
                        break;

                    case IBuilderTask.Result.Skipped:
                        Debug.LogWarning(str);
                        break;

                    case IBuilderTask.Result.Passed:
                    default:
                        Debug.Log(str);
                        break;
                }
            }
        }
    }
}
