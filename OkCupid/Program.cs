using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OkCupid.Comparer;
using OkCupid.JsonUtils;

namespace OkCupid
{
    class Program
    {

        static void Main(string[] args)
        {
            // Entries
            const string INPUT_FILE = "input.json";
            const string OUTPUT_FILE = "output.json";
            const int NUMBER_TOP_PROFILE = 10;
            int[] IMPORTANCE_POINTS = new int[] { 0, 1, 10, 50, 250 };

            // Open the file input
            using (StreamReader file = File.OpenText(INPUT_FILE))
            {
                // Serialise json to object
                JsonSerializer serializer = new JsonSerializer();
                JsonCustomReader reader = (JsonCustomReader)serializer.Deserialize(file, typeof(JsonCustomReader));

                // get all profiles
                List<Profile> listProfils = reader.profiles;

                // clone listProfils
                List<Profile> listLoopedProfils = new List<Profile>(listProfils);

                // Initialize the writer
                JsonCustomWriter jsonCustomWriter = new JsonCustomWriter();

                foreach (Profile actualProfil in listProfils)
                {
                    for(int i=0; i<listLoopedProfils.Count; i++)
                    {
                        // Get the compared profil
                        Profile comparedProfil = listLoopedProfils[i];

                        // We don't compare the profil with itself
                        if (!comparedProfil.Equals(actualProfil))
	                    {
                            // List of answers of others
                            List<Answer> answersComparedProfil = comparedProfil.answers;
                            // List of answers of actual profil
                            List<Answer> answersActualProfil = actualProfil.answers;
                            // Temporary answerProfil for the Intersect
                            List<Answer> tempAnswersProfil = new List<Answer>(answersComparedProfil);

                            // Remove answers that are not in actualProfil answers (By IdQuestion)
                            answersComparedProfil = answersComparedProfil.Intersect(answersActualProfil, new AnswerComparer()).ToList();
                            // Sort the list by IdQuestion
                            answersComparedProfil.Sort((a, b) => a.questionId.CompareTo(b.questionId));
                            

                            // Remove answers that are not in answerProfil answers (By IdQuestion)
                            answersActualProfil = answersActualProfil.Intersect(tempAnswersProfil, new AnswerComparer()).ToList();
                            // Sort the list by IdQuestion
                            answersActualProfil.Sort((a, b) => a.questionId.CompareTo(b.questionId));


                            int importanceTotalActual = 0;
                            int importanceTotalCompared = 0;
                            int happinessActual = 0;
                            int happinessCompared = 0;

                            // Loop into answers
                            for (int j = 0; j < answersActualProfil.Count; j++)
	                        {
	                            Answer answerActual = answersActualProfil[j];
	                            Answer answerCompared = answersComparedProfil[j];

                                // Calculate ActualProfil happiness
                                importanceTotalActual += IMPORTANCE_POINTS[answerActual.importance];
                                // If there is too much or none acceptableAnswer we dont add the importance
                                if (answerActual.acceptableAnswers.Count != 4)
	                            {
                                    if(answerActual.acceptableAnswers.Contains(answerCompared.answer))
	                                    happinessActual += IMPORTANCE_POINTS[answerActual.importance];                             
	                            }

                                // Calculate ComparedProfil happiness
                                importanceTotalCompared += IMPORTANCE_POINTS[answerCompared.importance];
                                // If there is too much or none acceptableAnswer we dont add the importance
                                if (answerCompared.acceptableAnswers.Count != 4)
                                {
                                    if(answerCompared.acceptableAnswers.Contains(answerCompared.answer))
                                        happinessCompared += IMPORTANCE_POINTS[answerCompared.importance];
                                }
                            }

                            // The total match of each other
	                        double calculatedMatchActual = (double)happinessActual / (double)importanceTotalActual;
	                        double calculatedMatchCompared = (double)happinessCompared / (double)importanceTotalCompared;

                            // The match of both
                            double calculatedScoreMatch = Math.Sqrt(calculatedMatchActual * calculatedMatchCompared);
                            
                            // Calculate the real match
	                        double marginEror = (double) 1/answersActualProfil.Count;
                        
                            double realScoreMatch = calculatedScoreMatch - marginEror;

                            // If real score match is negative we put zero instead
	                        if (realScoreMatch < 0)
	                            realScoreMatch = 0;

                            // Add the profil to the actualProfil matches
                            actualProfil.AddMatch(new Match(comparedProfil.id, Math.Round(realScoreMatch, 2)));

                            // Add the actualProfil to the profil matches (Gain of performance)
                            comparedProfil.AddMatch(new Match(actualProfil.id, Math.Round(realScoreMatch, 2)));

                        }
                    }

                    // Create a record with 10 top matchs
                    actualProfil.getListMatches().Sort(new MatchComparer());
                    Result record = new Result(actualProfil.id, actualProfil.getListMatches().Take(NUMBER_TOP_PROFILE).ToList() );

                    // Add result to the RootObject
                    jsonCustomWriter.AddResult(record);

                    // Remove the actualProfil as it has been added by everyone
                    listLoopedProfils.Remove(actualProfil);
                }

                // Serialise and write to Json file
                string json = JsonConvert.SerializeObject(jsonCustomWriter, Formatting.Indented);
                File.WriteAllText(OUTPUT_FILE, json);
            }

        }
    }
}
