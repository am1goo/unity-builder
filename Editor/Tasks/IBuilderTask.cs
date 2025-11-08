using UnityEditor.Build.Reporting;

namespace BuildSystem
{
    public interface IBuilderTask
    {
        Result Run(IBuilderConfiguration configuration, BuildSummary summary);

        public enum Result : byte
        {
            Failed = 0,
            Passed = 1,
            Skipped = 2,
        }
    }
}
