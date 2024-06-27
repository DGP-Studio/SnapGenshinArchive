# DGP.Genshin.FPSUnlocking
该项目是 [Snap Genshin Foundation](https://github.com/DGP-Studio/Snap.Genshin) 的一部分  

### 警告 | Warning

**因使用此库中的代码发生一切财产损失及相关后果，均由使用者或最终用户本人承担**  
**All property damages and related consequences arising from the use of the code in this repo shall be borne by the user.**

解锁原神60帧帧率上限  
Unlock Genshin Impact 60 FPS Limit

### Usage

Flexible way

```c#
// create a genshin impact process
Process p = new(){...};

// pass the process and target fps to unlocker
Unlocker unlocker = new(p,144);

// start the process
p.Start();

// immediately call the UnlockAsync method
// this method will not return until an error occurred or the process has exited
var result = await unlocker.UnlockAsync();
```

Straightforward way

```c#
// create a genshin impact process
Process p = new(){...};

// pass the process and target fps to unlocker
Unlocker unlocker = new(p,144);

// start the process and immediately call the UnlockAsync method
// this method will also not return until an error occurred or the process has exited
var result = await unlocker.StartProcessAndUnlockAsync();
```