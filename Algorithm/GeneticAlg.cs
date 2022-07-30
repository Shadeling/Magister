using System.Collections.Generic;
using System.Linq;
using System;



namespace Mag{


    public class Population{

        private List<Individuum> population;
        private DataBase DBLink;

        private int popSize;
        private int maxIter;

        public static Random r;

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
        }


        public void Evolve(){
            for(int i=0; i<maxIter; i++){
                Mutate();
                CalculateFitFunc();
                CrossOver();
                
                Selection();

                if(i % Constants.STATS_PRINT_PERIOD == 0 || i==0){
                    PrintEpochResults(i);
                }
            }
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
            foreach(var ind in population){
                Console.WriteLine($"    indiv fitness={ind.fitness}");
            }
            Console.WriteLine("\n\n");
        }

    }

}