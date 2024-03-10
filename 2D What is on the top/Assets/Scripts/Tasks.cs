

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
    /// - сделать анимации 5
    /// 1) отскок от стены (trigger) (not loop)),
    /// 2) игрок падаеть (анимация падения loop),
    /// 3) игрок пригатавливается к приземлению (not loop)
    /// 4) игрок падает (уже приготовленый к падению) (loop)
    /// 3) когда игрок Collision with MainMenuPlatform - (анимация приземляния (not loop))
    /// 
    /// </summary>
    

    // TODO - можно скинам дать что-то типо повышеной броне (что при столкновении камня с игроком то снимаем меньше выносливости)
    
    
    
    
    
    
}
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
