using Microsoft.CodeAnalysis;
using System;

namespace DGP.Genshin.SourceGeneration
{
    /// <summary>
    /// 需要在系统环境变量中添加
    /// key: SGReleaseAppCenterSecret value: secret
    /// Secret 获取
    /// https://appcenter.ms/orgs/DGP-Studio/apps/SG-Release-New
    /// </summary>
    [Generator]
    public class AppCenterGenerator : ISourceGenerator
    {
        private const string SecretKey = "SGReleaseAppCenterSecret";
        private const string FileName = "AppCenterConfiguration.g.cs";

        public void Execute(GeneratorExecutionContext context)
        {
            string secret = Environment.GetEnvironmentVariable(SecretKey, EnvironmentVariableTarget.User);
            string name = this.GetGeneratorType().Namespace;
            string version = this.GetGeneratorType().Assembly.ImageRuntimeVersion;
            string sourceCode = $@"using DGP.Genshin.Helper;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Snap.Core.Logging;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace DGP.Genshin
{{
    public partial class App : Application
    {{
        [DebuggerNonUserCode]
        [GeneratedCode(""{name}"",""{version}"")]
        partial void ConfigureAppCenter(bool enabled)
        {{
            if (enabled)
            {{
                AppCenter.SetUserId(User.Id);
                //AppCenter.LogLevel = LogLevel.Verbose;
#if DEBUG
                //DEBUG INFO should send to Snap Genshin Debug kanban
                AppCenter.Start(""2e4fa440-132e-42a7-a288-22ab1a8606ef"", typeof(Analytics), typeof(Crashes));
#else
                //开发测试人员请不要生成 Release 版本
                if (!System.Diagnostics.Debugger.IsAttached)
                {{
                    //RELEASE INFO should send to Snap Genshin kanban
                    AppCenter.Start(""{secret}"", typeof(Analytics), typeof(Crashes));
                }}
                else
                {{
                    throw Microsoft.Verify.FailOperation(""请不要生成 Release 版本"");
                }}
#endif
                this.Log(""AppCenter Initialized"");
            }}
        }}
    }}
}}";
            context.AddSource(FileName, sourceCode);
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
