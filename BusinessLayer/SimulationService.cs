namespace BusinessLayer;

using DataLayer;
using System;
using System.Collections.Generic;
using System.Threading;

public class SimulationService
{
    private readonly CurrencyPairRepository _repo;
    private readonly List<CurrencyPair> _pairs;
    private event Action? PairsUpdatedNotifier;

    public SimulationService(CurrencyPairRepository repo)
    {
        _repo = repo;
        _pairs = _repo.GetAll();
    }
    public List<CurrencyPair> GetPairs()
    {
        return _pairs;
    }
    public void AddNotification(Action callback)
    {
        PairsUpdatedNotifier += callback;
    }
    public void Start()
    {
        Random _rand = new();

        while (true)
        {
            foreach (var p in _pairs)
            {
                // Simulate a random change between -0.05 and +0.05
                var change = (decimal)(_rand.NextDouble() - 0.5) * 0.1m;
                p.CurrentValue += change;
                var newMin = Math.Min(p.MinValue, p.CurrentValue);
                var newMax = Math.Max(p.MaxValue, p.CurrentValue);

                if (newMin != p.MinValue || newMax != p.MaxValue)
                {
                    p.MinValue = newMin;
                    p.MaxValue = newMax;

                    _repo.UpdateMinMax(p.Id, p.MinValue, p.MaxValue);
                }
            }

            PairsUpdatedNotifier?.Invoke();
            Thread.Sleep(2000);
        }
    }
}