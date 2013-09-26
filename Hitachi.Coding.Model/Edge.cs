using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitachi.Coding.Model
{
    /// <summary>
    /// An Edge connecting two nodes in a graph with a specific weight
    /// </summary>
    /// <typeparam name="T">Type of objects connected by the edge</typeparam>
    public class Edge<T>
    {

        #region Properties

        /// <summary>
        /// Weight of the journey/edge
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Origin node
        /// </summary>
        public T Origin { get; set; }

        /// <summary>
        /// Destination node
        /// </summary>
        public T Destination { get; set; }

        #endregion

        #region Constructors

        public Edge(T origin, T destination, double weight)
        {
            Origin = origin;
            Destination = destination;
            Weight = weight;

        }

        #endregion

    }
}
