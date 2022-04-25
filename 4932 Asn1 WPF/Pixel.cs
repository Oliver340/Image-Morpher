using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4932_Asn1_WPF
{
    class Pixel
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;
        public Pixel(byte B, byte G, byte R, byte A)
        {
            this.B = B;
            this.G = G;
            this.R = R;
            this.A = A;
        }
    }
}
