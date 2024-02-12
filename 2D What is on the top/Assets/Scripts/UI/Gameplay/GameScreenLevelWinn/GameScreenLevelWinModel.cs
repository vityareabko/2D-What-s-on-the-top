using UI.MVP;

namespace UI.GameScreenLevelWinn
{
    public interface IGameScrenLevelWinModel : IModel
    {
        
    }

    public class GameScreenLevelWinModel : IGameScrenLevelWinModel
    {
        // седсь скорее всего будем хранить List<RewardItem> который нужно создаьб
        // метод добовления в List 
        // метод удаления из List
        // и еще Action<RewardItem> чтобы передать View item для добовления в панель с наградами
    }
}