using System.Text.Json;
using SchoolLibrary.Models;

namespace SchoolLibrary.Services;

public class LibraryService
{
    public List<Book> Books { get; private set; } = new();
    public List<Reader> Readers { get; private set; } = new();
    public List<IssuanceRecord> Issuances { get; private set; } = new();

    private int _nextTicketNumber = 1;

    // Путь к файлу данных (в папке приложения)
    private readonly string _dataFilePath;

    public LibraryService(IWebHostEnvironment environment)
    {
        // Файл будет лежать в папке проекта: SchoolLibrary/AppData/library_data.json
        var appDataPath = Path.Combine(environment.ContentRootPath, "AppData");

        // Создаём папку, если её нет
        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        _dataFilePath = Path.Combine(appDataPath, "library_data.json");

        // Загружаем данные при старте
        LoadData();
    }

    // ===== ЗАГРУЗКА И СОХРАНЕНИЕ =====

    private void LoadData()
    {
        if (!File.Exists(_dataFilePath)) return;

        try
        {
            var json = File.ReadAllText(_dataFilePath);
            var data = JsonSerializer.Deserialize<LibraryData>(json);

            if (data != null)
            {
                Books = data.Books ?? new List<Book>();
                Readers = data.Readers ?? new List<Reader>();
                Issuances = data.Issuances ?? new List<IssuanceRecord>();
                _nextTicketNumber = data.NextTicketNumber > 0 ? data.NextTicketNumber : 1;
            }
        }
        catch
        {
            // Если файл повреждён — начинаем с пустой базы
            Books = new List<Book>();
            Readers = new List<Reader>();
            Issuances = new List<IssuanceRecord>();
            _nextTicketNumber = 1;
        }
    }

    private void SaveData()
    {
        var data = new LibraryData
        {
            Books = Books,
            Readers = Readers,
            Issuances = Issuances,
            NextTicketNumber = _nextTicketNumber
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true  // Красивое форматирование JSON
        };

        var json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(_dataFilePath, json);
    }

    // ===== МЕТОДЫ ДЛЯ РАБОТЫ С КНИГАМИ =====

    public void AddBook(Book book)
    {
        Books.Add(book);
        SaveData();  // ← Сохраняем после каждого изменения
    }

    public bool BookExists(string title, string author) =>
        Books.Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                       b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));

    // ===== МЕТОДЫ ДЛЯ РАБОТЫ С ЧИТАТЕЛЯМИ =====

    public void AddReader(Reader reader)
    {
        reader.TicketNumber = _nextTicketNumber++;
        Readers.Add(reader);
        SaveData();  // ← Сохраняем после каждого изменения
    }

    // ===== МЕТОДЫ ДЛЯ РАБОТЫ С ВЫДАЧЕЙ =====

    public void AddIssuance(IssuanceRecord issuance)
    {
        Issuances.Add(issuance);
        SaveData();  // ← Сохраняем после каждого изменения
    }

    // ===== СТАТИСТИКА =====

    public int TotalBooksCount => Books.Count;
    public int TotalCopies => Books.Sum(b => b.Quantity);
    public int ReadersCount => Readers.Count;
    public int IssuedCount => Issuances.Count;

    // ===== ВСПОМОГАТЕЛЬНЫЙ КЛАСС ДЛЯ JSON =====

    private class LibraryData
    {
        public List<Book> Books { get; set; } = new();
        public List<Reader> Readers { get; set; } = new();
        public List<IssuanceRecord> Issuances { get; set; } = new();
        public int NextTicketNumber { get; set; } = 1;
    }
}