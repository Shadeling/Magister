using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Mag{


    public class Chromosome{

        private List<Gene> genes;

        private int groupId;

        public Individuum individuum;

        public float fitness {get; private set;}

        public Chromosome(Individuum individuum, int groupId){
            this.individuum = individuum;
            this.groupId = groupId;
            genes = new List<Gene>();
            CreateGenes();
        }

        public Chromosome(Chromosome template,Individuum individuum){
            this.individuum = individuum;
            this.groupId = template.groupId;
            genes = new List<Gene>();

            foreach(var templGene in template.genes){
                genes.Add(new Gene(templGene, individuum));
            }
        }


        public void CreateGenes(){
            var groupCurr = individuum.DBLink.Curricula.Values.Where(x => x.idGroup == groupId).ToList();

            foreach(var pair in groupCurr){
                for(int i =0; i<pair.pairsPerWeek; i++){
                    Gene newGene = new Gene(individuum, pair.discipline, pair.idTeacher, pair.idGroup, pair.lessonType);
                    genes.Add(newGene);
                }
            }
        }

        public bool CheckValid(){
            foreach(var gene in genes){
                if(!gene.CheckValid()){
                    return false;
                }
            }

            return true;
        }

        public void MutateAll(){
            foreach(var gene in genes){
                gene.Mutate();
            }
        }

        public void MutateInvalidGenes(){
            foreach(var gene in genes){
                gene.BecomeValid();
            }
        }

        public float CalculateFit(){
            fitness = 0;
            foreach(var gene in genes){
                fitness+= gene.CalculateFit();
            }
            // Self calcul
            CalcWindows(); 

            return fitness;
        }

        private void CalcWindows(){
            if(individuum.UsedTime.GroupTimeUsed.TryGetValue(groupId, out var myTimeUsed)){

            }else{
                return;
            }
            

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

                fitness+= windows * Weights.STUDENT_WINDOW_WEIGHT;
                if(pairPerDay==1){
                    fitness+= Weights.ONLY_ONE_PAIR_DAY_PENALTY;
                }
            }

            
        }


    }

}