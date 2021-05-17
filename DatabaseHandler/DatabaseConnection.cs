using DataBaseHandler.Model.Host;
using LiteDB;
using System;
using Microsoft.VisualBasic;

namespace DatabaseHandler
{
    public class DatabaseConnection
    {
        private string _collection;

        public void SetupDatabase()
        {
           
            using (var db = new LiteDatabase(@"C:\\Temp\\MyData.db"))
            {
                var col = db.GetCollection<PlayerModel>(GetCollectionName<PlayerModel>());
                var game = db.GetCollection<MainGameModel>(GetCollectionName<MainGameModel>());
                var gameGuid = Guid.NewGuid();

                LiteDB.BsonMapper.Global.Entity<PlayerModel>()
                    .DbRef(x => x.GameGuid, GetCollectionName<MainGameModel>());

                var mainGame = new MainGameModel()
                {
                    MainGameGuid = gameGuid
                };

                var player = new PlayerModel
                {
                    PlayerGuid = Guid.NewGuid(),
                    GameGuid = mainGame
                };

                try
                {
                    game.Insert(mainGame);
                    col.Insert(player);
                }
                catch(Exception e)
                {
                    throw e; 
                }


                var all = db.GetCollection<PlayerModel>(GetCollectionName<PlayerModel>());

                var all_all = all.Include(x => x.GameGuid).FindAll();
                Console.WriteLine(all_all);
            } ;
        }

        public string GetCollectionName<T>()
        {
            var name = typeof(T).Name + "s";
            return name;
        }
    }
}
 
