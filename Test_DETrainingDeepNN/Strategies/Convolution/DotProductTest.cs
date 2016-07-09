﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DETrainingDeepNN;
using DETrainingDeepNN.Strategies.Crossover;
using System.Linq;
using DETrainingDeepNN.Strategies.Convolution;
using DETrainingDeepNN.Models;
using DETrainingDeepNN.Mappers;
using Moq;
using DETrainingDeepNN.Strategies.Exceptions;

namespace Test_DETrainingDeepNN.Strategies.Crossover
{
    [TestClass]
    public class DotProductTest
    {
        [TestMethod]
        public void GivenAnIndividualAndASubsectionOfAnImageInput_WhenTheTwoAreConvolutedAndHaveTwoDimensions_ItShouldReturnTheDotProduct()
        {
            double[] position = new double[] { 6.0, 2.0 };

            double[] subSection = { 1, 5 };

            DotProductStrategy convolutionStrategy = new DotProductStrategy();

            double result = convolutionStrategy.Convolute(subSection, position);

            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void GivenAnIndividualAndASubsectionOfAnImageInput_WhenTheTwoAreConvolutedAndHaveThreeDimensions_ItShouldReturnTheDotProduct()
        {
            double[] position = new double[] { 6.0, 2.0, 3.0 };

            double[] subSection = { 1, 5, 7 };

            DotProductStrategy convolutionStrategy = new DotProductStrategy();

            double result = convolutionStrategy.Convolute(subSection, position);

            Assert.AreEqual(37, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DifferingConvolutionSizeException))]
        public void GivenAnIndividualAndASubsectionOfAnImageInput_WhenTheTwoAreConvolutedAndTheIndividualIsBigger_ItShouldThrowAnDifferingConvolutionSizeExceptionError()
        {
            double[] position = new double[] { 6.0, 2.0, 3.0 };

            double[] subSection = { 1, 5 };

            DotProductStrategy convolutionStrategy = new DotProductStrategy();

            convolutionStrategy.Convolute(subSection, position);
        }

        [TestMethod]
        [ExpectedException(typeof(DifferingConvolutionSizeException))]
        public void GivenAnIndividualAndASubsectionOfAnImageInput_WhenTheTwoAreConvolutedAndTheSubsectionIsBigger_ItShouldThrowAnDifferingConvolutionSizeExceptionError()
        {
            double[] position = new double[] { 6.0, 2.0 };

            double[] subSection = { 1, 5, 3 };

            DotProductStrategy convolutionStrategy = new DotProductStrategy();

            convolutionStrategy.Convolute(subSection, position);
        }
    }
}
