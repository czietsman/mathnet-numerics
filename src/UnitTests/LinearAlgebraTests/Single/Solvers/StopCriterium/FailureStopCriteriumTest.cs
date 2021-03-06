// <copyright file="FailureStopCriteriumTest.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Single.Solvers.StopCriterium
{
    using System;
    using LinearAlgebra.Generic.Solvers.Status;
    using LinearAlgebra.Single;
    using LinearAlgebra.Single.Solvers.StopCriterium;
    using NUnit.Framework;

    /// <summary>
    /// Failure stop criterium tests.
    /// </summary>
    [TestFixture]
    public sealed class FailureStopCriteriumTest
    {
        /// <summary>
        /// Can create.
        /// </summary>
        [Test]
        public void Create()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "Should have a criterium now");
        }

        /// <summary>
        /// Determine status with illegal iteration number throws <c>ArgumentOutOfRangeException</c>.
        /// </summary>
        [Test]
        public void DetermineStatusWithIllegalIterationNumberThrowsArgumentOutOfRangeException()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            Assert.Throws<ArgumentOutOfRangeException>(() => criterium.DetermineStatus(-1, new DenseVector(3, 4), new DenseVector(3, 5), new DenseVector(3, 6)));
        }

        /// <summary>
        /// Determine status with <c>null</c> solution vector throws <c>ArgumentNullException</c>.
        /// </summary>
        [Test]
        public void DetermineStatusWithNullSolutionVectorThrowsArgumentNullException()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            Assert.Throws<ArgumentNullException>(() => criterium.DetermineStatus(1, null, new DenseVector(3, 6), new DenseVector(4, 4)));
        }

        /// <summary>
        /// Determine status with <c>null</c> residual vector throws <c>ArgumentNullException</c>.
        /// </summary>
        [Test]
        public void DetermineStatusWithNullResidualVectorThrowsArgumentNullException()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            Assert.Throws<ArgumentNullException>(() => criterium.DetermineStatus(1, new DenseVector(3, 4), new DenseVector(3, 6), null));
        }

        /// <summary>
        /// Determine status with non-matching vectors throws <c>ArgumentException</c>.
        /// </summary>
        [Test]
        public void DetermineStatusWithNonMatchingVectorsThrowsArgumentException()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            Assert.Throws<ArgumentException>(() => criterium.DetermineStatus(1, new DenseVector(3, 4), new DenseVector(3, 6), new DenseVector(4, 4)));
        }

        /// <summary>
        /// Can determine status with residual NaN.
        /// </summary>
        [Test]
        public void DetermineStatusWithResidualNaN()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            var solution = new DenseVector(new[] { 1.0f, 1.0f, 2.0f });
            var source = new DenseVector(new[] { 1001.0f, 0, 2003.0f });
            var residual = new DenseVector(new[] { 1000, float.NaN, 2001 });

            criterium.DetermineStatus(5, solution, source, residual);
            Assert.IsInstanceOf(typeof(CalculationFailure), criterium.Status, "Should be failed");
        }

        /// <summary>
        /// Can determine status with solution NaN.
        /// </summary>
        [Test]
        public void DetermineStatusWithSolutionNaN()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            var solution = new DenseVector(new[] { 1, 1, float.NaN });
            var source = new DenseVector(new[] { 1001.0f, 0.0f, 2003.0f });
            var residual = new DenseVector(new[] { 1000.0f, 1000.0f, 2001.0f });

            criterium.DetermineStatus(5, solution, source, residual);
            Assert.IsInstanceOf(typeof(CalculationFailure), criterium.Status, "Should be failed");
        }

        /// <summary>
        /// Can determine status.
        /// </summary>
        [Test]
        public void DetermineStatus()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            var solution = new DenseVector(new[] { 3.0f, 2.0f, 1.0f });
            var source = new DenseVector(new[] { 1001.0f, 0.0f, 2003.0f });
            var residual = new DenseVector(new[] { 1.0f, 2.0f, 3.0f });

            criterium.DetermineStatus(5, solution, source, residual);
            Assert.IsInstanceOf(typeof(CalculationRunning), criterium.Status, "Should be running");
        }

        /// <summary>
        /// Can reset calculation state.
        /// </summary>
        [Test]
        public void ResetCalculationState()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            var solution = new DenseVector(new[] { 1.0f, 1.0f, 2.0f });
            var source = new DenseVector(new[] { 1001.0f, 0.0f, 2003.0f });
            var residual = new DenseVector(new[] { 1000.0f, 1000.0f, 2001.0f });

            criterium.DetermineStatus(5, solution, source, residual);
            Assert.IsInstanceOf(typeof(CalculationRunning), criterium.Status, "Should be running");

            criterium.ResetToPrecalculationState();
            Assert.IsInstanceOf(typeof(CalculationIndetermined), criterium.Status, "Should not have started");
        }

        /// <summary>
        /// Can clone stop criterium.
        /// </summary>
        [Test]
        public void Clone()
        {
            var criterium = new FailureStopCriterium();
            Assert.IsNotNull(criterium, "There should be a criterium");

            var clone = criterium.Clone();
            Assert.IsInstanceOf(typeof(FailureStopCriterium), clone, "Wrong criterium type");
        }
    }
}
