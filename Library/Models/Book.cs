namespace SchoolLibrary.Models;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string GenreClass => GetGenreClass(Genre);

    private static string GetGenreClass(string genre) => genre switch
    {
        "Классика" => "genre-classic",
        "Фантастика" => "genre-fantasy",
        "Наука" => "genre-science",
        "История" => "genre-history",
        "Детектив" => "genre-fiction",
        "Приключения" => "genre-fiction",
        "Учебник" => "genre-science",
        _ => "genre-fiction"
    };
}