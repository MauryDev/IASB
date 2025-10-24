namespace Web.View;

public record Informativo(
    DateTime Date,
    string Name,
    string Description,
    string Size, // Ex: "2.5MB"
    string Duration, // Ex: "5m"
    string Url // URL remota do vídeo MP4
);