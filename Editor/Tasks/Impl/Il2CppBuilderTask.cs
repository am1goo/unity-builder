using UnityEditor;
using UnityEditor.Build;

namespace BuildSystem
{
    public class Il2CppBuilderTask : IBuilderTask
    {
        private bool _enabled;
        private Il2CppCompilerConfiguration? _configuration;
        private Il2CppCodeGeneration? _generation;
        private string _additionalArgs;

        public Il2CppBuilderTask(bool enabled, Il2CppCompilerConfiguration? configuration = null, Il2CppCodeGeneration? generation = null, string additionalArgs = null)
        {
            _enabled = enabled;
            _configuration = configuration;
            _generation = generation;
            _additionalArgs = additionalArgs;
        }

        public IBuilderTask.Result Run(IBuilderConfiguration configuration)
        {
            PlayerSettings.SetScriptingBackend(configuration.targetGroup, _enabled ? ScriptingImplementation.IL2CPP : ScriptingImplementation.Mono2x);
            if (_configuration.HasValue)
            {
                PlayerSettings.SetIl2CppCompilerConfiguration(configuration.targetGroup, _configuration.Value);
            }
            if (_generation.HasValue)
            {
                NamedBuildTarget namedTarget = NamedBuildTarget.FromBuildTargetGroup(configuration.targetGroup);
                PlayerSettings.SetIl2CppCodeGeneration(namedTarget, _generation.Value);
            }
            if (!string.IsNullOrWhiteSpace(_additionalArgs))
            {
                PlayerSettings.SetAdditionalIl2CppArgs(_additionalArgs);
            }
            return IBuilderTask.Result.Passed;
        }
    }
}
