using DataBaseHandler.Model.Host;
using LiteDB;
using System;

namespace DatabaseHandler
{
    public class Class1
    {
        


        public void SetupDatabase()
        {
            var mapper = BsonMapper.Global;
            mapper.Entity<PlayerModel>()
                .DbRef(x => x.GameGuid, "GameGuid");
            using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
            {
                var col = db.GetCollection<PlayerModel>("Players");
                var game = db.GetCollection<MainGameModel>("Maingame");
                var gameGuid = Guid.NewGuid();
                var player = new PlayerModel
                {
                    PlayerGuid = Guid.NewGuid(),
                    gameGuid = game
                };
                /*var player = new PlayerModel
                {
                    PlayerGuid = Guid.NewGuid();
                GameGuid = new MainGameModel
                {
                    GameGuid = Guid.NewGuid();
                */
            };

                
            }
            
        }
    }
}
