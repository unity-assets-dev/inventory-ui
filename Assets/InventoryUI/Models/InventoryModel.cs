namespace InventoryUI.Inventory {
    
    public class InventoryModel {
        // Possible replace with UniRx
        public IObservableValue<int> Coins { get; } = new ObservableValue<int>(0);
        public IObservableValue<float> Weight { get; } = new ObservableValue<float>(0);

        public void AddCoins(int coins) {
            Coins.Value += coins;
            
        }
    }
}