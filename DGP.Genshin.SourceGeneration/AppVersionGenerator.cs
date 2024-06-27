using Microsoft.CodeAnalysis;
using System;

namespace DGP.Genshin.SourceGeneration
{
    [Generator]
    public class AppVersionGenerator : ISourceGenerator
    {
        private const string AutoVersionKey = "SGAppAutoVersion";
        private const string AutoVersionEnabledKey = "SGAppAutoVersionEnabled";

        public void Execute(GeneratorExecutionContext context)
        {
            string? enabled = Environment.GetEnvironmentVariable(AutoVersionEnabledKey, EnvironmentVariableTarget.User);
            string sourceCode;

            if (enabled == "true")
            {
                string version = Environment.GetEnvironmentVariable(AutoVersionKey, EnvironmentVariableTarget.User);

                sourceCode = $@"using System.Reflection;

[assembly: AssemblyVersion(""{version}"")]";
            }
            else
            {
                sourceCode = $@"using System.Reflection;

[assembly: AssemblyVersion(""2022.4.24.0"")]";
            }
            context.AddSource("AssemblyInfo.g.cs", sourceCode);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            string currentVersion = $"{DateTime.Now:yyyy.M.d}.0";

            //环境变量中存在版本号
            if (Environment.GetEnvironmentVariable(AutoVersionKey, EnvironmentVariableTarget.User) is string oldVersion)
            {
                if (new Version(currentVersion) <= new Version(oldVersion))
                {
                    currentVersion = oldVersion;
                }
            }

            //递增版本号
            string[] versionSegments = currentVersion.Split('.');

            versionSegments[3] = (int.Parse(versionSegments[3]) + 1).ToString();
            currentVersion = string.Join(".", versionSegments);

            Environment.SetEnvironmentVariable(AutoVersionKey, currentVersion, EnvironmentVariableTarget.User);
        }
    }
}
