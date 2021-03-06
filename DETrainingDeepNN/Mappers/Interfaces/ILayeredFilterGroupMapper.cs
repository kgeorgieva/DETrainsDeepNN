﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DETrainingDeepNN.Calculators.Interfaces
{
    public interface ILayeredFilterGroupMapper
    {
        List<List<double[]>> MapToIndividualGroup(Individual individual, int[] layerConfiguration);
    }
}
