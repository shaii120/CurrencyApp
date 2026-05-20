namespace DataLayer;

using System.Collections.Generic;
using Microsoft.Data.SqlClient;

enum PairReader
{
    Id = 0,
    BaseCode = 1,
    QuoteCode = 2,
    MinValue = 3,
    MaxValue = 4
}

public class CurrencyPairRepository
{
    private readonly string _connectionString;

    public CurrencyPairRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<CurrencyPair> GetAll()
    {
        var list = new List<CurrencyPair>();

        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"
                SELECT cp.Id, b.Code, q.Code, cp.MinValue, cp.MaxValue
                FROM CurrencyPair cp
                JOIN Currency b ON cp.BaseCurrencyId = b.Id
                JOIN Currency q ON cp.QuoteCurrencyId = q.Id",
                conn);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CurrencyPair
                {
                    Id = (int)reader[(int)PairReader.Id],
                    BaseCode = reader[(int)PairReader.BaseCode].ToString() ?? "",
                    QuoteCode = reader[(int)PairReader.QuoteCode].ToString() ?? "",
                    MinValue = (decimal)reader[(int)PairReader.MinValue],
                    MaxValue = (decimal)reader[(int)PairReader.MaxValue],
                    // Initialize CurrentValue to start in the middle of the range for demonstration purposes
                    CurrentValue = (
                            (decimal)reader[(int)PairReader.MaxValue] +
                            (decimal)reader[(int)PairReader.MinValue]
                        ) / 2
                });
            }
        }
        return list;
    }

    public void UpdateMinMax(int id, decimal min, decimal max)
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            var cmd = new SqlCommand(@"
                UPDATE CurrencyPair
                SET MinValue=@min, MaxValue=@max
                WHERE Id=@id", conn);

            cmd.Parameters.AddWithValue("@min", min);
            cmd.Parameters.AddWithValue("@max", max);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}