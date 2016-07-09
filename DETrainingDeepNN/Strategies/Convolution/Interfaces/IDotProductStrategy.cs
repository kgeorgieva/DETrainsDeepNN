﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DETrainingDeepNN.Strategies.Convolution
{
    public interface IDotProductStrategy
    {
        double Convolute(double[] imageSection, double[] filter);
    }
}