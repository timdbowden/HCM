using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.Spatial;

namespace Hitachi.Coding.Model
{
    /// <summary>
    /// Class to represent a port
    /// </summary>
    public class Port
    {
        #region Properties

        public string Name { get; set; }
        public DbGeography Location { get; set; }
        public Uri WikipediaEntry { get; set; }


        #endregion

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}
