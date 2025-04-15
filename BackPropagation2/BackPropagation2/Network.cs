using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BackPropagation2
{
    class Network
    {
        private Layer outLayer;
        private Layer[] hiddenLayers;
        private double totalError;
        private double allowedError;

        public double AllowedError
        {
            get { return allowedError; }
            set { allowedError = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hiddenLayerCount"></param>
        /// <param name="elementCount"></param>
        /// <param name="outElementCount"></param>
        public Network(int hiddenLayerCount, int[] elementCount, int outElementCount)
        {
            hiddenLayers = new Layer[hiddenLayerCount];
            for (int j = 0; j < hiddenLayerCount; j++)
            {
                int lowerLayerElementCount = j == 0 ? BPGlobalAccess.INPUT_COUNT : elementCount[j - 1];
                hiddenLayers[j] = new Layer();
                hiddenLayers[j].InitializeLayer(elementCount[j], lowerLayerElementCount);
            }
            outLayer = new Layer();
            outLayer.InitializeLayer(outElementCount, elementCount[elementCount.Length - 1]);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Train()
        {
            BPGlobalAccess.BPForm.UpdateProcessLabel("Training...");
            Program.AllocConsole(); // open console
            int round = 0;
            do
            {
                round++;
                totalError = 0;
                foreach (Pattern pattern in Program.TrainingSet)
                {
                    /* calculate output for every perceptron in network */
                    fireNetwork(pattern);
                    /* calculate error for every perceptron in network 
                     * and return outLayer's total error for pattern 
                     */
                    double outLayerError = calculateError(pattern);
                    /* calculate dw, adjust w */
                    evaluateAndApplyDeltaW(pattern);
                    /* add to total error */
                    totalError += outLayerError;
                }

                totalError = totalError / 2;
                Console.WriteLine("Total error : " + totalError + "\tRound: " + round);
                if (round > Program.MaxIteration)
                    break;
            } while (totalError > allowedError);
            Program.FreeConsole(); // close console
            if (round <= Program.MaxIteration)
                BPGlobalAccess.BPForm.UpdateProcessLabel("Learning successfully after " + round + " rounds.");
            else
                BPGlobalAccess.BPForm.UpdateProcessLabel("Learning failed.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        public void Recognize(double[] inputs)
        {
            BPGlobalAccess.BPForm.UpdateResultLabel("Recognizing...");
            calculateOutputs(inputs);
            double max = -1;
            int index = -1;
            double[] percents = new double[outLayer.PerceptronOutput.Length];
            for (int i = 0; i < outLayer.PerceptronOutput.Length; i++)
            {
                percents[i] = Math.Round(outLayer.PerceptronOutput[i] * 100, 2);
                if (outLayer.PerceptronOutput[i] > max)
                {
                    max = outLayer.PerceptronOutput[i];
                    index = i;
                }
            }
            BPGlobalAccess.BPForm.UpdateResultLabel("Pattern is most likely letter " + 
                BPGlobalAccess.LETTERS[index] + " (" + percents[index] + "%)");
            for (int k = 0; k < outLayer.PerceptronOutput.Length; k+=2)
            {
                String line = BPGlobalAccess.LETTERS[k] + " : " + percents[k] + "%";
                if (k + 1 < outLayer.PerceptronOutput.Length)
                    line += "          " +
                        BPGlobalAccess.LETTERS[k + 1] + " : " + percents[k + 1] + "%";
                BPGlobalAccess.BPForm.UpdateResultLabel(line);
            }
        }

        private void calculateOutputs(double[] inputVals)
        {
            ////////////  hiddenLayers  ////////////////
            for (int j = 0; j < hiddenLayers.Length; j++)
            {
                double[] inputs = j == 0 ? inputVals : hiddenLayers[j - 1].PerceptronOutput;
                for (int k = 0; k < hiddenLayers[j].Perc.Count; k++)
                {
                    hiddenLayers[j].PerceptronOutput[k] = hiddenLayers[j].Perc[k].GetOutput(inputs);
                }
            }
            ////////////  outLayer  ////////////////////
            for (int i = 0; i < outLayer.Perc.Count; i++)
            {
                double[] inputs = hiddenLayers[hiddenLayers.Length - 1].PerceptronOutput;
                outLayer.PerceptronOutput[i] =
                    outLayer.Perc[i].GetOutput(inputs);
            }
        }

        /// <summary>
        /// Calculate output value for every perceptron in network
        /// </summary>
        /// <param name="inputs"></param>
        private void fireNetwork(Pattern pattern)
        {
            ////////////  hiddenLayers  ////////////////
            for (int j = 0; j < hiddenLayers.Length; j++)
            {
                double[] inputs = j == 0 ? pattern.InputsX : hiddenLayers[j - 1].PerceptronOutput;
                for (int k = 0; k < hiddenLayers[j].Perc.Count; k++)
                {
                    hiddenLayers[j].PerceptronOutput[k] = hiddenLayers[j].Perc[k].GetOutput(inputs);
                }
            }
            ////////////  outLayer  ////////////////////
            for (int i = 0; i < outLayer.Perc.Count; i++)
            {
                double[] inputs = hiddenLayers[hiddenLayers.Length - 1].PerceptronOutput;
                outLayer.PerceptronOutput[i] =
                    outLayer.Perc[i].GetOutput(inputs);
            }
        }

        /// <summary>
        /// Calculate error of every perceptron in network
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private double calculateError(Pattern pattern)
        {
            ////////  outLayer  ///////////
            double outLayerError = 0;
            for (int i = 0; i < outLayer.PerceptronOutput.Length; i++)
            {
                // (yi - oi)^2
                double d = outLayer.PerceptronOutput[i] - pattern.ExpectedOutput[i];
                outLayerError += Math.Pow(d, 2);
                // dE_dy = delta
                outLayer.Perc[i].dE_dY = d;
                // dy_dz = lambda * y * (1 - y)
                outLayer.Perc[i].dy_dz =
                    outLayer.Perc[i].Lambda * outLayer.Perc[i].YValue * (1 - outLayer.Perc[i].YValue);
            }
            ////////  hiddenLayers, top - down  //////////////
            for (int j = hiddenLayers.Length - 1; j >= 0; j--)
            {
                for (int k = 0; k < hiddenLayers[j].Perc.Count; k++)
                {
                    List<Perceptron> higherLayerPerc =
                        j == (hiddenLayers.Length - 1) ? outLayer.Perc : hiddenLayers[j + 1].Perc;
                    hiddenLayers[j].Perc[k].dE_dY = 0;
                    for (int t = 0; t < higherLayerPerc.Count; t++)
                    {
                        Perceptron per = higherLayerPerc[t];
                        // delta = sum(deltaT * lambda * yT * (1 - yT) * wT)
                        hiddenLayers[j].Perc[k].dE_dY +=
                            per.dE_dY * hiddenLayers[j].Perc[k].Lambda * per.YValue * (1 - per.YValue) * per.Weights[k];
                    }
                    // dy_dz = lambda * y * (1 - y)
                    hiddenLayers[j].Perc[k].dy_dz =
                        hiddenLayers[j].Perc[k].Lambda * hiddenLayers[j].Perc[k].YValue * (1 - hiddenLayers[j].Perc[k].YValue);
                }
            }
            return outLayerError;
        }

        /// <summary>
        /// Adjust weights of every perceptron
        /// </summary>
        /// <param name="pattern"></param>
        private void evaluateAndApplyDeltaW(Pattern pattern)
        {
            //////////  hiddenLayers, bottom - up  ////////////////
            for (int j = 0; j < hiddenLayers.Length; j++)
            {
                double[] lowerLevelInputs = j == 0 ? pattern.InputsX : hiddenLayers[j - 1].PerceptronOutput;
                for (int k = 0; k < hiddenLayers[j].Perc.Count; k++)
                {
                    Perceptron perc = hiddenLayers[j].Perc[k];
                    for (int i = 0; i < perc.DeltaW.Length; i++)
                    {
                        // dwi = -eta * dE_dy * dy_dz * xi
                        perc.DeltaW[i] = -1 * perc.Eta * perc.dE_dY * perc.dy_dz * lowerLevelInputs[i];
                        perc.Weights[i] += perc.DeltaW[i];
                        // theta = -phi * dE_dy * y * (1 - y) * (-lambda)
                        perc.Theta += perc.Phi * perc.dE_dY * perc.YValue * (1 - perc.YValue) * perc.Lambda;
                    }
                }
            }
            ////////////  outLayer  //////////////
            double[] lastLevelInputs = hiddenLayers[hiddenLayers.Length - 1].PerceptronOutput;
            for (int k = 0; k < outLayer.Perc.Count; k++)
            {
                Perceptron per = outLayer.Perc[k];
                for (int i = 0; i < per.DeltaW.Length; i++)
                {
                    // dwi = -eta * dE_dy * dy_dz * xi
                    per.DeltaW[i] = -1 * per.Eta * per.dE_dY * per.dy_dz * lastLevelInputs[i];
                    per.Weights[i] += per.DeltaW[i];
                    // theta = -phi * dE_dy * y * (1 - y) * (-lambda)
                    per.Theta += per.Phi * per.dE_dY * per.YValue * (1 - per.YValue) * per.Lambda;
                }
            }
        }
    }
}
