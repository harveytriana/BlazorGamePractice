namespace BlazorPong.Models
{
    public class BarModel
    {
        public string PlayerName { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public bool KeyPressed { get; set; }
        public string KeyCode { get; set; } 
        public int Points { get; set; }
    }
}
