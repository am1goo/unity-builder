using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuildSystem
{
    public class DeleteFileBuilderTask : IBuilderTask
    {
        private RootPath _root;
        private IEnumerable<string> _paths;

        public DeleteFileBuilderTask(IEnumerable<SpecialPath> paths) : this(RootPath.RootFolder, GetSpecialPathPattern(paths))
        {
        }

        public DeleteFileBuilderTask(SpecialPath path) : this(RootPath.RootFolder, GetSpecialPathPattern(path))
        {
        }

        public DeleteFileBuilderTask(RootPath root, string path) : this(root, new string[] { path })
        {
        }

        public DeleteFileBuilderTask(RootPath root, IEnumerable<string> paths)
        {
            _root = root;
            _paths = paths;
        }

        public IBuilderTask.Result Run(IBuilderConfiguration configuration)
        {
            var rootPath = Builder.GetArtifactPath(configuration);
            var rootFileInfo = new FileInfo(rootPath);
            var rootDirectionInfo = rootFileInfo.Directory;
            switch (_root)
            {
                case RootPath.RootFolder:
                    {
                        return RunAtPath(rootDirectionInfo, configuration);
                    }

                case RootPath.DataFolder:
                    {
                        if (Builder.IsOSX(configuration.target))
                        {
                            var di = new DirectoryInfo(Path.Combine(rootDirectionInfo.FullName, $"{configuration.productName}.app", "Contents/Resources/Data"));
                            return RunAtPath(di, configuration);
                        }
                        else
                        {
                            var di = new DirectoryInfo(Path.Combine(rootDirectionInfo.FullName, $"{configuration.productName}_Data"));
                            return RunAtPath(di, configuration);
                        }
                    }

                case RootPath.PluginsFolder:
                    {
                        if (Builder.IsOSX(configuration.target))
                        {
                            var di = new DirectoryInfo(Path.Combine(rootDirectionInfo.FullName, $"{configuration.productName}.app", "Contents/Plugins"));
                            return RunAtPath(di, configuration);
                        }
                        else
                        {
                            var di = new DirectoryInfo(Path.Combine(rootDirectionInfo.FullName, $"{configuration.productName}_Data", "Plugins"));
                            return RunAtPath(di, configuration);
                        }
                    }

                default:
                    throw new Exception($"unsupported type {_root}");
            }
        }

        private IBuilderTask.Result RunAtPath(DirectoryInfo directory, IBuilderConfiguration configuration)
        {
            if (!directory.Exists)
                return IBuilderTask.Result.Skipped;

            var skipped = true;
            foreach (var path in _paths)
            {
                var dis = directory.GetDirectoriesSafe(path);
                foreach (var di in dis)
                {
                    if (!di.Exists)
                        continue;

                    di.Delete(recursive: true);
                    skipped = false;
                }

                var fis = directory.GetFilesSafe(path);
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

                case SpecialPath.Il2CppDebugFolder:
                    return "*_BackUpThisFolder_ButDontShipItWithYourGame";

                default:
                    throw new Exception($"unsupported type {path}");
            }
        }

        public enum RootPath
        {
            RootFolder      = 0,
            DataFolder      = 1,
            PluginsFolder   = 2,
        }

        public enum SpecialPath
        {
            BurstDebugFolder    = 0,
            Il2CppDebugFolder   = 1,
        }
    }
}
