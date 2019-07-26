//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Valve.VR;

#endregion

using System.Xml.Serialization;

namespace shared_data_layer.EquippmentData
{

    public class RacketsData
    {

        [XmlElement("Racket")]
        public List<Racket> Rackets { get; set; }

        public RacketsData()
        {
            Rackets = new List<Racket>();
        }

        /// <summary>
        /// Methods for finding the Racket definition class given the Racket id or a UserRacket object.
        /// </summary>

        public Racket FindRacket(UserRacket userRacket)
        {
            if (userRacket != null)
            {
                return (FindRacket(userRacket.RacketId));
            }
            else
            {
                return (null);
            }
        }

        public Racket FindRacket(string racketId)
        {
            return (Rackets.Find(item => item.RacketId == racketId));
        }

       
        public override string ToString()
        {
            StringBuilder xmlAsString = new StringBuilder();
            if (Rackets != null)
            {

                xmlAsString.AppendLine("Rackets (");
                xmlAsString.Append(Rackets.Count);
                xmlAsString.Append("):");

                foreach (Racket racket in Rackets)
                {
                    xmlAsString.AppendLine("---\n");
                    xmlAsString.Append(racket.ToString());
                }
            }
            else
            {
                xmlAsString.AppendLine("Rackets (0)");
            }
            return xmlAsString.ToString();
        }
    }
}
