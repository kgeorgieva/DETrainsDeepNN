﻿using System;
using DETrainingDeepNN.Algorithms;
using DETrainingDeepNN;
using System.Collections.Generic;
using Moq;
using DETrainingDeepNN.Strategies.Selection.Interfaces;
using DETrainingDeepNN.Strategies.FitnessEvaluation.Interfaces;
using DETrainingDeepNN.Strategies.Crossover.Interfaces;
using DETrainingDeepNN.Strategies.Mutation.Interfaces;
using DETrainingDeepNN.Algorithms.Interfaces;
using NUnit.Framework;
using DETrainingDeepNN.ConfigurationSettings;

namespace Test_DETrainingDeepNN.Algorithms
{
    [TestFixture]
    public class DifferentialEvolutionTest
    {
        public Mock ISelection { get; private set; }
        
        [Test]
        public void GivenADifferentialEvolutionAlgorithm_WhenThePopulationIsInitialisedWithAPopulationSizeOfTen_ItShouldReturnAPopulationOfTenIndividuals()
        {
            DifferentialEvolution differentialEvolution = new DifferentialEvolution(null, null, null, null, null, new Configuration());

            List<Individual> population = differentialEvolution.InitialisePopulation(10);

            Assert.AreEqual(10, population.Count);
        }

        [Test]
        public void GivenAPopulationOfThreeAndAnInvalidIndividual_WhenTheGettingTheDifferenceIndividual_ItShouldCallTheSelectionStrategyWithAPopulationExcludingTheInvalidIndividual()
        {
            Individual individual1 = new Individual(null, new Configuration())
            {
                Position = new double[] { 1.0, 2.0, 3.0 }
            };

            Individual individual2 = new Individual(null, new Configuration())
            {
                Position = new double[] { 2.0, 3.0, 4.0 }
            };

            Individual individual3 = new Individual(null, new Configuration())
            {
                Position = new double[] { 5.0, 6.0, 7.0 }
            };

            List<Individual> population = new List<Individual>
            {
                individual1, individual2, individual3
            };

            List<Individual> invalidIndividuals = new List<Individual>
            {
                individual2
            };

            List<Individual> resultingPopulation = new List<Individual>();

            Mock<ISelectionStrategy> mock = new Mock<ISelectionStrategy>();
            mock.Setup(c => c.Select(It.IsAny<List<Individual>>()))
            .Callback((List<Individual> validPopulation) => resultingPopulation = validPopulation);

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(null, null, null, 
                mock.Object, null, new Configuration());
            
            differentialEvolution.SelectDifferenceIndividual(population, invalidIndividuals);

            Assert.AreEqual(2, resultingPopulation.Count);
            Assert.IsFalse(resultingPopulation.Contains(individual2));
            Assert.IsTrue(resultingPopulation.Contains(individual1));
            Assert.IsTrue(resultingPopulation.Contains(individual3));
        }

        [Test]
        public void GivenAPopulationOfThree_WhenANewPopulationIsRetrieved_ItShouldCallTheEvaluateMethodThreeTimes()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());
            
            differentialEvolution.population = new List<Individual>
            {
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration())
            };

            differentialEvolution.GetNewPopulation();

            fitnessEvaluationMock.Verify(c => c.GetFitnessForIndividual(It.IsAny<Individual>()), Times.Exactly(3 * 2));
        }

        [Test]
        public void GivenAPopulationOfOne_WhenANewPopulationIsRetrieved_ItShouldCallTheEvaluateMethodWithTheIndividualAsAParameter()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy, 
                                                                                    new Configuration());

            Individual individual = new Individual(fitnessEvaluationStrategy, new Configuration());
            
            differentialEvolution.population = new List<Individual>
            {
                individual
            };

            differentialEvolution.GetNewPopulation();

            fitnessEvaluationMock.Verify(c => c.GetFitnessForIndividual(It.Is<Individual>(p => p == individual)));
        }

        [Test]
        public void GivenAPopulationOfThree_WhenANewPopulationIsRetrieved_ItShouldCallTheMutationStrategyThreeTimes()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());
            differentialEvolution.population = new List<Individual>
            {
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration())
            };

            differentialEvolution.GetNewPopulation();

            mutationMock.Verify(c => c.GetTrialVector(It.IsAny<Individual>(), It.IsAny<List<Individual>>()), Times.Exactly(3));
        }

        [Test]
        public void GivenAPopulationOfFour_WhenANewPopulationIsRetrieved_ItShouldCallTheMutationStrategyWithThreeDifferentIndividuals()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Individual individual = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual target = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual difference1 = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual difference2 = new Individual(fitnessEvaluationStrategy, new Configuration());
            
            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            selectionMock.SetupSequence(x => x.Select(It.IsAny<List<Individual>>())).Returns(target)
                                                                            .Returns(difference1)
                                                                            .Returns(difference2);
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());

            
            differentialEvolution.population = new List<Individual>
            {
                individual, target, difference1, difference2
            };

            differentialEvolution.GetNewPopulation();

            mutationMock.Verify(c => c.GetTrialVector(target, new List<Individual>() { difference1, difference2 }), Times.Once());
        }

        [Test]
        public void GivenAPopulationOfThree_WhenANewPopulationIsRetrieved_ItShouldCallTheCrossoverStrategyThreeTimes()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy, 
                                                                                    crossoverStrategy, 
                                                                                    selectionStrategy, 
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());
            differentialEvolution.population = new List<Individual>
            {
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration())
            };

            differentialEvolution.GetNewPopulation();

            crossoverMock.Verify(c => c.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()), Times.Exactly(3));
        }

        [Test]
        public void GivenAPopulationOfOne_WhenANewPopulationIsRetrieved_ItShouldCallTheCrossoverStrategyWithTheIndividualAndTheTrialIndividual()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Individual individual = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual mutant = new Individual(fitnessEvaluationStrategy, new Configuration());

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            mutationMock.Setup(x => x.GetTrialVector(It.IsAny<Individual>(), It.IsAny<List<Individual>>()))
                .Returns(mutant);
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());

            

            differentialEvolution.population = new List<Individual>
            {
                individual
            };

            differentialEvolution.GetNewPopulation();

            crossoverMock.Verify(c => c.Cross(individual, mutant), Times.Once());
        }

        [Test]
        public void GivenAPopulationOfThree_WhenANewPopulationIsRetrieved_ItShouldCallTheSelectionStrategyThreeTimes()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;
            
            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            Mock<ISelectionStrategy> otherSelectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy otherSelectionStrategy = otherSelectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    otherSelectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());



            differentialEvolution.population = new List<Individual>
            {
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration()),
                new Individual(fitnessEvaluationStrategy, new Configuration())
            };

            differentialEvolution.GetNewPopulation();

            otherSelectionMock.Verify(c => c.Select(It.IsAny<List<Individual>>()), Times.Exactly(3));
        }

        [Test]
        public void GivenAPopulationOfOne_WhenANewPopulationIsRetrieved_ItShouldCallTheSelectionStrategyWithTheIndividualAndTheChild()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;
            
            Individual individual = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual child = new Individual(fitnessEvaluationStrategy, new Configuration());

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>())).Returns(child);
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;
            
            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            List<Individual> receivedIndividuals = new List<Individual>();
            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            selectionMock.Setup(x => x.Select(It.IsAny<List<Individual>>())).Callback((List<Individual> individualList) =>
                    receivedIndividuals = individualList
            );
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());



            differentialEvolution.population = new List<Individual>
            {
                individual
            };

            differentialEvolution.GetNewPopulation();

            Assert.AreEqual(2, receivedIndividuals.Count);
            Assert.IsTrue(receivedIndividuals.Contains(individual));
            Assert.IsTrue(receivedIndividuals.Contains(child));
        }

        [Test]
        public void GivenAPopulationOfTwo_WhenANewPopulationIsRetrieved_ItShouldReturnTheExpectedPopulationOfTwo()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Individual individual1 = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual child1 = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual individual2 = new Individual(fitnessEvaluationStrategy, new Configuration());
            Individual child2 = new Individual(fitnessEvaluationStrategy, new Configuration());

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;
            
            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            selectionMock.SetupSequence(x => x.Select(It.IsAny<List<Individual>>())).Returns(child1)
                                                                                    .Returns(individual2);
            ISelectionStrategy selectionStrategy = selectionMock.Object;

            Mock<ISelectionStrategy> otherSelectionMock = new Mock<ISelectionStrategy>();
            ISelectionStrategy otherSelectionStrategy = otherSelectionMock.Object;

            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    otherSelectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());



            differentialEvolution.population = new List<Individual>
            {
                individual1, individual2
            };
            
            List<Individual> newPopulation = differentialEvolution.GetNewPopulation();

            Assert.AreEqual(2, newPopulation.Count);
            Assert.IsTrue(newPopulation.Contains(child1));
            Assert.IsTrue(newPopulation.Contains(individual2));
            Assert.IsFalse(newPopulation.Contains(child2));
            Assert.IsFalse(newPopulation.Contains(individual1));
        }

        [Test]
        public void GivenADifferentialEvolutionAlgorithm_WhenTheAlgorithmIsRun_ItShouldResultInAPopulationWithAtLeastOneIndividual()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;
            
            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;
            
            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            selectionMock.Setup(x => x.Select(It.IsAny<List<Individual>>()))
                                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ISelectionStrategy selectionStrategy = selectionMock.Object;
            
            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy,
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());

            differentialEvolution.Run();
            
            Assert.IsTrue(differentialEvolution.population.Count > 0);
        }

        [Test]
        public void GivenFiveIterationsAndThreeIndividuals_WhenTheAlgorithmIsRun_ItShouldCallTheCrossoverStrategyFifteenTimes()
        {
            Mock<IFitnessEvaluationStrategy> fitnessEvaluationMock = new Mock<IFitnessEvaluationStrategy>();
            IFitnessEvaluationStrategy fitnessEvaluationStrategy = fitnessEvaluationMock.Object;

            Mock<ICrossoverStrategy> crossoverMock = new Mock<ICrossoverStrategy>();
            crossoverMock.Setup(x => x.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()))
                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ICrossoverStrategy crossoverStrategy = crossoverMock.Object;

            Mock<IMutationStrategy> mutationMock = new Mock<IMutationStrategy>();
            IMutationStrategy mutationStrategy = mutationMock.Object;

            Mock<ISelectionStrategy> selectionMock = new Mock<ISelectionStrategy>();
            selectionMock.Setup(x => x.Select(It.IsAny<List<Individual>>()))
                                .Returns(new Individual(fitnessEvaluationStrategy, new Configuration()));
            ISelectionStrategy selectionStrategy = selectionMock.Object;


            DifferentialEvolution differentialEvolution = new DifferentialEvolution(mutationStrategy,
                                                                                    crossoverStrategy,
                                                                                    selectionStrategy,
                                                                                    selectionStrategy, 
                                                                                    fitnessEvaluationStrategy,
                                                                                    new Configuration());

            differentialEvolution.Run();

            crossoverMock.Verify(c => c.Cross(It.IsAny<Individual>(), It.IsAny<Individual>()), Times.Exactly(15));
        }
        
    }
}
