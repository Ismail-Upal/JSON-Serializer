public class Product
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int[]? Subs { get; set; } = Array.Empty<int>();
}

public class Person
{
    public string? Name { get; set; }
    public Product? product { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, object>? Meta { get; set; }
}

class Program
{
    static void Main()
    {
        var root = new Person
        {
            Name = "upal",
            IsActive = true,
            product = new Product
            {
                Name = "laptop",
                Price = 334.5m,
                Subs = new int[] { 3, 3, 3 }
            },
            Meta = new Dictionary<string, object>
            {
                ["theme"] = "dark",
                ["count"] = 42
            }
        };

        string json = JsonSerializer.Serialize(root);
        Console.WriteLine(json);

        // Test calling it TWICE (Bug 1 test)
        string json2 = JsonSerializer.Serialize(root);
        Console.WriteLine(json2);

        // Test string escaping (Bug 3 test)
        var tricky = new Person { Name = "He said \"hello\"\nNew line" };
        Console.WriteLine(JsonSerializer.Serialize(tricky));
    }
}