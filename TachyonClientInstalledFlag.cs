using TachyonCommon;

namespace TachyonClientRPC {
    public class TachyonClientInstalledFlag : ITachyonInstalledFlag {
        public string ActivationArgument => "--client";

        public TachyonClientInstalledFlag() { }
    }
}