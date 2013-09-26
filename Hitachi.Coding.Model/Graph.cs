using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitachi.Coding.Model
{
    /// <summary>
    /// Generically typed graph data structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T>
    {

        #region Private Variables

        /// <summary>
        /// Private list of nodes
        /// </summary>
        private List<Node<T>> graphNodes;

        /// <summary>
        /// List of all journeys that match a set of criteria
        /// </summary>
        private List<Journey<T>> mlstMatchedJourneys;
       
        #endregion

        #region Properties

        /// <summary>
        /// All the nodes of type T in the graph
        /// </summary>
        public List<Node<T>> Nodes 
        { 
            get {return graphNodes;} 
            set {graphNodes = value;} 
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Add a node of Type T to the graph
        /// </summary>
        /// <param name="item">Instance of the type T object</param>
        /// <returns>Reurns the added node</returns>
        public Node<T> AddNode(T item)
        {

            Node<T> node = new Node<T>(item);                 
            graphNodes.Add(node);
            return node; 
        }
         
     
        /// <summary>
        /// Gets the length of time of a journey
        /// </summary>
        /// <param name="origin">Start of the journey</param>
        /// <param name="nodes">Waypoints on the journey</param>
        /// <returns>Length of the journey in days</returns>
        public double GetJourneyTime(T origin, params T[] nodes)
        {
            double dblReturn = 0;
            Node<T> nodStart = GetNodeFromData(origin);

            foreach (T item in nodes)
            {
                dblReturn += nodStart.GetJourneyTime(item);
                nodStart = GetNodeFromData(item);
            }   

            return dblReturn;

        }

        /// <summary>
        /// Returns the node associated with the instance of the object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Node<T> GetNodeFromData(T data)
        {
            foreach (Node<T> item in Nodes)
            {
                if (item.Data.Equals(data))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a journey with a maximum number of stops
        /// </summary>
        /// <param name="origin">Start of the journey</param>
        /// <param name="destination">End of the journey</param>
        /// <param name="maxStops">Maximum number of stops</param>
        /// <param name="isExact">Does it have to be exactly that number of stop, or does fewer also count</param>
        /// <returns></returns>
        public List<Journey<T>> GetJourneyWithMaxStops(T origin, T destination, int maxStops, bool isExact)
        {
            mlstMatchedJourneys = new List<Journey<T>>();

            BuildJourneys(new Journey<T>(), origin, destination, maxStops, isExact);

            return mlstMatchedJourneys;
        }

        /// <summary>
        /// Get journeys up to a mixumum amount of time
        /// </summary>
        /// <param name="origin">Start of the journey</param>
        /// <param name="destination">End of the journey</param>
        /// <param name="maxTime">Maximum amount of time</param>
        /// <param name="isExact">Does it have to be that amount of days</param>
        /// <param name="includeCircular">Include journeys that may loop</param>
        /// <returns>List </returns>
        public List<Journey<T>> GetJourneyWithMaxTime(T origin, T destination, double maxTime, bool isExact, bool includeCircular)
        {
            mlstMatchedJourneys = new List<Journey<T>>();

            BuildJourneys(new Journey<T>(), origin, destination, maxTime, isExact, includeCircular);

            return mlstMatchedJourneys;
        }

        /// <summary>
        /// Get the quickest journey between two ports (NB this cannot contain a loop)
        /// </summary>
        /// <param name="origin">Start of the journey</param>
        /// <param name="destination">End of the journey</param>
        /// <returns>Return the fastest journey</returns>
        public Journey<T> GetFastestJourney(T origin, T destination)
        {
            mlstMatchedJourneys = new List<Journey<T>>();

            BuildJourneys(new Journey<T>(), origin, destination);

            mlstMatchedJourneys.Sort();

            return mlstMatchedJourneys.FirstOrDefault();
        }

        #endregion

        #region Constructor


        #endregion

        #region Overrides
        /// <summary>
        /// Overriden ToString method
        /// </summary>
        /// <returns>All the nodes and edges</returns>
        public override string ToString()
        {

            string strReturn = "";
            
            foreach (var item in Nodes)
            {
                strReturn += item.Data.ToString() + Environment.NewLine;

                foreach (var edge in item.Edges)
                {
                    strReturn += "-->" + edge.Destination.ToString() + " (" + edge.Weight.ToString() + ")" + Environment.NewLine;
                }

                strReturn += "--------------------------------------" + Environment.NewLine;
            }
            
            return strReturn;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add to journeys between a current point and all it's neighbours unless it's the destination
        /// </summary>
        /// <param name="currentJourney">The current journey</param>
        /// <param name="currentStop">Where we are</param>
        /// <param name="destination">Where we want to be</param>
        private void BuildJourneys(Journey<T> currentJourney, T currentStop, T destination)
        {
            bool blnContinue = true;

            if (currentJourney.Route.Count > 0)
            {
                //does the journey meet the criteria
                if (currentStop.Equals(destination))
                {
                    currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                    currentJourney.Route.Enqueue(currentStop);
                    mlstMatchedJourneys.Add(currentJourney);
                    blnContinue = false;

                }
                //See if there is a loop in which case not the fastest route
                else if (currentJourney.Route.Contains(currentStop))
                {

                    var prt = from p in currentJourney.Route
                              where p.Equals(currentStop)
                              select p;

                    //We allow origin and destination to be the same
                    if (currentJourney.Route.Peek().Equals(currentStop))
                    {
                        blnContinue = (prt.Count() == 1);
                    }
                    else
                    {
                        blnContinue = (prt.Count() == 0);
                    }
                } 
            }

            if (blnContinue)
            {
                if (currentJourney.Route.Count == 0)
                {
                    currentJourney.JourneyTime = 0;
                }
                else
                {
                    currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                }

                currentJourney.Route.Enqueue(currentStop);

                Node<T> currNode = GetNodeFromData(currentStop);

                foreach (T stop in currNode.Neighbours)
                {
                    BuildJourneys(currentJourney.Clone(), stop, destination);
                }

            }

        }

        /// <summary>
        /// Build journeys upto max stops
        /// </summary>
        /// <param name="currentJourney"></param>
        /// <param name="currentStop"></param>
        /// <param name="destination"></param>
        /// <param name="intMaxStops"></param>
        /// <param name="blnExact"></param>
        private void BuildJourneys(Journey<T> currentJourney, T currentStop, T destination, int intMaxStops, bool blnExact)
        {
            bool blnContinue = true;
            
            //does the journey meet the criteria
            if (blnExact &&(currentStop.Equals(destination) && currentJourney.Route.Count == intMaxStops && currentJourney.Route.Count > 0))
            {
                currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                currentJourney.Route.Enqueue(currentStop);
                mlstMatchedJourneys.Add(currentJourney);
                blnContinue = false;

            }
            if (!blnExact && (currentStop.Equals(destination) && currentJourney.Route.Count <= intMaxStops && currentJourney.Route.Count > 0))
	        {
                currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                currentJourney.Route.Enqueue(currentStop);
                mlstMatchedJourneys.Add(currentJourney);
                blnContinue = false;

	        }
            else if (currentJourney.Route.Count == intMaxStops)
	        {
                blnContinue = false; 		 
	        }

            if (blnContinue)
	        {
                if (currentJourney.Route.Count == 0)
                {
                    currentJourney.JourneyTime = 0;
                }
                else
                {
                    currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                }
                
                currentJourney.Route.Enqueue(currentStop);

                Node<T> currNode =  GetNodeFromData(currentStop);

                foreach (T stop in currNode.Neighbours)
	            {
		            BuildJourneys(currentJourney.Clone(), stop, destination, intMaxStops, blnExact);
	            }

	        }      

        }

        /// <summary>
        /// Build journeys up to max time
        /// </summary>
        /// <param name="currentJourney"></param>
        /// <param name="currentStop"></param>
        /// <param name="destination"></param>
        /// <param name="maxTime"></param>
        /// <param name="isExact"></param>
        /// <param name="blnIncludeCircular"></param>
        private void BuildJourneys(Journey<T> currentJourney, T currentStop, T destination, double maxTime, bool isExact, bool blnIncludeCircular)
        {
            bool blnContinue = true;

           

            if (currentJourney.Route.Count == 0)
            {
                currentJourney.JourneyTime = 0;
                currentJourney.Route.Enqueue(currentStop);

            } else
            {
                currentJourney.JourneyTime += GetJourneyTime(currentJourney.Route.Last(), currentStop);
                currentJourney.Route.Enqueue(currentStop);

                
                //does the journey meet the criteria
                if (isExact && (currentStop.Equals(destination) && currentJourney.JourneyTime == maxTime))
                {
                    mlstMatchedJourneys.Add(currentJourney);
                    blnContinue = blnIncludeCircular;

                }
                if (!isExact && (currentStop.Equals(destination) && currentJourney.JourneyTime <= maxTime))
                {
                    mlstMatchedJourneys.Add(currentJourney);
                    blnContinue = blnIncludeCircular;

                }
                else if (currentJourney.JourneyTime > maxTime)
                {
                    blnContinue = false;
                } 
            }

            if (blnContinue)
            {

                Node<T> currNode = GetNodeFromData(currentStop);

                foreach (T stop in currNode.Neighbours)
                {
                    BuildJourneys(currentJourney.Clone(), stop, destination, maxTime, isExact, blnIncludeCircular);
                }

            }

        }

        #endregion
    }


    public class InvalidJourneyException<T> : Exception
    {
         private string mstrMessage;
        /// <summary>
        /// Error message
        /// </summary>
        public override string Message
        {
            get { return mstrMessage; }            
        }

        /// <summary>
        /// Origin object
        /// </summary>
        public T Origin { get; set; }
        /// <summary>
        /// Destination object
        /// </summary>
        public T Destination { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="origin">Start node</param>
        /// <param name="destination">End node</param>
        public InvalidJourneyException(string message, T origin, T destination)
        {
            mstrMessage = message;

            Origin = origin;
            Destination = destination;
        }
    }
}
