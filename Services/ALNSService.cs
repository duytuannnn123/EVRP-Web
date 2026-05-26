
using EVRP_Web.Data;
using EVRP_Web.Models;
using EVRP_Web.Options;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Services
{
    public class ALNSService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ALNSService> _logger;
        private readonly ALNSOptions _options;

        private DistanceMatrixService? _distanceMatrix;

        public ALNSService(
            ApplicationDbContext db,
            ILogger<ALNSService> logger)
        {
            _db = db;
            _logger = logger;

            _options = new ALNSOptions();
        }

        public async Task<List<List<Node>>> SolveFromDbAsync(
            int vehicleCount,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var nodes = await _db.Nodes
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                _distanceMatrix = new DistanceMatrixService(nodes);

                var solution = GenerateInitialSolution(nodes, vehicleCount);

                double bestCost = EvaluateCost(solution);

                double temperature = _options.InitialTemperature;

                int noImprove = 0;

                for (int iteration = 0;
                    iteration < _options.MaxIterations;
                    iteration++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var candidate = CloneSolution(solution);

                    Destroy(candidate);
                    Repair(candidate);

                    double candidateCost = EvaluateCost(candidate);

                    bool accept =
                        candidateCost < bestCost ||
                        AcceptWorse(candidateCost, bestCost, temperature);

                    if (accept)
                    {
                        solution = candidate;

                        if (candidateCost < bestCost)
                        {
                            bestCost = candidateCost;
                            noImprove = 0;
                        }
                    }

                    noImprove++;

                    if (noImprove > _options.EarlyStopping)
                    {
                        break;
                    }

                    temperature *= _options.CoolingRate;
                }

                return solution;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ALNS failed");
                return new List<List<Node>>();
            }
        }

        private List<List<Node>> GenerateInitialSolution(
            List<Node> nodes,
            int vehicleCount)
        {
            var routes = new List<List<Node>>();

            for (int i = 0; i < vehicleCount; i++)
            {
                routes.Add(new List<Node>());
            }

            int index = 0;

            foreach (var node in nodes)
            {
                routes[index % vehicleCount].Add(node);
                index++;
            }

            return routes;
        }

        private double EvaluateCost(List<List<Node>> solution)
        {
            if (_distanceMatrix == null)
            {
                return double.MaxValue;
            }

            double total = 0;

            foreach (var route in solution)
            {
                for (int i = 0; i < route.Count - 1; i++)
                {
                    total += _distanceMatrix.GetDistance(i, i + 1);
                }
            }

            return total;
        }

        private void Destroy(List<List<Node>> solution)
        {
            foreach (var route in solution)
            {
                if (route.Count > 1)
                {
                    route.RemoveAt(Random.Shared.Next(route.Count));
                }
            }
        }

        private void Repair(List<List<Node>> solution)
        {
        }

        private bool AcceptWorse(
            double newCost,
            double currentCost,
            double temperature)
        {
            double probability =
                Math.Exp((currentCost - newCost) / temperature);

            return Random.Shared.NextDouble() < probability;
        }

        private List<List<Node>> CloneSolution(
            List<List<Node>> source)
        {
            return source
                .Select(route => route.ToList())
                .ToList();
        }
    }
}
