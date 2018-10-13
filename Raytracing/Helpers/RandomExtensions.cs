using System;
using System.Numerics;

namespace Raytracing.Helpers {
    static class RandomExtensions {

        /// <summary>
        /// Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently they canbe cached or something for better performance, but who cares.
        /// From https://bitbucket.org/Superbest/superbest-random/src/f067e1dc014c31be62c5280ee16544381e04e303/Superbest%20random/RandomExtensions.cs?at=master&fileviewer=file-view-default
        /// </summary>
        /// <param name="r"></param>
        /// <param name = "mu">Mean of the distribution</param>
        /// <param name = "sigma">Standard deviation</param>
        /// <returns></returns>
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1) {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                Math.Sin(2.0 * Math.PI * u2);

            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        /// <summary>
        /// Generates a vector with three random components greater than or equal to 0.0 and less than 1.0.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Vector3 NextVector3(this Random r) {
            return new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
        }
    }
}
