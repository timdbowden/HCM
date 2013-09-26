using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hitachi.Coding.Model;
using System.Collections.Generic;

namespace Hitachi.Coding.Test
{
    [TestClass]
    public class TestModel
    {

        Port prtNY = new Port { Name = "New York" };
        Port prtLiverpool = new Port { Name = "Liverpool" };
        Port prtBA = new Port { Name = "Buenos Aires" };
        Port prtCT = new Port { Name = "Cape Town" };
        Port prtCasablanca = new Port { Name = "Casablanca" };
        
        /// <summary>
        /// Check nominated journey times
        /// </summary>
        [TestMethod]
        public void CheckJourneyTimes()
        {
            Graph<Port> g = SeedData();

            Assert.AreEqual(g.GetJourneyTime(prtBA, prtNY, prtLiverpool), 10.0);
            Assert.AreEqual(g.GetJourneyTime(prtBA, prtCasablanca, prtLiverpool), 8.0);
            Assert.AreEqual(g.GetJourneyTime(prtBA, prtCT, prtNY, prtLiverpool, prtCasablanca), 19.0);
        }


        /// <summary>
        /// Check invalid journey throw correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidJourneyException<Port>))]
        public void CheckInvalidJourneyTime()
        {
            Graph<Port> g = SeedData();
            g.GetJourneyTime(prtBA, prtCT, prtCasablanca);
        }

        
        /// <summary>
        /// Check correct fastest routes are returned
        /// </summary>
        [TestMethod]
        public void CheckFastestRoutes()
        {
            Graph<Port> g = SeedData();
            Port origin = prtBA;
            Port destination = prtLiverpool;
            

            Journey<Port> jrn = g.GetFastestJourney(origin, destination);

            Assert.AreEqual(jrn.JourneyTime, 8.0);
            Assert.IsTrue(jrn.Route.Peek().Equals(origin));
            Port[] prts = jrn.Route.ToArray();

            Assert.IsTrue(prts[prts.Length - 1].Equals(destination));

            origin = prtNY;
            destination = prtNY;

            jrn = g.GetFastestJourney(origin, destination);
            
            Assert.AreEqual(jrn.JourneyTime, 18.0);
            Assert.IsTrue(jrn.Route.Peek().Equals(origin));
            prts = jrn.Route.ToArray();

            Assert.IsTrue(prts[prts.Length - 1].Equals(destination));
        }

        /// <summary>
        /// Check journeys upto max stops
        /// </summary>
        [TestMethod]
        public void CheckJourneysUpToMaxStops()
        {
            int intMaxStops = 3;
            Port origin = prtLiverpool;
            Port destination = prtLiverpool;
            Graph<Port> g = SeedData();

            List<Journey<Port>> lstJourney = g.GetJourneyWithMaxStops(origin, destination, intMaxStops, true);

            foreach (Journey<Port> jrn in lstJourney)
            {
                Assert.IsTrue(jrn.Route.Count <= intMaxStops + 1);
                Assert.IsTrue(jrn.Route.Peek().Equals(origin));
                Port[] prts = jrn.Route.ToArray();

                Assert.IsTrue(prts[prts.Length - 1].Equals(destination));

            }


        }

        [TestMethod]
        public void CheckJourneysExactlyNoOfStops()
        {
            int intStops = 4;
            Port origin = prtBA;
            Port destination = prtLiverpool;
           
            Graph<Port> g = SeedData();

            List<Journey<Port>> lstJourney = g.GetJourneyWithMaxStops(origin, destination, 4, true);

            foreach (Journey<Port> jrn in lstJourney)
            {
                Assert.AreEqual(jrn.Route.Count, intStops + 1);
                Assert.IsTrue(jrn.Route.Peek().Equals(origin));
                Port[] prts = jrn.Route.ToArray();
                Assert.IsTrue(prts[prts.Length - 1].Equals(destination));
            }
        }

        [TestMethod]
        public void CheckJourneysUpToTime()
        {
            double dblMaxTime = 25.0;
            Port origin = prtLiverpool;
            Port destination = prtLiverpool;
            
            Graph<Port> g = SeedData();


            List<Journey<Port>> lstJourney = g.GetJourneyWithMaxTime(origin, destination, dblMaxTime, false, true);

            foreach (Journey<Port> jrn in lstJourney)
            {
                Assert.IsTrue(jrn.JourneyTime <= dblMaxTime);
                Assert.IsTrue(jrn.Route.Peek().Equals(origin));
                Port[] prts = jrn.Route.ToArray();
                Assert.IsTrue(prts[prts.Length - 1].Equals(destination));
            }
        }




        #region Seed Data

        private Graph<Port> SeedData()
        {
            Graph<Port> graThis = new Graph<Port>();

            graThis.Nodes = new List<Node<Port>>();

            Node<Port> nodThis = null;

            nodThis = graThis.AddNode(prtNY);
            nodThis.AddEdge(prtLiverpool, 4.0);

            nodThis = graThis.AddNode(prtLiverpool);
            nodThis.AddEdge(prtCasablanca, 3.0);
            nodThis.AddEdge(prtCT, 6.0);

            nodThis = graThis.AddNode(prtBA);
            nodThis.AddEdge(prtNY, 6.0);
            nodThis.AddEdge(prtCasablanca, 5.0);
            nodThis.AddEdge(prtCT, 4.0);

            nodThis = graThis.AddNode(prtCT);
            nodThis.AddEdge(prtNY, 8.0);

            nodThis = graThis.AddNode(prtCasablanca);
            nodThis.AddEdge(prtLiverpool, 3.0);
            nodThis.AddEdge(prtCT, 6.0);

            return graThis;
        }


        #endregion


    }
}
