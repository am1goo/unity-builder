using System;
using System.Collections.Generic;
using UnityEditor;

namespace BuildSystem
{
    public class BuilderConfiguration : IBuilderConfiguration
    {
        private Options _options;
        protected Options options => _options;

        private bool _isConfigured;
        public bool isConfigured => _isConfigured;

        private BuildTarget _target;
        public BuildTarget target => _target;

        private BuildTargetGroup _targetGroup;
        public BuildTargetGroup targetGroup => _targetGroup;

        private BuildOptions _buildOptions;
        public BuildOptions buildOptions => _buildOptions;

        private string _productName;
        public string productName => _productName;

        private string _artifactsPath;
        public string artifactsPath => _artifactsPath;

        private List<IBuilderTask> _preBuildTasks;
        public IEnumerable<IBuilderTask> preBuildTasks => _preBuildTasks;

        private List<IBuilderTask> _postBuildTasks;
        public IEnumerable<IBuilderTask> postBuildTasks => _postBuildTasks;

        public delegate void OnGetBuildTasksDelegate(List<IBuilderTask> result);

        public BuilderConfiguration(Options options)
        {
            AssertBuildTarget(options.target);

            _options = options;
            _target = options.target;
            _targetGroup = BuildPipeline.GetBuildTargetGroup(options.target);
            _productName = options.productName;
            _buildOptions = options.buildOptions;
            _artifactsPath = string.Empty;
            _preBuildTasks = new List<IBuilderTask>();
            _postBuildTasks = new List<IBuilderTask>();
        }

        public void Configure(OnGetBuildTasksDelegate onPreBuildTasks, OnGetBuildTasksDelegate onPostBuildTasks)
        {
            if (_isConfigured)
                return;

            var pathSegments = new List<string>();
            if (!string.IsNullOrWhiteSpace(options.prefix))
                pathSegments.Add(options.prefix);
            if (!string.IsNullOrWhiteSpace(options.subplatform))
                pathSegments.Add(options.subplatform);
            else
                pathSegments.Add("generic");
            var buildTargetFolder = GetBuildTargetFolder(options.target);
            pathSegments.Add(buildTargetFolder);
            var buildTargetExecutable = GetBuildTargetExecutable(_target, options.productName);
            pathSegments.Add(buildTargetExecutable);
            _artifactsPath = string.Join("/", pathSegments);

            _preBuildTasks.Clear();
            if (onPreBuildTasks != null)
                onPreBuildTasks(_preBuildTasks);

            _postBuildTasks.Clear();
            if (onPostBuildTasks != null)
                onPostBuildTasks(_postBuildTasks);

            _isConfigured = true;
        }

        protected virtual void AssertBuildTarget(BuildTarget target)
        {
            //do nothing
        }

        protected virtual string GetBuildTargetFolder(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    return "win_x86_64";
                case BuildTarget.StandaloneWindows64:
                    return "win_x64";
                case BuildTarget.StandaloneLinux64:
                    return "linux_x86_64";
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel64:
                    return "macos_x64";
                case BuildTarget.StandaloneOSXIntel:
                    return "macos_x86";
                default:
                    return buildTarget.ToString();
            }
        }

        protected virtual string GetBuildTargetExecutable(BuildTarget buildTarget, string productName)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return $"{productName}.exe";

                case BuildTarget.StandaloneLinux:
                    return $"{productName}.x86";
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                    return $"{productName}.x86_64";

                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                    return $"{productName}.app";

                default:
                    return $"{productName}";
            }
        }

        public struct Options
        {
            public BuildTarget target;
            public BuildOptions buildOptions;
            public string prefix;
            public string subplatform;
            public string productName;
        }
    }
}
