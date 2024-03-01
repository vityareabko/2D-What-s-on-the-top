

using UnityEngine;
using Zenject;

namespace MyNamespace
{
    public class PlayerSpawnData : ScriptableObject
    {
        public Player Player;
        public SpawnPointType SpawnPoint;
        
    }
    
    public class GameplayController11 : MonoBehaviour
    {
        
        private PlayerSpawnData _playerSpawnData;
        
        [Inject] public void Construct(PlayerSpawnData playerSpawnData) => _playerSpawnData = playerSpawnData;
        
        
    }

    // ReSharper disable once InvalidXmlDocComment
    
    /// <summary>
    /// чтобы главное меню было в сцене где и игра (когда игрок стоит на CheckPoint place то показываем ему кнопки главного меню (кнопка магазина, кнопка мастерской, кнопка выбора скина из доступных игроку, Кнопка увеличения статов))
    /// рессурсы которые спавнится чтобы зависили от удачи(стат) игрока чему выше удача тем выше шанс выпадения тех или иных рессурсов
    /// игрка начинается когда игрок свайпает вправо или в лево тогда игрок прыгает на стену и двигается вверх
    /// при проиграше игрока ставится на паузу и спрашывет игрок если он хочет удвоить выйграш монет за рекламу если игрок выбырает нет
    /// то игрока телепертирует на y = 20(в зависимости) и он падает на (CheckPoint place) и ему снова показывается менюшка
    /// </summary>
    ///
    
    /// <summary>
    /// Как я хочу чтобы было : - Done
    /// я хочу чтобы при попадания в obstacle (или стамина заканчилось) игрок его кинуло немного вверх (с какой-то анимации)
    /// а потом когда игрок падает вниз (камера немного идет вверх чтобы игрок был немного ниже центра экранна)
    /// и чтобы он вечно падал (анимация падения) пока игрок не нажмет на кнопки в окне Defeat
    /// - при нажатия игрока (скрываем DefeatScreen) перемещаем игрока в startPosition.y + 30
    /// и он падает на MianMenuPlatform и показываем ему менюшку (MainMenuView) 
    /// 
    /// Что у меня есть :
    /// - у меня сейчась нет анимации,
    /// - есть последний прыжок игрока (срабатывает только на попадения в ловушку),
    /// - нет поведения камеры для проиграша
    /// - игрок вечно не падает
    /// - после нажатия на любых из кнопок DefeatScreen сразу показывается MainMenu
    /// - нет перемещения игрока в startPosition.y + 30 
    ///
    /// 
    /// Что я могу сделаьб :
    /// - сделать поведения камеры а имменно сделать StateMachine 
    /// - сделать чтобы методо который отвечает за Последний прыжок игрока срабатываел и когда стамина кончалась
    /// - сделать анимации 4
    /// 1) отскок от стены (trigger) (not loop)),
    /// 2) когда уже игрок начинает падать (анимация падения (bool)(loop),
    /// 3) игрок пригатавливается к падения (not loop)
    /// 3) когда игрок Collision with MainMenuPlatform - (анимация приземляния (not loop))
    /// 
    /// </summary>

    
  
    
    
     
    
    /// Как я хочу чтобы было :
    /// Так как у меня карта делится на 5 частей (5 tileset) и у каждой части есть свой MainPlatformMenu и он активирувается когда игрок проходит через него
    /// и я хочу чтобы когда игрок достиг этого - то он уже начинал игрру оттуда т.е. менял точку старта игры
    ///
    /// Что у меня есть :
    /// у меня есть тригер перехода игрока на другой тайлсет
    /// - у меня нет сохранения спавн точек
    /// - у меня нет полной сделанной
    ///
    /// Что я могу сделать :
    ///
    /// я могу сделать Config игрока который будет хранить тип спавн точки, префаб игрока.
    /// 
    ///
    /// 



    
    
    
    
    
    
    
}
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
