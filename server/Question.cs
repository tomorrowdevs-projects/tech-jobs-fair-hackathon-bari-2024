namespace server.Models
{
    public class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> AllAnswers { get; set; }
    }
}