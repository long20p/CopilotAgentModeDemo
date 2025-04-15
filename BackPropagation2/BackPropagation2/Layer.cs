using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackPropagation2
{
    class Layer
    {
        private List<Perceptron> perc;
        private double[] perceptronOutput;

        public List<Perceptron> Perc
        {
            get { return perc; }
        }

        public double[] PerceptronOutput
        {
            get { return perceptronOutput; }
        }

        public Layer()
        {
            perc = new List<Perceptron>();
        }

        /// <summary>
        /// Initialize layer and its perceptrons
        /// </summary>
        /// <param name="elementCount">number of elements of layer</param>
        /// <param name="weightCount">number of weights, also number of elements of lower layer</param>
        public void InitializeLayer(int elementCount, int weightCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                Perceptron per = new Perceptron(weightCount);
                Perc.Add(per);
            }
            perceptronOutput = new double[perc.Count];
        }
    }
}
