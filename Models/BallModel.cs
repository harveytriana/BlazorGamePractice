namespace BlazorPong.Models
{
    public class BallModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int State { get; set; }
        public int Direction  { get; set; }

        public override string ToString()
        {
            return $"(x, y): ({X}, {Y}) S: {State} D: {Direction}";
        }
    }
}
