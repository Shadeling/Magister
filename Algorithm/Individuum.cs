using System.Collections.Generic;
using System.Linq;



namespace Mag{


    public class Individuum{
        public DataBase DBLink;

        public UsedTime UsedTime;

        private List<Chromosome> Chromosomes;

        public float fitness;

        public Individuum(DataBase DB){
            this.DBLink = DB;
            this.UsedTime = new UsedTime();
            this.Chromosomes = new List<Chromosome>();
            CreateChromosomes();
        }

        // For crossingover
        public Individuum(Individuum a, Individuum b){
            this.DBLink = a.DBLink;
            this.UsedTime = new UsedTime();
            this.Chromosomes = new List<Chromosome>();

            for(int i=0; i<a.Chromosomes.Count; i++){
                if(a.Chromosomes[i].fitness > a.Chromosomes[i].fitness){
                    this.Chromosomes.Add(new Chromosome(a.Chromosomes[i], this));
                }
                else{
                    this.Chromosomes.Add(new Chromosome(b.Chromosomes[i], this));
                }
            }

            CalculateFit();
        }

        public void CreateChromosomes(){
            var groups = DBLink.ClassGroups.Keys.ToList();
            foreach(var groupId in groups){
                Chromosome newChrom = new Chromosome(this, groupId);
                Chromosomes.Add(newChrom);
            }
        }


        public bool CheckValid(){
            foreach(var chrom in Chromosomes){
                if(!chrom.CheckValid()){
                    return false;
                }
            }

            return true;
        }

        public void Mutate(){
            foreach(var chrom in Chromosomes){
                chrom.MutateAll();
            }
        }

        public void MutateInvalidChroms(){
            foreach(var chrom in Chromosomes){
                chrom.MutateInvalidGenes();
            }
        }

        public void CalculateFit(){
            fitness = 0;
            foreach(var chrom in Chromosomes){
                fitness+= chrom.CalculateFit();
            }
            // Self calcul
            CalcTeacherWindows();

        }

        private void CalcTeacherWindows(){

            foreach(var key in UsedTime.TeacherTimeUsed.Keys){
                var myTimeUsed = UsedTime.TeacherTimeUsed[key];

                for(int i=0; i < myTimeUsed.GetLength(0); i++){
                int pairPerDay =0;
                int windows = 0;
                int winLen = 0;

                for(int j=0; j < myTimeUsed.GetLength(1); j++){
                    if(myTimeUsed[i,j]>0){
                        pairPerDay++;
                        if(winLen>0){
                            windows++;
                            winLen = 0;
                        }
                    }

                    if(pairPerDay >0 && myTimeUsed[i,j]==0){
                        winLen++;
                    }
                    
                }

                fitness+= windows * Weights.TEACHER_WINDOW_WEIGHT;
            }
            }
            
        }
    }
}