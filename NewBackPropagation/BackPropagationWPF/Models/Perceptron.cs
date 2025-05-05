namespace BackPropagationWPF.Models
{
    public class Perceptron
    {
        private double[] weights;
        private double[] deltaW;
        private double zValue;
        private double yValue;
        private double theta; // bias

        // Learning parameters
        private readonly double eta = 0.2;    // Learning rate
        private readonly double phi = 0.2;    // Momentum coefficient
        private readonly double lambda = 1.0; // Steepness of the sigmoid

        public double[] Weights => weights;
        public double YValue => yValue;
        public double Lambda => lambda;
        public double Eta => eta;
        public double Phi => phi;
        public double[] DeltaW => deltaW;

        public double Theta
        {
            get => theta;
            set => theta = value;
        }

        public double dE_dY { get; set; } // Error gradient
        public double dy_dz { get; set; } // Activation function derivative

        public Perceptron(int weightCount)
        {
            weights = new double[weightCount];
            deltaW = new double[weightCount];

            var random = new Random();

            // Initialize weights with small random values
            for (int j = 0; j < weightCount; j++)
            {
                weights[j] = random.NextDouble() * 0.6 - 0.3;
            }

            // Initialize threshold with small random value
            theta = random.NextDouble() * 0.6 - 0.3;
        }

        public double GetOutput(double[] inputsX)
        {
            // Calculate weighted sum
            zValue = 0;
            for (int i = 0; i < inputsX.Length; i++)
            {
                zValue += weights[i] * inputsX[i];
            }

            // Apply sigmoid activation function
            double exponent = -1 * lambda * (zValue - theta);
            yValue = 1 / (1 + Math.Exp(exponent));

            return yValue;
        }
    }
}
