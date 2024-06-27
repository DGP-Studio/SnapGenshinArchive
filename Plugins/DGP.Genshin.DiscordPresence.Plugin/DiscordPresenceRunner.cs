using DGP.Genshin.DiscordPresence.Plugin.DiscordRpc;
using Microsoft;
using Microsoft.VisualStudio.Threading;
using Snap.Win32.NativeMethod;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.DiscordPresence.Plugin;

/// <summary>
/// Discord 状态运行器
/// </summary>
public class DiscordPresenceRunner
{
    /// <summary>
    /// 异步运行
    /// </summary>
    /// <param name="token">取消令牌</param>
    /// <returns>任务</returns>
    public async Task RunAsync(CancellationToken token)
    {
        using DiscordRpcClient? zhClient = new("757909870258159646");
        using DiscordRpcClient? enClient = new("761911105081442335");
        zhClient.Initialize();
        enClient.Initialize();

        bool playing = false;

        while (!token.IsCancellationRequested)
        {
            try
            {
                await Task
                    .Delay(1000, token)

                    // free up current thread and seek for any available thread
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // immediately exit
                break;
            }

            IntPtr hwndZh = User32.FindWindow("UnityWndClass", "原神");
            IntPtr hwndEn = User32.FindWindow("UnityWndClass", "Genshin Impact");

            // no game process found, continue loop
            if (hwndZh == IntPtr.Zero && hwndEn == IntPtr.Zero)
            {
                TryClearPresence(ref playing, zhClient, enClient);
                continue;
            }

            // have game process
            try
            {
                Process[] processes = Process.GetProcesses();
                Process? procEn = processes.FirstOrDefault(x => x.MainWindowHandle == hwndEn);
                Process? procZh = processes.FirstOrDefault(x => x.MainWindowHandle == hwndZh);

                Verify.Operation(procEn != null || procZh != null, "Game process not matched.");

                string processName = procZh?.ProcessName ?? procEn?.ProcessName ?? string.Empty;
                _ = processName switch
                {
                    "YuanShen" => TrySetPresence(ref playing, zhClient, "原神", "提瓦特大陆"),
                    "GenshinImpact" => TrySetPresence(ref playing, enClient, "Genshin Impact", "Teyvat continent"),
                    _ => TryClearPresence(ref playing, zhClient, enClient),
                };
            }
            catch
            {
                TryClearPresence(ref playing, zhClient, enClient);
            }
        }
    }

    private static bool TrySetPresence(ref bool playing, DiscordRpcClient clientEn, string gameName, string state)
    {
        if (playing)
        {
            return playing;
        }

        playing = true;

        clientEn.SetPresence(new()
        {
            Timestamps = Timestamps.Now,
            State = state,
            Assets = new()
            {
                LargeImageKey = "genshin",
                LargeImageText = gameName,
            },
        });

        return playing;
    }

    private static bool TryClearPresence(ref bool playing, DiscordRpcClient zhClient, DiscordRpcClient enClient)
    {
        playing = false;

        if (enClient.CurrentPresence != null)
        {
            enClient.ClearPresence();
        }

        if (zhClient.CurrentPresence != null)
        {
            zhClient.ClearPresence();
        }

        return playing;
    }
}