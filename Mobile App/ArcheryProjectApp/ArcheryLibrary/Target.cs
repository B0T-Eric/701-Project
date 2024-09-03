using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryLibrary
{
    public class Target
    {
        public List<int> ZoneValues { get; set; }
        public string Face { get; set; }
        public string FaceImageURL { get; set; }
        public Target() { }
        public Target(List<int> zoneValues, string face, string faceImageURL)
        {
            ZoneValues = zoneValues;
            Face = face;
            FaceImageURL = faceImageURL;
        }
        public override string ToString()
        {
            return Face;
        }
    }
}
