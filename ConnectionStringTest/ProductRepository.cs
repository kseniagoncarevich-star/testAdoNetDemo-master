using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;

namespace ConnectionStringTest;

// Задание 1: Реализуй метод получения тестовой Test Connection String из json и из .env
public static class ConnectionStringProvider
{
    public static string GetConnectionStringFromJson()
    {
        var jsonText = File.ReadAllText("config.json");
        var jsonDoc = JsonDocument.Parse(jsonText);
        return jsonDoc.RootElement.GetProperty("ConnectionString").GetString();
    }

    public static string GetConnectionStringFromEnv()
    {
        return Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    }

    public static string GetConnectionString()
    {
        return GetConnectionStringFromJson();
        
        // return GetConnectionStringFromEnv();
    }
}

// Задание 2: С помощью Dapper и группы методов Query напишите метод получения общей стоимости всех продуктов из таблицы table_products

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public decimal GetTotalCost()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection.ExecuteScalar<decimal>("SELECT SUM(price) FROM table_products");
    }
    
    // Задание 3: С помощью Dapper и группы методов Execute напишите метод добавления нового продукта (без user_id)
    
    public void AddProduct(product product)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var sql = @"
            INSERT INTO table_products (item_name, price, quantity, is_purchased)
            VALUES (@Name, @Price, @Quantity, @IsPurchased)";
        connection.Execute(sql, product);
    }
    
    // Задание 4: С помощью Dapper и группы методов Query напишите метод получения списка названий продуктов из таблицы table_products
    
    public List<string> GetProductNames()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection.Query<string>("SELECT item_name FROM table_products").ToList();
    }
}