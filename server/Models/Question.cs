using Newtonsoft.Json;

namespace server.Models
{
    public class QuestionList
    {
        [JsonProperty(PropertyName ="response_code")] public int ResponseCode { get; set; }
        [JsonProperty(PropertyName = "results")] public List<Question> Questions { get; set; } = [];
    }

    public class Question
    {
        [JsonProperty(PropertyName = "type")] public String Type { get; set; } = String.Empty;
        [JsonProperty(PropertyName ="difficulty")] public String Difficulty { get; set; } = String.Empty;
        [JsonProperty(PropertyName ="category")] public String Category { get; set; } = String.Empty;
        [JsonProperty(PropertyName ="question")] public String QuestionText { get; set; } = String.Empty;
        [JsonProperty(PropertyName ="correct_answer")] public String CorrectAnswer { get; set; } = String.Empty;
        [JsonProperty(PropertyName = "incorrect_answers")] public String[] IncorrectAnswers { get; set; } = [];
    }
}



// {
//   "response_code": 0,
//   "results": [
//     {
//       "type": "multiple",
//       "difficulty": "easy",
//       "category": "Animals",
//       "question": "What do you call a baby bat?",
//       "correct_answer": "Pup",
//       "incorrect_answers": [
//         "Cub",
//         "Chick",
//         "Kid"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "easy",
//       "category": "Entertainment: Music",
//       "question": "The &quot;K&quot; in &quot;K-Pop&quot; stands for which word?",
//       "correct_answer": "Korean",
//       "incorrect_answers": [
//         "Kenyan",
//         "Kazakhstan",
//         "Kuwaiti"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "hard",
//       "category": "Geography",
//       "question": "What is the official German name of the Swiss Federal Railways?",
//       "correct_answer": "Schweizerische Bundesbahnen",
//       "incorrect_answers": [
//         "Schweizerische Nationalbahnen",
//         "Bundesbahnen der Schweiz",
//         "Schweizerische Staatsbahnen"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "medium",
//       "category": "Entertainment: Film",
//       "question": "Who provided a majority of the songs and lyrics for &quot;Spirit: Stallion of the Cimarron&quot;?",
//       "correct_answer": "Bryan Adams",
//       "incorrect_answers": [
//         "Smash Mouth",
//         "Oasis",
//         "Air Supply"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "hard",
//       "category": "History",
//       "question": "When did Spanish Peninsular War start?",
//       "correct_answer": "1808",
//       "incorrect_answers": [
//         "1806",
//         "1810",
//         "1809"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "medium",
//       "category": "Science: Mathematics",
//       "question": "How many books are in Euclid&#039;s Elements of Geometry?",
//       "correct_answer": "13",
//       "incorrect_answers": [
//         "8",
//         "10",
//         "17"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "medium",
//       "category": "Entertainment: Video Games",
//       "question": "Which of these &quot;Worms&quot; games featured 3D gameplay?",
//       "correct_answer": "Worms 4: Mayhem",
//       "incorrect_answers": [
//         "Worms W.M.D",
//         "Worms Reloaded",
//         "Worms: Open Warfare 2"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "medium",
//       "category": "Science: Computers",
//       "question": "Approximately how many Apple I personal computers were created?",
//       "correct_answer": "200",
//       "incorrect_answers": [
//         "100",
//         "500",
//         "1000"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "hard",
//       "category": "Entertainment: Television",
//       "question": "Which of the following actors portrayed the Ninth Doctor in the British television show &quot;Doctor Who&quot;?",
//       "correct_answer": "Christopher Eccleston",
//       "incorrect_answers": [
//         "David Tennant",
//         "Matt Smith",
//         "Tom Baker"
//       ]
//     },
//     {
//       "type": "multiple",
//       "difficulty": "easy",
//       "category": "Celebrities",
//       "question": "Which actress married Michael Douglas in 2000?",
//       "correct_answer": "Catherine Zeta-Jones",
//       "incorrect_answers": [
//         "Ruth Jones",
//         "Pam Ferris",
//         "Sara Sugarman"
//       ]
//     }
//   ]
// }