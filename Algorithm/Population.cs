using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.IO;



namespace Mag{


    public class Population{

        private List<Individuum> population;
        private DataBase DBLink;

        private int popSize;
        private int maxIter;

        public static Random r;
        private StringBuilder csv = new StringBuilder();

        public Population(DataBase DB, int size, int iterations){
            this.DBLink = DB;
            this.popSize = size;
            this.maxIter = iterations;
            r = new Random();
            population = new List<Individuum>();
            for(int i=0; i<popSize; i++){
                Individuum newInd = new Individuum(DBLink);
                population.Add(newInd);
            }
            csv.AppendLine();
        }


        public void Evolve(){
            population.Sort((x, y) => y.fitness.CompareTo(x.fitness));
            // Write initial individuum to file
            File.AppendAllText("Results/curriculum.csv", $"\nFirst:{population[0].WriteToString()}");

            for(int i=0; i<maxIter; i++){
                Mutate();
                CalculateFitFunc();
                CrossOver();
                
                Selection();

                if(i % Constants.STATS_PRINT_PERIOD == 0 || i==0){
                    PrintEpochResults(i);
                }
            }

            WriteResultsToFile();
        }


        private void Mutate(){
            foreach(var ind in population){
                ind.Mutate();
            }
        }

        private void CalculateFitFunc(){
            foreach(var ind in population){
                ind.CalculateFit();
            }
        }

        private void CrossOver(){
            // Турнирный отбор
            for(int i=0; i<popSize*Constants.CROSS_PROB; i++){
                int r1 = r.Next(0, popSize);
                int r2;
                do{
                    r2 = r.Next(0, popSize);
                }while(r1 == r2);

                var newInd = new Individuum(population[r1], population[r2]);
                population.Add(newInd);
            }
            
        }

        private void Selection(){
            population.Sort((x, y) => y.fitness.CompareTo(x.fitness));
            population.RemoveRange(popSize, population.Count-popSize);
        }

        private void PrintEpochResults(int epoch){
            Console.WriteLine($"Epoch {epoch} population");
            csv.AppendLine();
            csv.Append($"Epoch ; {epoch}; ;");
            foreach(var ind in population){
                Console.WriteLine($"    indiv fitness={ind.fitness}");
                csv.Append($"{ind.fitness}; ");
            }
            Console.WriteLine("\n\n");
        }

        private void WriteResultsToFile(){

            var schedule = population[0].GetSchedule();
            StringBuilder scheduleData = new StringBuilder();
            foreach(var groupId in schedule.Keys){
                scheduleData.AppendLine($"\n\n Schedule for group {groupId}\n");
                for(int pair=0; pair<schedule[groupId].GetLength(1); pair++){
                    for(int weekDay=0; weekDay<schedule[groupId].GetLength(0); weekDay++){
                        if(schedule[groupId][weekDay, pair] != null){
                            var d = schedule[groupId][weekDay, pair];
                            scheduleData.Append($"{d.discipline}, {d.teacherId}, {d.auditoriumId}; ");
                        }
                        else{
                            scheduleData.Append($" - ;");
                        }
                    }
                    scheduleData.AppendLine();
                }
            }

            File.AppendAllText("Results/schedule.csv", scheduleData.ToString());
            File.AppendAllText("Results/curriculum.csv", $"\nLast:{population[0].WriteToString()}");
            File.AppendAllText("Results/output.csv", csv.ToString());
        }

    }

}