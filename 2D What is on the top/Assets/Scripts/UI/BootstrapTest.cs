using UnityEngine;

namespace UI
{
    public class BootstrapTest : MonoBehaviour
    {
        [SerializeField] private GameScreenView gameScreenViewPrefab; // Предполагается, что это Prefab с компонентом GameScreenView
        [SerializeField] private Transform _parentGameScreenViewPrefab;

        private GameScreenModel _model;
        
        private void Start()
        {
            _model = new GameScreenModel(0); // Инициализируем модель с начальным счётом 0
            var view = Instantiate(gameScreenViewPrefab, _parentGameScreenViewPrefab); // Создаём представление на сцене
            var presenter = new GameScreenPresenter(_model, view); // Создаём презентер с моделью и представлением

            presenter.Init(); // Инициализируем презентер
        }

        [ContextMenu("ss")]
        private void addScoreTest()
        {
            _model.AddScore(10);
            // Debug.Log(_model.Score);
        }
    }
}