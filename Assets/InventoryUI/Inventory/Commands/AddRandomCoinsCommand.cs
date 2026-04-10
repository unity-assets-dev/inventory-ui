using InventoryUI.Inventory;
using UnityEngine;

public class AddRandomCoinsCommand : IExecutionCommand {
    
    private readonly InventoryModel _model;
    private readonly IInventoryLogger _logger;

    public AddRandomCoinsCommand(InventoryModel model, IInventoryLogger logger) {
        _model = model;
        _logger = logger;
    }
    
    public void Execute() {
        // TODO: Add random coins from 9 to 99
        // TODO: Debug log:  Добавлено ([количество]) монет
        var coins = Random.Range(9, 100);
        _model.AddCoins(coins);
        _logger.OnCoinsAdded(coins);
    }
}