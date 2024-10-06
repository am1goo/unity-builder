using System;
using System.Collections.Generic;
using UnityEditor;

namespace BuildSystem
{
    public class BuilderConfiguration : IBuilderConfiguration
    {
        private BuildTarget _target;
        public BuildTarget target => _target;

        private BuildTargetGroup _targetGroup;
        public BuildTargetGroup targetGroup => _targetGroup;

        private string _productName;
        public string productName => _productName;

        private string _artifactsPath;
        public string artifactsPath => _artifactsPath;

        private IEnumerable<IBuilderTask> _preBuildTasks;
        public IEnumerable<IBuilderTask> preBuildTasks => _preBuildTasks;

        private IEnumerable<IBuilderTask> _postBuildTasks;
        public IEnumerable<IBuilderTask> postBuildTasks => _postBuildTasks;

        public delegate void OnGetBuildTasksDelegate(List<IBuilderTask> result);

        public BuilderConfiguration(BuildTarget target, string productName, string artifactsPath, OnGetBuildTasksDelegate onPreBuildTasks, OnGetBuildTasksDelegate onPostBuildTasks)
        {
            AssertBuildTarget(target);

            _target = target;
            _targetGroup = BuildPipeline.GetBuildTargetGroup(target);
            _productName = productName;
            _artifactsPath = artifactsPath;

            var preBuildTasks = new List<IBuilderTask>();
            if (onPreBuildTasks != null)
                onPreBuildTasks(preBuildTasks);
            _preBuildTasks = preBuildTasks;

            var postBuildTasks = new List<IBuilderTask>();
            if (onPostBuildTasks != null)
                onPostBuildTasks(postBuildTasks);
            _postBuildTasks = postBuildTasks;
        }

        protected virtual void AssertBuildTarget(BuildTarget target)
        {
            //do nothing
        }
    }
}
