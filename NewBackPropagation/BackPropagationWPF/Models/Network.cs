namespace BackPropagationWPF.Models
{
    public class Network
    {
        private Layer outputLayer;
        private Layer[] hiddenLayers;
        private double totalError;
        private double allowedError;

        // Event for updating training progress
        public delegate void TrainingProgressHandler(string message, double error, int round);
        public event TrainingProgressHandler OnTrainingProgress;

        // Event for recognition results
        public delegate void RecognitionResultHandler(string result, Dictionary<string, double> allResults);
        public event RecognitionResultHandler OnRecognitionResult;

        public double AllowedError
        {
            get => allowedError;
            set => allowedError = value;
        }

        /// <summary>
        /// Creates a new neural network with the specified architecture
        /// </summary>
        /// <param name="hiddenLayerCount">Number of hidden layers</param>
        /// <param name="elementsPerLayer">Array specifying number of neurons in each hidden layer</param>
        /// <param name="outputCount">Number of output neurons</param>
        /// <param name="inputCount">Number of input neurons</param>
        public Network(int hiddenLayerCount, int[] elementsPerLayer, int outputCount, int inputCount)
        {
            hiddenLayers = new Layer[hiddenLayerCount];

            for (int j = 0; j < hiddenLayerCount; j++)
            {
                int lowerLayerElementCount = j == 0 ? inputCount : elementsPerLayer[j - 1];
                hiddenLayers[j] = new Layer();
                hiddenLayers[j].InitializeLayer(elementsPerLayer[j], lowerLayerElementCount);
            }

            outputLayer = new Layer();
            outputLayer.InitializeLayer(outputCount, elementsPerLayer[elementsPerLayer.Length - 1]);
        }

        /// <summary>
        /// Trains the network using the provided training set
        /// </summary>
        /// <param name="trainingSet">Collection of training patterns</param>
        /// <param name="maxIterations">Maximum number of training iterations</param>
        /// <returns>A task that completes when training is finished</returns>
        public async Task TrainAsync(List<Pattern> trainingSet, int maxIterations)
        {
            OnTrainingProgress?.Invoke("Training started...", 0, 0);

            // Run the intensive training process on a background thread
            await Task.Run(() =>
            {
                int round = 0;

                do
                {
                    round++;
                    totalError = 0;

                    foreach (Pattern pattern in trainingSet)
                    {
                        // Forward pass - calculate outputs
                        FireNetwork(pattern);

                        // Calculate error and gradients for backpropagation
                        double outLayerError = CalculateError(pattern);

                        // Backward pass - adjust weights
                        EvaluateAndApplyDeltaW(pattern);

                        // Add to total error
                        totalError += outLayerError;
                    }

                    totalError = totalError / 2;

                    // Report progress
                    OnTrainingProgress?.Invoke($"Training round {round}, Error: {totalError}", totalError, round);

                    if (round >= maxIterations)
                        break;

                } while (totalError > allowedError);

                if (round <= maxIterations)
                {
                    OnTrainingProgress?.Invoke($"Training completed successfully after {round} rounds.", totalError, round);
                }
                else
                {
                    OnTrainingProgress?.Invoke("Training reached maximum iterations without converging.", totalError, round);
                }
            });
        }

        /// <summary>
        /// Recognizes a pattern using the trained network
        /// </summary>
        /// <param name="inputs">Input values to recognize</param>
        /// <param name="possibleOutputs">Array of possible output labels</param>
        public void Recognize(double[] inputs, string[] possibleOutputs)
        {
            CalculateOutputs(inputs);

            double maxOutput = -1;
            int maxIndex = -1;
            var outputPercentages = new Dictionary<string, double>();

            // Find the highest output and calculate percentages
            for (int i = 0; i < outputLayer.PerceptronOutput.Length; i++)
            {
                double percentage = Math.Round(outputLayer.PerceptronOutput[i] * 100, 2);

                if (i < possibleOutputs.Length)
                {
                    outputPercentages[possibleOutputs[i]] = percentage;
                }

                if (outputLayer.PerceptronOutput[i] > maxOutput)
                {
                    maxOutput = outputLayer.PerceptronOutput[i];
                    maxIndex = i;
                }
            }

            string result = "";
            if (maxIndex >= 0 && maxIndex < possibleOutputs.Length)
            {
                result = $"Pattern is most likely '{possibleOutputs[maxIndex]}' ({outputPercentages[possibleOutputs[maxIndex]]}%)";
            }

            OnRecognitionResult?.Invoke(result, outputPercentages);
        }

        /// <summary>
        /// Forward pass - calculates the output of all neurons in the network
        /// </summary>
        private void CalculateOutputs(double[] inputValues)
        {
            // Process hidden layers
            for (int j = 0; j < hiddenLayers.Length; j++)
            {
                double[] inputs = j == 0 ? inputValues : hiddenLayers[j - 1].PerceptronOutput;

                for (int k = 0; k < hiddenLayers[j].Perceptrons.Count; k++)
                {
                    hiddenLayers[j].PerceptronOutput[k] = hiddenLayers[j].Perceptrons[k].GetOutput(inputs);
                }
            }

            // Process output layer
            double[] lastHiddenLayerOutputs = hiddenLayers[hiddenLayers.Length - 1].PerceptronOutput;
            for (int i = 0; i < outputLayer.Perceptrons.Count; i++)
            {
                outputLayer.PerceptronOutput[i] = outputLayer.Perceptrons[i].GetOutput(lastHiddenLayerOutputs);
            }
        }

        /// <summary>
        /// Forward pass for a pattern - same as CalculateOutputs but takes a Pattern object
        /// </summary>
        private void FireNetwork(Pattern pattern)
        {
            CalculateOutputs(pattern.InputsX);
        }

        /// <summary>
        /// Calculates error gradients for all neurons in the network
        /// </summary>
        private double CalculateError(Pattern pattern)
        {
            // Calculate error for output layer
            double outLayerError = 0;

            for (int i = 0; i < outputLayer.PerceptronOutput.Length; i++)
            {
                // Calculate error: (actual - expected)²
                double diff = outputLayer.PerceptronOutput[i] - pattern.ExpectedOutput[i];
                outLayerError += Math.Pow(diff, 2);

                // Calculate gradient for output layer
                var perceptron = outputLayer.Perceptrons[i];
                perceptron.dE_dY = diff;
                perceptron.dy_dz = perceptron.Lambda * perceptron.YValue * (1 - perceptron.YValue);
            }

            // Backpropagate error to hidden layers (top-down)
            for (int j = hiddenLayers.Length - 1; j >= 0; j--)
            {
                for (int k = 0; k < hiddenLayers[j].Perceptrons.Count; k++)
                {
                    // Get perceptrons from layer above this one
                    IReadOnlyList<Perceptron> higherLayerPerceptrons =
                        j == (hiddenLayers.Length - 1) ? outputLayer.Perceptrons : hiddenLayers[j + 1].Perceptrons;

                    hiddenLayers[j].Perceptrons[k].dE_dY = 0;

                    // Sum up contributions from all connected neurons in the layer above
                    for (int t = 0; t < higherLayerPerceptrons.Count; t++)
                    {
                        Perceptron higherPerceptron = higherLayerPerceptrons[t];

                        // Error contribution from higher layer
                        hiddenLayers[j].Perceptrons[k].dE_dY +=
                            higherPerceptron.dE_dY *
                            hiddenLayers[j].Perceptrons[k].Lambda *
                            higherPerceptron.YValue *
                            (1 - higherPerceptron.YValue) *
                            higherPerceptron.Weights[k];
                    }

                    // Calculate derivative of activation function
                    hiddenLayers[j].Perceptrons[k].dy_dz =
                        hiddenLayers[j].Perceptrons[k].Lambda *
                        hiddenLayers[j].Perceptrons[k].YValue *
                        (1 - hiddenLayers[j].Perceptrons[k].YValue);
                }
            }

            return outLayerError;
        }

        /// <summary>
        /// Updates weights based on calculated gradients
        /// </summary>
        private void EvaluateAndApplyDeltaW(Pattern pattern)
        {
            // Update weights for hidden layers (bottom-up)
            for (int j = 0; j < hiddenLayers.Length; j++)
            {
                double[] lowerLevelInputs = j == 0 ? pattern.InputsX : hiddenLayers[j - 1].PerceptronOutput;

                for (int k = 0; k < hiddenLayers[j].Perceptrons.Count; k++)
                {
                    Perceptron perceptron = hiddenLayers[j].Perceptrons[k];

                    for (int i = 0; i < perceptron.DeltaW.Length; i++)
                    {
                        // Weight update rule: dw = -η * dE/dy * dy/dz * input
                        perceptron.DeltaW[i] = -1 * perceptron.Eta * perceptron.dE_dY * perceptron.dy_dz * lowerLevelInputs[i];
                        perceptron.Weights[i] += perceptron.DeltaW[i];

                        // Threshold update
                        perceptron.Theta += perceptron.Phi * perceptron.dE_dY * perceptron.YValue * (1 - perceptron.YValue) * perceptron.Lambda;
                    }
                }
            }

            // Update weights for output layer
            double[] lastHiddenLayerOutputs = hiddenLayers[hiddenLayers.Length - 1].PerceptronOutput;

            for (int k = 0; k < outputLayer.Perceptrons.Count; k++)
            {
                Perceptron perceptron = outputLayer.Perceptrons[k];

                for (int i = 0; i < perceptron.DeltaW.Length; i++)
                {
                    // Same weight update rule as for hidden layers
                    perceptron.DeltaW[i] = -1 * perceptron.Eta * perceptron.dE_dY * perceptron.dy_dz * lastHiddenLayerOutputs[i];
                    perceptron.Weights[i] += perceptron.DeltaW[i];

                    // Threshold update
                    perceptron.Theta += perceptron.Phi * perceptron.dE_dY * perceptron.YValue * (1 - perceptron.YValue) * perceptron.Lambda;
                }
            }
        }
    }
}
