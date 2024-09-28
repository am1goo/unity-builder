using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildSystem
{
    public class DeleteFileBuilderTask : IBuilderTask
    {
        private IEnumerable<string> _paths;

        public DeleteFileBuilderTask(IEnumerable<SpecialPath> paths) : this(GetSpecialPathPattern(paths))
        {
        }

        public DeleteFileBuilderTask(SpecialPath path) : this(GetSpecialPathPattern(path))
        {
        }

        public DeleteFileBuilderTask(string path) : this(new string[] { path })
        {
        }

        public DeleteFileBuilderTask(IEnumerable<string> paths)
        {
            _paths = paths;
        }

        public IBuilderTask.Result Run(IBuilderConfiguration configuration)
        {
            var artifactPath = Builder.GetArtifactPath(configuration);
            var artifactFileInfo = new FileInfo(artifactPath);
            var artifactDirectionInfo = artifactFileInfo.Directory;
            if (!artifactDirectionInfo.Exists)
                return IBuilderTask.Result.Skipped;

            var skipped = true;
            foreach (var path in _paths)
            {
                var dis = artifactDirectionInfo.GetDirectoriesSafe(path);
                foreach (var di in dis)
                {
                    if (!di.Exists)
                        continue;

                    di.Delete(recursive: true);
                    skipped = false;
                }

                var fis = artifactDirectionInfo.GetFilesSafe(path);
                foreach (var fi in fis)
                {
                    if (!fi.Exists)
                        continue;

                    fi.Delete();
                    skipped = false;
                }
            }

            if (skipped)
                return IBuilderTask.Result.Skipped;

            return IBuilderTask.Result.Passed;
        }

        public override string ToString()
        {
            return $"{nameof(DeleteFileBuilderTask)} [paths={string.Join(";", _paths)}]";
        }

        private static IEnumerable<string> GetSpecialPathPattern(IEnumerable<SpecialPath> paths)
        {
            return paths.Select(x => GetSpecialPathPattern(x));
        }

        private static string GetSpecialPathPattern(SpecialPath path)
        {
            switch (path)
            {
                case SpecialPath.BurstDebugFolder:
                    return "*_BurstDebugInformation_DoNotShip";

                default:
                    throw new System.Exception("unsupported type " + path);
            }
        }

        public enum SpecialPath
        {
            BurstDebugFolder = 0,
        }
    }
}
