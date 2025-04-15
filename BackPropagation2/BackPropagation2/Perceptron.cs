using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackPropagation2
{
    class Perceptron
    {
        private double[] weights;
        private double eta;
        private double phi;
        private double lambda;
        private double zValue;
        private double yValue;
        private double[] deltaW;
        private double theta;

        public double[] Weights
        {
            get { return weights; }
        }

        public double YValue
        {
            get { return yValue; }
        }

        public double Lambda
        {
            get { return lambda; }
        }

        public double Eta
        {
            get { return eta; }
        }

        public double Phi
        {
            get { return phi; }
        }

        public double Theta
        {
            get { return theta; }
            set { theta = value; }
        }

        public double[] DeltaW
        {
            get { return deltaW; }
        }

        public double dE_dY { get; set; }

        public double dy_dz { get; set; }

        public Perceptron(int weightCount)
        {
            weights = new double[weightCount];
            deltaW = new double[weightCount];
            Random ran = new Random();
            lambda = 1;// ran.NextDouble();
            eta = 0.2;
            phi = 0.2;
            for (int j = 0; j < weightCount; j++)
            {
                weights[j] = ran.NextDouble() * 0.6 - 0.3;
            }
            theta = ran.NextDouble() * 0.6 - 0.3;
        }

        public double GetOutput(double[] inputsX)
        {
            zValue = 0;
            for (int i = 0; i < inputsX.Length; i++)
            {
                zValue += weights[i] * inputsX[i];
            }
            double exponent = -1 * lambda * (zValue - theta);
            yValue = 1 / (1 + Math.Exp(exponent));
            return yValue;
        }

    }
}
