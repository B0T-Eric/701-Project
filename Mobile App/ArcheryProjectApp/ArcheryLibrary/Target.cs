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
        public Image FaceImage { get; set; }
        public Target() { }
        public Target(List<int> zoneValues, string face, Image faceImage)
        {
            ZoneValues = zoneValues;
            Face = face;
            FaceImage = faceImage;
        }
        public override string ToString()
        {
            return Face;
        }
    }
}
