using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackPropagation2
{
    class Pattern
    {
        private double[] inputsX;
        private double[] expectedOutputs;

        public double[] InputsX
        {
            get { return inputsX; }
        }

        public double[] ExpectedOutput
        {
            get { return expectedOutputs; }
        }

        public Pattern(double[] inputs, double[] expected)
        {
            this.inputsX = inputs;
            this.expectedOutputs = expected;
        }
    }
}
