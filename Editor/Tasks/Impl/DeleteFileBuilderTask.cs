using System.Collections.Generic;
using System.IO;

namespace BuildSystem
{
    public class DeleteFileBuilderTask : IBuilderTask
    {
        private IEnumerable<string> _paths;

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
                var dis = artifactDirectionInfo.GetDirectories(path);
                foreach (var di in dis)
                {
                    if (!di.Exists)
                        continue;

                    di.Delete(recursive: true);
                    skipped = false;
                }

                var fis = artifactDirectionInfo.GetFiles(path);
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
    }
}
