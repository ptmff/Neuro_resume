namespace back.Application.DTOs;

public class SuggestionDto
{
    public string Id { get; set; }           // Например, "sug-{уникальныйId}"
    public string Type { get; set; }         // experience | skills | description | etc
    public string Title { get; set; }
    public string Description { get; set; }
    public double Confidence { get; set; }   // От 0.0 до 1.0
    public object Before { get; set; }       // Может быть строкой или массивом строк
    public object After { get; set; }        // Аналогично
    public string Reasoning { get; set; }
}