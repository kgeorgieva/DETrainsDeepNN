﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DETrainingDeepNN.Strategies.FitnessEvaluation.Interfaces
{
    public interface IFitnessEvaluationStrategy
    {
        double GetFitnessForIndividual(Individual individual);
    }
}
