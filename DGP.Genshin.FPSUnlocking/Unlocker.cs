using Microsoft;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.System.Diagnostics.ToolHelp;
using static Windows.Win32.PInvoke;

namespace DGP.Genshin.FPSUnlocking
{
    /// <summary>
    /// FPS Unlocker
    /// 需要 .NET 进程为 64 位 才能正常使用
    /// <para/>
    /// Credit to @Crskycode Github
    /// </summary>
    public class Unlocker
    {
        private const string UnityPlayerDllName = "UnityPlayer.dll";

        /// <summary>
        /// 设置fps位的偏移量
        /// </summary>
        private nuint fpsAddress;

        /// <summary>
        /// 游戏进程
        /// </summary>
        private readonly Process gameProcess;

        /// <summary>
        /// 当前解锁器是否无效
        /// </summary>
        private bool isInvalid = false;

        /// <summary>
        /// 目标FPS,运行动态设置以动态更改帧率
        /// </summary>
        public int TargetFPS
        {
            get;
            set;
        }

        /// <summary>
        /// 构造一个新的 <see cref="Unlocker"/> 对象，
        /// 每个解锁器只能解锁一次原神的进程，
        /// 再次解锁需要重新创建对象
        /// <para/>
        /// 解锁器需要在管理员模式下才能正确的完成解锁操作，
        /// 非管理员模式不能解锁
        /// </summary>
        /// <param name="gameProcess">游戏进程</param>
        /// <param name="targetFPS">目标fps</param>
        public Unlocker(Process gameProcess, int targetFPS)
        {
            Verify.Operation(Environment.Is64BitProcess, $"无法在32位进程中使用 {nameof(Unlocker)}");
            Requires.Range(targetFPS >= 30 && targetFPS <= 2000, nameof(targetFPS));

            TargetFPS = targetFPS;
            this.gameProcess = gameProcess;
        }

        /// <summary>
        /// 启动进程，然后调用<see cref="UnlockAsync(int, int, int)"/>
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">每次查找UnityPlayer的延时,默认100毫秒</param>
        /// <param name="findModuleTimeMillisecondLimit">查找UnityPlayer的最大阈值,默认10000毫秒</param>
        /// <param name="adjustFpsMillisecondsDelay">每次循环调整的间隔时间，默认2000毫秒</param>
        /// <returns>解锁的结果</returns>
        public Task<UnlockResult> StartProcessAndUnlockAsync(int findModuleMillisecondsDelay = 100, int findModuleTimeMillisecondLimit = 10000, int adjustFpsMillisecondsDelay = 2000)
        {
            if (gameProcess.Start())
            {
                return UnlockAsync(findModuleMillisecondsDelay, findModuleTimeMillisecondLimit, adjustFpsMillisecondsDelay);
            }
            else
            {
                return Task.FromResult(UnlockResult.ProcessStartFailed);
            }
        }

        /// <summary>
        /// 异步的解锁原神进程的帧数限制，
        /// 只调整了fps，并没有调整垂直同步限制，需要用户手动关闭，
        /// 调用不会阻止，直到遇到错误或原神进程结束前不会返回
        /// <para/>
        /// 尽可能在 async void 方法内使用此方法，以使调用等待不影响UI线程
        /// <para/>
        /// 用法
        /// <code>
        /// Process p = new(){...};
        /// Unlocker unlocker = new(p,144);
        /// p.Start();
        /// var result = await unlocker.UnlockAsync();
        /// </code>
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">每次查找UnityPlayer的延时,默认100毫秒</param>
        /// <param name="findModuleTimeMillisecondsLimit">查找UnityPlayer的最大阈值,默认10000毫秒</param>
        /// <param name="adjustFpsMillisecondsDelay">每次循环调整的间隔时间，默认2000毫秒</param>
        /// <returns>解锁的结果</returns>
        public async Task<UnlockResult> UnlockAsync(int findModuleMillisecondsDelay = 100, int findModuleTimeMillisecondsLimit = 10000, int adjustFpsMillisecondsDelay = 2000)
        {
            Verify.Operation(!isInvalid, "解锁器已经失效");
            Verify.Operation(!gameProcess.HasExited, "游戏进程已退出");

            MODULEENTRY32? module = await FindModuleContinuouslyAsync(findModuleMillisecondsDelay, findModuleTimeMillisecondsLimit)
                .ConfigureAwait(false);

            Verify.Operation(module.HasValue, "查找进程模块超时");
            Verify.Operation(!gameProcess.HasExited, "游戏进程已退出");

            MODULEENTRY32 unityPlayer = module.Value;
            // Read UnityPlayer.dll
            TryGetFpsAddress(unityPlayer);

            // if player switch between scenes, we have to re adjust the fps
            while (true)
            {
                if (!gameProcess.HasExited && fpsAddress != UIntPtr.Zero)
                {
                    UnsafeWriteProcessMemory(gameProcess, fpsAddress, TargetFPS);
                }
                else
                {
                    isInvalid = true;
                    fpsAddress = UIntPtr.Zero;
                    return UnlockResult.Ok;
                }
                await Task.Delay(adjustFpsMillisecondsDelay).ConfigureAwait(false);
            }
        }

        private unsafe void TryGetFpsAddress(MODULEENTRY32 unityPlayer)
        {
            bool readOk = UnsafeReadProcessMemory(gameProcess, unityPlayer, out Span<byte> image);
            Verify.Operation(readOk, "读取内存失败");

            // Find FPS offset
            // 7F 0F              jg   0x11
            // 8B 05 ? ? ? ?      mov eax, dword ptr[rip+?]
            int adr = image.IndexOf(new byte[] { 0x7F, 0x0F, 0x8B, 0x05 });

            Requires.Range(adr >= 0, "未匹配到FPS字节");

            int rip = adr + 2;
            byte a = image[rip + 2];
            int rel = Unsafe.ReadUnaligned<int>(ref image[rip + 2]);
            int ofs = rip + rel + 6;
            fpsAddress = (nuint)((long)unityPlayer.modBaseAddr + ofs);
        }

        private static unsafe bool UnsafeReadProcessMemory(Process process,MODULEENTRY32 entry,out Span<byte> image)
        {
            image = new byte[entry.modBaseSize];
            fixed(byte* lpBuffer = image)
            {
                return ReadProcessMemory(process.SafeHandle, entry.modBaseAddr, lpBuffer, entry.modBaseSize, null);
            }
        }

        private static unsafe bool UnsafeWriteProcessMemory(Process process,nuint baseAddress,int write)
        {
            int* lpBuffer = &write;

            return WriteProcessMemory(process.SafeHandle, (void*)baseAddress, lpBuffer, sizeof(int), null);
        }

        /// <summary>
        /// 循环查找UnityPlayer Module
        /// 调用前需要确保 <see cref="gameProcess"/> 不为 null
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">延迟</param>
        /// <param name="findModuleTimeMillisecondsLimit">上限</param>
        /// <returns>模块</returns>
        private async Task<MODULEENTRY32> FindModuleContinuouslyAsync(int findModuleMillisecondsDelay, int findModuleTimeMillisecondsLimit)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan timeLimit = TimeSpan.FromMilliseconds(findModuleTimeMillisecondsLimit);

            while (true)
            {
                MODULEENTRY32 module = FindModule(gameProcess.Id, UnityPlayerDllName);
                if (module.dwSize != 0)
                {
                    return module;
                }


                if (watch.Elapsed > timeLimit)
                {
                    break;
                }
                await Task.Delay(findModuleMillisecondsDelay).ConfigureAwait(false);
            }
            watch.Stop();
            return default;
        }

        /// <summary>
        /// 在进程中查找对应名称的模块
        /// 在找到模块或创建快照失败后立即返回
        /// </summary>
        /// <param name="processId">进程id</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块</returns>
        private unsafe MODULEENTRY32 FindModule(int processId, string moduleName)
        {
            using (SafeFileHandle snapshot = CreateToolhelp32Snapshot_SafeHandle(CREATE_TOOLHELP_SNAPSHOT_FLAGS.TH32CS_SNAPMODULE, (uint)processId))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());

                MODULEENTRY32 entry = new (){ dwSize = (uint)sizeof(MODULEENTRY32) };
                bool found = false;

                //First module must be exe. Ignoring it.
                for (Module32First(snapshot, ref entry); Module32Next(snapshot, ref entry);)
                {
                    byte* pszModule = (byte*)&entry.szModule;
                    string szModule = Encoding.UTF8.GetString(pszModule, GetLength(pszModule));

                    if (entry.th32ProcessID == processId && szModule == moduleName)
                    {
                        found = true;
                        break;
                    }
                }

                return found ? entry : default;
            }
        }

        private static unsafe int GetLength(byte* pszStr)
        {
            int len = 0;
            while (*pszStr++ != 0)
            {
                len += 1;
            }

            return len;
        }
    }
}