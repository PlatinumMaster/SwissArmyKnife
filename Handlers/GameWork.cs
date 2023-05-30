using Hotswap;
using Hotswap.Project;

namespace SwissArmyKnife.Avalonia.Handlers; 

public class GameWork {
    public static Patcher? Patcher { get; private set; }
    public static IProject? Project { get; private set; }
    public static void Init(string BaseROMConfigurationPath) {
        Patcher = new Patcher(BaseROMConfigurationPath);
    }

    public static bool HandleProject(IProject Configuration) {
        Project = Configuration;
        Patcher?.LoadProject(Configuration);
        return true;
    }
}