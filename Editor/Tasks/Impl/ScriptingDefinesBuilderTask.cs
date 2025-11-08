using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BuildSystem
{
    public class ScriptingDefinesBuilderTask : IBuilderTask
    {
        private string _scriptingDefine;
        private bool _isEnabled;

        public ScriptingDefinesBuilderTask(string scriptingDefine, bool isEnabled)
        {
            _scriptingDefine = scriptingDefine;
            _isEnabled = isEnabled;
        }

        public IBuilderTask.Result Run(IBuilderConfiguration configuration, BuildSummary summary)
        {
            var isSkipped = false;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(configuration.targetGroup);
            var list = ToList(defines);
            var exists = list.Contains(_scriptingDefine);
            if (exists)
            {
                if (!_isEnabled)
                    list.Remove(_scriptingDefine);
                else
                    isSkipped = true;
            }
            else
            {
                if (_isEnabled)
                    list.Add(_scriptingDefine);
                else
                    isSkipped = true;
            }

            if (isSkipped)
                return IBuilderTask.Result.Skipped;

            defines = ToString(list);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(configuration.targetGroup, defines);
            return IBuilderTask.Result.Passed;
        }

        private static List<string> ToList(string str)
        {
            var result = new List<string>();
            var splitted = str.Split(';');
            for (int i = 0; i < splitted.Length; ++i)
            {
                if (string.IsNullOrEmpty(splitted[i]))
                    continue;

                result.Add(splitted[i]);
            }
            return result;
        }

        private static string ToString(IEnumerable<string> enumerable)
        {
            return string.Join(";", enumerable);
        }

        public override string ToString()
        {
            return $"{nameof(ScriptingDefinesBuilderTask)} [scriptingDefine={_scriptingDefine}, isEnabled={_isEnabled}]";
        }
    }
}
