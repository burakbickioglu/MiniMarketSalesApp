namespace BaseProject.Domain.Models.Dtos.GeneralContent;

public class GeneralContentDTO
{
    public Guid? Id { get; set; }
    //public ContentType Type { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
}
