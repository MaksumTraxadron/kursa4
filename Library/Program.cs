using SchoolLibrary.Components;
using SchoolLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Регистрируем наш сервис библиотеки (Singleton = один на всё приложение)
builder.Services.AddSingleton<LibraryService>();

var app = builder.Build();

// Настройка middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();