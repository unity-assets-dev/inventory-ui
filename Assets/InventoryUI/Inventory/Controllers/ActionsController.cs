namespace InventoryUI.Inventory {
    
    public class ActionsController : IExpandable {
        
        private readonly ScreenLayout _screen;
        private readonly ExecutionCommandFactory _commands;

        public ActionsController(ScreenLayout screen, ExecutionCommandFactory commands) {
            _screen = screen;
            _commands = commands;
        }
        
        public void Show() {
            _screen.OnButtonClick<RandomShootAction>(Execute<ShootRandomWeaponCommand>);
            _screen.OnButtonClick<AddAmmoAction>(Execute<AddAmmoToInventoryCommand>);
            _screen.OnButtonClick<AddItemsAction>(Execute<AddItemToInventoryCommand>);
            _screen.OnButtonClick<DeleteItemAction>(Execute<DeleteItemFromInventoryCommand>);
            _screen.OnButtonClick<AddCoinsAction>(Execute<AddRandomCoinsCommand>);
        }

        private void Execute<TCommand>() where TCommand : IExecutionCommand => _commands.Resolve<TCommand>().Execute();
        

        public void Hide() {}
    }
}