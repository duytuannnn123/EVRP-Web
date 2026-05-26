
using EVRP_Web.Models;

namespace EVRP_Web.Services
{
    public class DistanceMatrixService
    {
        private readonly double[,] _matrix;

        public DistanceMatrixService(List<Node> nodes)
        {
            int count = nodes.Count;
            _matrix = new double[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    _matrix[i, j] = CalculateDistance(nodes[i], nodes[j]);
                }
            }
        }

        private double CalculateDistance(Node a, Node b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double GetDistance(int from, int to)
        {
            return _matrix[from, to];
        }
    }
}
