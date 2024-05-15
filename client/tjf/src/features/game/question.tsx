import React, { useState } from "react";
import QuestionModel from "./model/questionModel";
import ButtonComponent from "../../shared/design/button/ButtonComponent";
import "../../App.css";


const Question: React.FC = () => {
  const azioneUno = () => {
    console.log("Sono Azione Question");
  };
  const quenstionForUser: QuestionModel = {
    type: "multiple",
    question: "When someone is inexperienced they are said to be what color?",
    answer: ["Green", "Red", "Blue", "Yellow"],
  };

  const [answerQuestion, setAnswerQuestion] = useState(null);

  const onValueChange = (event: any) => {
    setAnswerQuestion(event.target.value);
  };

  const handleSubmit = (event: any) => {
    event.preventDefault();
    
    console.log(answerQuestion);
  };

  return (
    <>
          <h3  style={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            
          }}>Ecco la tua domanda!</h3>
      <div className="question-container">
        <div className="question-header">
        </div>
        <div  className="question-content">
          <h4>{quenstionForUser.question}</h4>
        </div>
      </div>
      <div>
        <div  className="answer-section">
          <h3>Scegli la tua risposta</h3>
        </div>
        <div>
          <form onSubmit={handleSubmit} action="" method="post" className="answer-form">
            <ul className="answer-list">
              {quenstionForUser.answer.map((question, index) => (
                <li key={index} className="answer-item">
                  <label>
                    <input
                      type="radio"
                      value={question}
                      onChange={onValueChange}
                      name="question"
                    ></input>
                    {question}
                  </label>
                </li>
              ))}
            </ul>
            <ButtonComponent type="submit" text="Azione Question" />
          </form>
        </div>
      </div>
    </>
  );
};

export default Question;
