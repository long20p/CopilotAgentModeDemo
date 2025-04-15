using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackPropagation2
{
    class BPGlobalAccess
    {
        //public const int TOLERABLE_PIXELS = 3;
        public const int BRUSH_RADIUS = 5;

        public static BackProgagationForm BPForm;
        public static int INPUT_COUNT = 800;
        public static int OUTPUT_COUNT = 256;
        public static String[] LETTERS = { "A", "B", "C", "D", "E", "F", "G",
                                           "H", "I", "J", "K", "L", "M", "N",
                                           "O", "P", "Q", "R", "S", "T", "U",
                                           "V", "W", "X", "Y", "Z" };
    }
}
