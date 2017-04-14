using System;

namespace Microsoft.Expression.Drawing.Core
{
	internal class RandomEngine
	{
		private Random random;

		private double? anotherSample;

		public RandomEngine(long seed)
		{
			this.Initialize(seed);
		}

		private double Gaussian()
		{
			double num;
			double num1;
			double num2;
			if (this.anotherSample.HasValue)
			{
				double value = this.anotherSample.Value;
				this.anotherSample = null;
				return value;
			}
			do
			{
				num1 = 2 * this.Uniform() - 1;
				num2 = 2 * this.Uniform() - 1;
				num = num1 * num1 + num2 * num2;
			}
			while (num >= 1);
			double num3 = Math.Sqrt(-2 * Math.Log(num) / num);
			this.anotherSample = new double?(num1 * num3);
			return num2 * num3;
		}

		private void Initialize(long seed)
		{
			this.random = new Random((int)seed);
		}

		public double NextGaussian(double mean, double variance)
		{
			return this.Gaussian() * variance + mean;
		}

		public double NextUniform(double min, double max)
		{
			return this.Uniform() * (max - min) + min;
		}

		private double Uniform()
		{
			return this.random.NextDouble();
		}
	}
}