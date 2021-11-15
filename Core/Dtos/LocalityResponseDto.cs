namespace Core.Dtos
{
    public class LocalityResponseDto
    {
        public string status { get; set; }
        public Results[] results { get; set; }
    }

    public class Results
    {
        public string formatted_address { get; set; }
    }
}