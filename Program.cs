using System.Collections.Generic;
using Mag;



DataBase DB = new DataBase();
DB.LoadAllTables();


Population pop = new Population(DB, 25, 100);
pop.Evolve();