﻿using DETrainingDeepNN;
using DETrainingDeepNN.ConfigurationSettings;
using DETrainingDeepNN.Calculators;
using DETrainingDeepNN.Models;
using DETrainingDeepNN.Strategies.ConvolutionStrategies;
using DETrainingDeepNN.Strategies.NeuronActivation;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace Test_DETrainingDeepNN.Strategies.NeuronActivation
{
    [TestFixture]
    public class FeatureMapNeuronActivationStrategyTest
    {
        private IDotProductConvolutionStrategy MockDotProduct()
        {
            Mock<IDotProductConvolutionStrategy> mock = new Mock<IDotProductConvolutionStrategy>();
            mock.SetupSequence(x => x.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
                                                .Returns(0.3).Returns(2.7)
                                                .Returns(6.2).Returns(1.9)
                                                .Returns(3.4).Returns(0.8)
                                                .Returns(1.7).Returns(3.5)
                                                .Returns(4.8).Returns(0.1);
            return mock.Object;
        }

        [Test]
        public void GivenAnInputSubsectionAndAFilter_WhenAFeatureMapPartIsCalculated_ItShouldCallTheDotProductConvoluteFunction()
        {
            double[,] inputSubsection = new double[3, 3];
            inputSubsection[0, 0] = 0;
            inputSubsection[0, 1] = 1;
            inputSubsection[1, 0] = 2;
            inputSubsection[1, 1] = 4;

            double[] filter = { 0.1, 0.2 };

            ImageInput input = new ImageInput(new TwoDimensionalMapper(), new Configuration());
            input.SetNumericRepresentation(inputSubsection);

            Mock<IDotProductConvolutionStrategy> dotProductMock = new Mock<IDotProductConvolutionStrategy>();
            dotProductMock.SetupSequence(x => x.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()))
                                                .Returns(0.3).Returns(2.7)
                                                .Returns(6.2).Returns(1.9)
                                                .Returns(3.4).Returns(0.8);
            
            FeatureMapNeuronActivationStrategy strategy = new FeatureMapNeuronActivationStrategy(dotProductMock.Object, new Configuration());

            double part = strategy.ConvolveSubSection(input.GetNumericRepresentation(), filter);

            dotProductMock.Verify(c => c.Calculate(It.IsAny<double[]>(), It.IsAny<double[]>()), Times.Exactly(1));
        }

        [Test]
        public void GivenAnInputSubsectionAndAFilter_WhenAFeatureMapPartIsCalculated_ItShouldReturnTheDotProduct()
        {
            double[,] inputSubsection = new double[3, 3];
            inputSubsection[0, 0] = 0;
            inputSubsection[0, 1] = 1;
            inputSubsection[1, 0] = 2;
            inputSubsection[1, 1] = 4;

            double[] filter = { 0.1, 0.2 };

            ImageInput input = new ImageInput(new TwoDimensionalMapper(), new Configuration());
            input.SetNumericRepresentation(inputSubsection);

            FeatureMapNeuronActivationStrategy strategy = new FeatureMapNeuronActivationStrategy(MockDotProduct(), new Configuration());

            double part = strategy.ConvolveSubSection(input.GetNumericRepresentation(), filter);

            Assert.AreEqual(0.3, part);
        }
        
        [Test]
        public void GivenAFourByFourInputAndATwoByTwoFilter_WhenTheNeuronIsActivated_ItShouldReturnAFeatureMap()
        {
            FeatureMapNeuronActivationStrategy strategy = new FeatureMapNeuronActivationStrategy(MockDotProduct(), new Configuration());

            double[,] matrix = new double[3, 3];
            matrix[0, 0] = 0;
            matrix[0, 1] = 1;
            matrix[0, 2] = 2;
            matrix[1, 0] = 3;
            matrix[1, 1] = 4;
            matrix[1, 2] = 5;
            matrix[2, 0] = 6;
            matrix[2, 1] = 7;
            matrix[2, 2] = 8;
            
            ImageInput input = new ImageInput(new TwoDimensionalMapper(), new Configuration());
            input.SetNumericRepresentation(matrix);

            double[] mockFilter = new double[4];

            double[] featureMap = strategy.Activate(input, mockFilter);
            double[] expected = { 0.3, 2.7, 6.2, 1.9 };

            Assert.IsTrue(Enumerable.SequenceEqual(expected, featureMap));
        }

        [Test]
        public void GivenAThreeByThreeInputAndATwoByTwoFilter_WhenTheNeuronIsActivated_ItShouldReturnAFeatureMap()
        {
            FeatureMapNeuronActivationStrategy strategy = new FeatureMapNeuronActivationStrategy(MockDotProduct(), new Configuration());

            double[,] matrix = new double[4, 4];
            matrix[0, 0] = 0;
            matrix[0, 1] = 1;
            matrix[0, 2] = 2;
            matrix[0, 3] = 3;
            matrix[1, 0] = 4;
            matrix[1, 1] = 5;
            matrix[1, 2] = 6;
            matrix[1, 3] = 7;
            matrix[2, 0] = 8;
            matrix[2, 1] = 9;
            matrix[2, 2] = 10;
            matrix[2, 3] = 11;
            matrix[3, 0] = 12;
            matrix[3, 1] = 13;
            matrix[3, 2] = 14;
            matrix[3, 3] = 15;

            ImageInput input = new ImageInput(new TwoDimensionalMapper(), new Configuration());
            input.SetNumericRepresentation(matrix);

            double[] mockFilter = new double[4];

            double[] featureMap = strategy.Activate(input, mockFilter);
            double[] expected = { 0.3, 2.7, 6.2, 1.9, 3.4, 0.8, 1.7, 3.5, 4.8 };

            Assert.IsTrue(Enumerable.SequenceEqual(expected, featureMap));
        }
    }
}
