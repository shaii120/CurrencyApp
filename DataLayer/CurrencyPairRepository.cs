namespace DataLayer;

using System.Collections.Generic;
using Microsoft.Data.SqlClient;

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
                    Id = (int)reader[0],
                    BaseCode = reader[1].ToString() ?? "",
                    QuoteCode = reader[2].ToString() ?? "",
                    MinValue = (decimal)reader[3],
                    MaxValue = (decimal)reader[4],
                    // Initialize CurrentValue to MaxValue for demonstration purposes
                    CurrentValue = (decimal)reader[4]
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