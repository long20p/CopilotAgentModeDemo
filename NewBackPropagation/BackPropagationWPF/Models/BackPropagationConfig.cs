namespace BackPropagationWPF.Models
{
    /// <summary>
    /// Global configuration and constants for the backpropagation neural network
    /// </summary>
    public static class BackPropagationConfig
    {
        // UI Parameters
        public const int BrushRadius = 5;

        // Network Parameters
        public static int InputCount { get; set; } = 900;  // 30x30 pixels by default
        public static int OutputCount { get; set; } = 26;  // 26 letters by default

        // Letter labels (A-Z)
        public static readonly string[] Letters = {
            "A", "B", "C", "D", "E", "F", "G",
            "H", "I", "J", "K", "L", "M", "N",
            "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z"
        };
    }
}
