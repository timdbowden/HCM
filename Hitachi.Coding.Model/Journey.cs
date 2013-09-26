using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitachi.Coding.Model
{
    /// <summary>
    /// A wrapper object containing queue of type T of a journey, plus the total time
    /// </summary>
    /// <typeparam name="T">Type of stops on the journey</typeparam>
    public class Journey<T> : IComparable<Journey<T>>
    {

        #region Private Variables
        
        private Queue<T> mqueRoute = new Queue<T>();

        #endregion

        /// <summary>
        /// The queue of waypoints of type T on teh route
        /// </summary>
        public Queue<T> Route
        {
            get { return mqueRoute; }
            set { mqueRoute = value; }
        }

        /// <summary>
        /// The time for the journey
        /// </summary>
        public double JourneyTime { get; set; }


        /// <summary>
        /// Clone the current journey
        /// </summary>
        /// <returns>Clone of this journey</returns>
        public Journey<T> Clone()
        {
            Journey<T> jrnClone = new Journey<T>();

            T[] stops = mqueRoute.ToArray();            

            foreach (T stop in stops)
	        {
                jrnClone.Route.Enqueue(stop);
	        }

            jrnClone.JourneyTime = JourneyTime;

            return jrnClone;
        } 
      
        /// <summary>
        /// Implements IComparaable by comaring journey time
        /// </summary>
        /// <param name="b">Object to compare</param>
        /// <returns></returns>
        public int CompareTo(Journey<T> b)
        {
            return this.JourneyTime.CompareTo(b.JourneyTime);
        }
        
    }
}
