using System.Collections.Generic;
using Mag;



DataBase DB = new DataBase();
DB.LoadAllTables();


Population pop = new Population(DB, 50, 500);
pop.Evolve();