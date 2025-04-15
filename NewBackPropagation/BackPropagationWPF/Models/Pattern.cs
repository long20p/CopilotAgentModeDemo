namespace BackPropagationWPF.Models
{
    public class Pattern
    {
        public double[] InputsX { get; }
        public double[] ExpectedOutput { get; }

        public Pattern(double[] inputs, double[] expected)
        {
            InputsX = inputs;
            ExpectedOutput = expected;
        }
    }
}
