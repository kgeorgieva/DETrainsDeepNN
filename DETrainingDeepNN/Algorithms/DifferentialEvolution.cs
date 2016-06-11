﻿using DETrainingDeepNN.Strategies.Crossover.Interfaces;
using DETrainingDeepNN.Strategies.Mutation.Interfaces;
using DETrainingDeepNN.Strategies.Selection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DETrainingDeepNN.Algorithms
{
    public class DifferentialEvolution
    {
        private IMutationStrategy mutationStrategy;
        private ICrossoverStrategy crossoverStrategy;
        private ISelectionStrategy selectionStrategy;

        public DifferentialEvolution(IMutationStrategy mutationStrategy, 
                                     ICrossoverStrategy crossoverStrategy, 
                                     ISelectionStrategy selectionStrategy)
        {
            this.mutationStrategy = mutationStrategy;
            this.crossoverStrategy = crossoverStrategy;
            this.selectionStrategy = selectionStrategy;
        }

        public void Run()
        {
            //initialise
            //for each iteration
            //run iteration
        }

        internal List<Individual> InitialisePopulation(int populationSize)
        {
            List<Individual> population = new List<Individual>();

            for(int index = 0; index < populationSize; index++)
            {
                population.Add(new Individual());
            }

            return population;
        }

        internal void RunIteration()
        {
            //for each individual
                //evaluate fitness
                //create trial vector
                //create offspring
                //select
        }
    }
}
