namespace BackPropagationWPF.Models
{
    public class Layer
    {
        private List<Perceptron> perceptrons;
        private double[] perceptronOutputs;

        public IReadOnlyList<Perceptron> Perceptrons => perceptrons;
        public double[] PerceptronOutput => perceptronOutputs;

        public Layer()
        {
            perceptrons = new List<Perceptron>();
        }

        /// <summary>
        /// Initializes the layer with the specified number of perceptrons, each with the specified number of weights
        /// </summary>
        /// <param name="elementCount">Number of perceptrons in this layer</param>
        /// <param name="weightCount">Number of weights for each perceptron (equal to the number of inputs)</param>
        public void InitializeLayer(int elementCount, int weightCount)
        {
            perceptrons.Clear();

            for (int i = 0; i < elementCount; i++)
            {
                var perceptron = new Perceptron(weightCount);
                perceptrons.Add(perceptron);
            }

            perceptronOutputs = new double[perceptrons.Count];
        }
    }
}
