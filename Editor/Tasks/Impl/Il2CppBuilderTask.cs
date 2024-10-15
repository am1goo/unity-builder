using UnityEditor;
using UnityEditor.Build;

namespace BuildSystem
{
    public class Il2CppBuilderTask : IBuilderTask
    {
        private bool _enabled;
        private Il2CppCompilerConfiguration? _configuration;
#if UNITY_2021_2_OR_NEWER
        private Il2CppCodeGeneration? _generation;
#endif
        private string _additionalArgs;

        public Il2CppBuilderTask(bool enabled, Il2CppCompilerConfiguration? configuration = null,
#if UNITY_2021_2_OR_NEWER
            Il2CppCodeGeneration? generation = null,
#endif
            string additionalArgs = null)
        {
            _enabled = enabled;
            _configuration = configuration;
#if UNITY_2021_2_OR_NEWER
            _generation = generation;
#endif
            _additionalArgs = additionalArgs;
        }

        public IBuilderTask.Result Run(IBuilderConfiguration configuration)
        {
            PlayerSettings.SetScriptingBackend(configuration.targetGroup, _enabled ? ScriptingImplementation.IL2CPP : ScriptingImplementation.Mono2x);
            if (_configuration.HasValue)
            {
                PlayerSettings.SetIl2CppCompilerConfiguration(configuration.targetGroup, _configuration.Value);
            }
#if UNITY_2021_2_OR_NEWER
            if (_generation.HasValue)
            {
                NamedBuildTarget namedTarget = NamedBuildTarget.FromBuildTargetGroup(configuration.targetGroup);
                PlayerSettings.SetIl2CppCodeGeneration(namedTarget, _generation.Value);
            }
#endif
            if (!string.IsNullOrWhiteSpace(_additionalArgs))
            {
                PlayerSettings.SetAdditionalIl2CppArgs(_additionalArgs);
            }
            return IBuilderTask.Result.Passed;
        }
    }
}
