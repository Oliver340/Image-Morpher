using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Numerics;

namespace _4932_Asn1_WPF
{
    internal class PairedLines
    {
        public Line? leftLine;
        public Line? rightLine;

        private Vector2 P;
        private Vector2 Q;
        private Vector2 pPrime;
        private Vector2 qPrime;
        private Vector2 PQ;
        private Vector2 PQPrime;
        private Vector2 n;
        private Vector2 nPrime;
        private Vector2 T;
        private Vector2 tPrime;
        private Vector2 TP;
        private Vector2 PT;
        private double d;
        private double b;


        public PairedLines()
        {
        }
        public PairedLines(Line left, Line right)
        {
            this.leftLine = left;
            this.rightLine = right;
        }
        public void setLeft(Line left)
        {
            this.leftLine = left;
        }
        public void setRight(Line right)
        {
            this.rightLine= right;
        }

        private Vector2 calcVector(Vector2 start, Vector2 end)
        {
            Vector2 result = new Vector2();
            result.X = end.X - start.X;
            result.Y = end.Y - start.Y;
            return result;
        }

        private double dotProduct(Vector2 first, Vector2 second)
        {
            return first.X * second.X + first.Y * second.Y;
        }

        private double magnitude(Vector2 p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        private double projection(Vector2 lower, Vector2 upper)
        {
            return dotProduct(lower, upper) / magnitude(lower);
        }


        
        public Vector2 calcXPrime(Vector2 T)
        {
            this.T = T;

            P = new Vector2((float)rightLine.X1, (float)rightLine.Y1);
            Q = new Vector2((float)rightLine.X2, (float)rightLine.Y2);
            PQ = calcVector(P, Q);
            n = new Vector2(PQ.Y, PQ.X * -1);
            TP = calcVector(T, P);
            d = projection(n, TP);
            PT = calcVector(P, T);
            b = projection(PQ, PT) / magnitude(PQ);
            pPrime = new Vector2((float)leftLine.X1, (float)leftLine.Y1);
            qPrime = new Vector2((float)leftLine.X2, (float)leftLine.Y2);
            PQPrime = calcVector(pPrime, qPrime);
            nPrime = new Vector2(PQPrime.Y, PQPrime.X * -1);
            double x = pPrime.X + b * PQPrime.X - d * nPrime.X / magnitude(nPrime);
            double y = pPrime.Y + b * PQPrime.Y - d * nPrime.Y / magnitude(nPrime);
            tPrime = new Vector2((float)x, (float)y);
            return tPrime;
        }

        public double calcWeight()
        {
            double a = 0.01;
            double p = 0;
            double b_ = 2;
            double w = Math.Pow(Math.Abs(Math.Pow(magnitude(PQPrime), p) / (a + Math.Abs(d))), b_);
            return w;
        }

        public Vector2 calcDeltaX()
        {
            Vector2 deltaX = new Vector2(tPrime.X - T.X, tPrime.Y - T.Y);
            return deltaX;
        }
    }
}
