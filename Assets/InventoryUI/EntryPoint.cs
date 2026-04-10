using System.Collections.Generic;
using InventoryUI.Inventory;
using UnityEngine;

public class EntryPoint : MonoBehaviour {
    
    [SerializeField] private InventoryScreen _inventoryScreen;
    [SerializeField] private TemplateFactory _templateFactory;
    
    private readonly List<IExpandable> _expansions = new ();
    
    private InventoryController _inventoryControllerTester;
    private StatsController _statsController;
    private ActionsController _actionsController;
    private InventorySerialization _serialization;
    
    private void OnEnable() {
        // Or using Zenject;
        // container.BindInterfacesAndSelfTo<Inventory>().AsSingle().NonLazy();
        // container.BindInterfacesAndSelfTo<InventoryModel>().AsSingle().NonLazy();
        // container.BindInterfacesAndSelfTo<InventoryLogger>().AsSingle().NonLazy();
        // etc
        
        var inventory = new Inventory(50);
        var model = new InventoryModel();
        var logger = new InventoryLogger();
        
        // serialized container for inventory with ISerializationContainer
        // to wrap are InventoryModel and Inventory
        // using strategy pattern between LocalStorageContainer and PlayerPrefContainer
        _serialization = new InventorySerialization(model, inventory, new LocalStorageContainer());
        _serialization.Deserialize(_templateFactory); // load serialized inventory;

        // action commands
        var commands = new ExecutionCommandFactory(new IExecutionCommand[] {
            new AddRandomCoinsCommand(model, logger), 
            new AddItemToInventoryCommand(_templateFactory, inventory, logger),
            new DeleteItemFromInventoryCommand(inventory, logger),
            new AddAmmoToInventoryCommand(_templateFactory, inventory, logger),
            new ShootRandomWeaponCommand(inventory, logger)
        });
        
        // shortening invokes by interface
        _expansions.Add(new ActionsController(_inventoryScreen, commands));
        _expansions.Add(new StatsController(_inventoryScreen, model));
        _expansions.Add(new InventoryController(_inventoryScreen, model, inventory));
        
        _inventoryScreen.Show();
        
        foreach (var expandable in _expansions) expandable.Show();
    }
    

    private void OnDisable() {
        _serialization.Serialize(); // save on close;
        
        foreach (var expandable in _expansions) expandable.Hide();
    }
}

