namespace InventoryUI.Inventory {
    public class StatsController : IExpandable {
        
        private readonly InventoryModel _model;
        private readonly ScreenLayout _screen;
        private StatsExpansion _stats;

        public StatsController(ScreenLayout screen, InventoryModel model) {
            _model = model;
            _screen = screen;
        }

        public void Show() {
            if (_screen.TryGetElement<StatsExpansion>(out var stats)) {
                _stats = stats;
                
                _model.Coins.AddListener(_stats.UpdateCoins);
                _model.Weight.AddListener(_stats.UpdateWeight);
            }
        }

        public void Hide() {
            if (_stats != null) {
                _model.Coins.RemoveListener(_stats.UpdateCoins);
                _model.Weight.RemoveListener(_stats.UpdateWeight);
            }
        }
    }
}