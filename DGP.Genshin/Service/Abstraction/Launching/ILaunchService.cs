using DGP.Genshin.Core.ImplementationSwitching;
using DGP.Genshin.DataModel.Launching;
using IniParser.Model;
using Snap.Data.Primitive;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DGP.Genshin.Service.Abstraction.Launching
{
    /// <summary>
    /// 游戏启动服务
    /// </summary>
    [SwitchableInterfaceDefinitionContract]
    public interface ILaunchService
    {
        /// <summary>
        /// 游戏配置文件
        /// </summary>
        IniData GameConfig { get; }

        /// <summary>
        /// 启动器配置文件
        /// </summary>
        IniData LauncherConfig { get; }

        /// <summary>
        /// 游戏启动监视器
        /// </summary>
        WorkWatcher GameWatcher { get; }

        /// <summary>
        /// 异步启动游戏
        /// </summary>
        /// <param name="option">启动方案</param>
        /// <param name="failAction">启动失败回调</param>
        /// <returns>结果</returns>
        Task LaunchAsync(LaunchOption option, Action<Exception> failAction);

        /// <summary>
        /// 加载配置文件数据
        /// </summary>
        /// <param name="launcherPath">启动器路径</param>
        /// <returns>是否加载成功</returns>
        bool TryLoadIniData(string? launcherPath);

        /// <summary>
        /// 启动官方启动器
        /// </summary>
        /// <param name="failAction">启动失败回调</param>
        void OpenOfficialLauncher(Action<Exception>? failAction);

        /// <summary>
        /// 保存启动方案到配置文件
        /// </summary>
        /// <param name="scheme">启动方案</param>
        void SaveLaunchScheme(LaunchScheme? scheme);

        /// <summary>
        /// 选择启动器路径, 若提供有效的路径则直接返回, 否则应使用户选择路径
        /// </summary>
        /// <param name="launcherPath">待检验的启动器路径</param>
        /// <returns>启动器路径</returns>
        string? SelectLaunchDirectoryIfIncorrect(string? launcherPath);

        /// <summary>
        /// 保存所有账号到本地文件
        /// </summary>
        /// <param name="accounts">待保存的账号列表</param>
        void SaveAllAccounts(IEnumerable<GenshinAccount> accounts);

        /// <summary>
        /// 从本地文件加载账号
        /// </summary>
        /// <returns>从文件创建的账号列表</returns>
        ObservableCollection<GenshinAccount> LoadAllAccount();

        /// <summary>
        /// 从注册表获取账号信息
        /// </summary>
        /// <returns>注册表中的账号信息，当注册表中不存在信息时，返回 <see langword="null"/></returns>
        GenshinAccount? GetFromRegistry();

        /// <summary>
        /// 将账号信息写入注册表
        /// </summary>
        /// <param name="account">待写入的账号</param>
        /// <returns>是否写入成功</returns>
        bool SetToRegistry(GenshinAccount? account);

        /// <summary>
        /// 动态更新解锁的帧率
        /// 一般设置内部的unlocker即可
        /// </summary>
        /// <param name="targetFPS">目标帧率</param>
        void SetTargetFPSDynamically(int targetFPS);
    }
}