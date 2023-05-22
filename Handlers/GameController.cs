using System;
using Hotswap;

namespace SwissArmyKnife.Handlers; 

public class GameController {
    public static Patcher? PatcherInstance { get; private set; }
    public static IProject? Project { get; private set; }
    public static void Init(string BaseROMConfigurationPath) {
        PatcherInstance = new Patcher(BaseROMConfigurationPath);
    }

    public static bool HandleProject(IProject Configuration) {
        Project = Configuration;
        PatcherInstance?.LoadProject(Configuration);
        return true;
    }
}